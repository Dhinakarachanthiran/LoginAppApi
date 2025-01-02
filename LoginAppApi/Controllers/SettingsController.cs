using LoginAppApi.Data;
using LoginAppApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly LoginAppDbContext _context;

        // Constructor to inject the DbContext
        public SettingsController(LoginAppDbContext context)
        {
            _context = context;
        }

        // --------------------------- Department APIs --------------------------

        // GET: api/settings/departments
        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                // Retrieve all departments and select only the required fields
                var departments = await _context.Departments
                                                .Select(d => new
                                                {
                                                    d.DepartmentId,
                                                    d.DepartmentName,
                                                    d.DepartmentDescription
                                                })
                                                .ToListAsync();

                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving departments: {ex.Message}");
            }
        }

        // GET: api/settings/departments/{id}
        [HttpGet("departments/{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            try
            {
                var department = await _context.Departments
                                               .Where(d => d.DepartmentId == id)
                                               .Select(d => new
                                               {
                                                   d.DepartmentId,
                                                   d.DepartmentName,
                                                   d.DepartmentDescription
                                               })
                                               .FirstOrDefaultAsync();

                // Return NotFound if department doesn't exist
                if (department == null)
                {
                    return NotFound($"Department with ID {id} not found.");
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving department with ID {id}: {ex.Message}");
            }
        }

        // POST: api/settings/departments
        [HttpPost("departments")]
        public async Task<IActionResult> PostDepartment([FromBody] Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest("Department data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid department data");
                }

                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                // Return only a success message
                return Ok(new { Message = "Department created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating department: {ex.Message}");
            }
        }

        // DELETE: api/settings/departments/{id}
        [HttpDelete("departments/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound($"Department with ID {id} not found.");
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"Department with ID {id} has been successfully deleted." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting department with ID {id}: {ex.Message}");
            }
        }
        // --------------------------- Category APIs --------------------------

        // GET: api/settings/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving categories: {ex.Message}");
            }
        }

        // GET: api/settings/categories/{id}
        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving category with ID {id}: {ex.Message}");
            }
        }

        // POST: api/settings/categories
        [HttpPost("categories")]
        public async Task<IActionResult> PostCategory([FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Category data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid category data");
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                // Return only a success message
                return Ok(new { Message = "Category created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating category: {ex.Message}");
            }
        }

        // DELETE: api/settings/categories/{id}
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"Category with ID {id} has been successfully deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting category with ID {id}: {ex.Message}");
            }
        }

        // GET: api/settings/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                // Retrieve all roles and select only the necessary fields
                var roles = await _context.Roles
                                          .Select(r => new
                                          {
                                              r.RoleId,
                                              r.RoleTitle,
                                              r.RolePermissions
                                          })
                                          .ToListAsync();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error retrieving roles.", Details = ex.Message });
            }
        }

        // GET: api/roles/{id}
        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var role = await _context.Roles
                                         .Where(r => r.RoleId == id)
                                         .Select(r => new
                                         {
                                             r.RoleId,
                                             r.RoleTitle,
                                             r.RolePermissions
                                         })
                                         .FirstOrDefaultAsync();

                // Return NotFound if role doesn't exist
                if (role == null)
                {
                    return NotFound(new { Message = $"Role with ID {id} not found." });
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the role.", Details = ex.Message });
            }
        }

        // POST: api/roles
        [HttpPost("roles")]
        public async Task<IActionResult> PostRole([FromBody] Role role)
        {
            try
            {
                if (role == null)
                {
                    return BadRequest(new { Message = "Role data cannot be null." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Message = "Invalid role data." });
                }

                role.CreatedAt = DateTime.UtcNow;

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                // Return only a success message
                return Ok(new { Message = "Role created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error creating role.", Details = ex.Message });
            }
        }

        // DELETE: api/roles/{id}
        [HttpDelete("roles/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                if (role == null)
                {
                    return NotFound(new { Message = $"Role with ID {id} not found." });
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"Role with ID {id} has been successfully deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while deleting the role.", Details = ex.Message });
            }
        }


        // POST: api/settings/clients
        [HttpPost("clients")]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            try
            {
                if (client == null)
                {
                    return BadRequest("Invalid client data.");
                }

                var existingClient = await _context.Clients
                    .AnyAsync(c => c.ClientId == client.ClientId);

                if (existingClient)
                {
                    return BadRequest("ClientId already exists. Please use a unique ClientId.");
                }

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                // Return only a success message
                return Ok(new { Message = "Client created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating client: {ex.Message}");
            }
        }

        // GET: api/settings/clients
        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _context.Clients
                .Select(c => new
                {
                    c.ClientId,
                    c.ClientName,
                    c.ClientLocation
                })
                .ToListAsync();

            return Ok(clients);
        }

    }
}
