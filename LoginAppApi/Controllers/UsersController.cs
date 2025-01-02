using LoginAppApi.Data;
using LoginAppApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LoginAppDbContext _context;

        public UsersController(LoginAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] User loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required");
            }

            // Validate user credentials
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Fetch the screens and access details directly from Access table based on HideShow
            var accessDetails = await _context.Access
                                               .Where(a => a.Role == user.Role && a.HideShow) // Use HideShow to filter visible screens
                                               .Select(a => new
                                               {
                                                   a.ScreenId,
                                                   screenname = a.ScreenName, // Changed to screenname
                                                   FullAccess = a.FullAccess ? 1 : 0,
                                                   ReadOnly = a.ReadOnly ? 1 : 0,
                                               })
                                               .ToListAsync();

            var token = "dummy-jwt-token"; // Replace with actual token generation logic.

            return Ok(new
            {
                message = "Login successful",
                username = user.Username, // Add username to the response
                role = user.Role,
                token,
                accessDetails
            });
        }

        [HttpGet("get-role")]
        public async Task<ActionResult> GetRole([FromQuery] string username, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username and password are required");
            }

            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return NotFound("User not found or invalid credentials");
            }

            var response = new
            {
                username = user.Username,
                role = user.Role
            };

            return Ok(response);
        }

        [HttpPost("save-access-by-role")]
        public async Task<IActionResult> SaveAccessPermissionsByRole([FromBody] AccessRequest accessRequest)
        {
            if (accessRequest == null || accessRequest.AccessData == null || accessRequest.AccessData.Count == 0)
            {
                return BadRequest("No access data provided.");
            }

            try
            {
                foreach (var access in accessRequest.AccessData)
                {
                    // If HideShow is false, set FullAccess and ReadOnly to false
                    if (!access.HideShow)
                    {
                        access.FullAccess = false;
                        access.Read = false; // Assuming "Read" is the correct field for ReadOnly
                    }

                    // Parse ScreenId if it's a string (assuming access.ScreenId is a string)
                    int screenId = int.Parse(access.ScreenId);

                    // Find if there is an existing record by checking Role and ScreenId
                    var existingAccess = await _context.Access
                                                       .FirstOrDefaultAsync(a => a.ScreenId == screenId && a.Role == accessRequest.Role);

                    if (existingAccess != null)
                    {
                        // Update existing access record
                        existingAccess.ScreenName = access.ScreenName; // Changed from access.Screen
                        existingAccess.HideShow = access.HideShow;
                        existingAccess.FullAccess = access.FullAccess;
                        existingAccess.ReadOnly = access.Read; // Assuming "Read" is the correct field for ReadOnly
                    }
                    else
                    {
                        // Insert new record into Access table
                        _context.Access.Add(new Access
                        {
                            ScreenName = access.ScreenName, // Changed from access.Screen
                            ScreenId = screenId,
                            Role = accessRequest.Role,
                            HideShow = access.HideShow,
                            FullAccess = access.FullAccess,
                            ReadOnly = access.Read // Assuming "Read" is the correct field for ReadOnly
                        });
                    }

                    // Update Screens table IsVisible field for new record (if needed)
                    var screen = await _context.screens
                                                .FirstOrDefaultAsync(s => s.ScreenId == screenId);
                    if (screen != null)
                    {
                        screen.IsVisible = access.HideShow;  // Set IsVisible in Screens table
                    }
                }

                // Save changes to Access table
                await _context.SaveChangesAsync();
                return Ok(new { message = "Access permissions saved/updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-access-by-role/{role}")]
        public async Task<IActionResult> GetAccessPermissionsByRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return BadRequest("Role is required.");
            }

            try
            {
                // Retrieve distinct access data for the specified role
                var accessData = await _context.Access
                                               .Where(a => a.Role == role)
                                               .Join(_context.screens,
                                                     a => a.ScreenId,
                                                     s => s.ScreenId,
                                                     (a, s) => new
                                                     {
                                                         a.ScreenId,
                                                         ScreenName = a.ScreenName, // Changed to screenname
                                                         a.HideShow,
                                                         a.FullAccess,
                                                         a.ReadOnly,
                                                         s.IsVisible
                                                     })
                                               .GroupBy(x => new { x.ScreenId, x.ScreenName }) // Group by ScreenId and screenname
                                               .Select(g => g.First()) // Select only the first record from each group
                                               .ToListAsync();

                if (accessData == null || accessData.Count == 0)
                {
                    return NotFound("No access data found for the specified role.");
                }

                return Ok(accessData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                                      .Select(u => new
                                      {
                                          u.UserId,
                                          u.Username,
                                          u.Password,
                                          u.Role
                                      })
                                      .ToListAsync();

            return Ok(users);
        }//

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password))
            {
                return BadRequest("Username, password, and role are required.");
            }

            // Check if the username already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username);
            if (existingUser != null)
            {
                return Conflict("A user with the same username already exists.");
            }

            try
            {
                // Add the new user to the database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsers), new { id = newUser.UserId }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }
        [HttpPost("create-user")]  // Modified to avoid conflict
        public async Task<IActionResult> CreateUser([FromBody] AddUser user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }

            // Validate data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Add user to the database
                _context.AddUsers.Add(user);
                await _context.SaveChangesAsync();

                // Return success response
                return CreatedAtAction(nameof(CreateUser), new { id = user.UserName }, user);
            }
            catch (Exception ex)
            {
                // Catch and display a more specific error message
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
