using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginAppApi.Data;
using LoginAppApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly LoginAppDbContext _context;

        public TasksController(LoginAppDbContext context)
        {
            _context = context;
        }

        [HttpGet("tasks")]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskEntity>> CreateTask(TaskEntity task)
        {
            task.CreatedAt = DateTime.UtcNow;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Return the created task with its ID
            return Ok(task);
        }

        [HttpPut("id/{id}")]
        public async Task<ActionResult<TaskEntity>> UpdateTaskById(int id, TaskEntity task)
        {
            if (id != task.Id)
            {
                return BadRequest(new { message = "Task ID mismatch" });
            }

            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found" });
            }

            existingTask.TaskName = task.TaskName;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.StartDate = task.StartDate;
            existingTask.EndDate = task.EndDate;

            _context.Entry(existingTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingTask);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Error updating the task" });
            }
        }

        [HttpDelete("id/{id}")]
        public async Task<ActionResult<TaskEntity>> DeleteTaskById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found" });
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }
    }
}
