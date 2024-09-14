using JobBoardApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace JobBoardApp.Controllers
{
    /// <summary>
    /// Controller for handling jobPosting-related operations.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "JobPolicy")]
    public class JobsController : ControllerBase
    {
        private readonly JobBoardContext context;
        private readonly ErrorMessages Messages;

        public JobsController(JobBoardContext context, ErrorMessages Messages)
        {
            this.context = context;
            this.Messages = Messages;
        }
        /// <summary>
        /// Gets the list of all jobs.
        /// </summary>
        /// <returns>A list of jobs.</returns>

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            var jobs = await context.Jobs.ToListAsync();
            return Ok(jobs);
        }

        /// <summary>
        /// Gets a job by its ID.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        /// <returns>The job with the specified ID, if found.</returns>
        [HttpGet("Get-By-Id/{job_id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound($"The Requested Job Posting With Id{id} does not Exist!");
            }

            return Ok(job);
        }

        /// <summary>
        /// Adds a new job, if there is a Job with the same Id, an exception will be thrown.
        /// </summary>
        /// <param name="job">The job to add.</param>
        /// <returns>The created job.</returns>
        [HttpPost("Create")]
        [Authorize(Roles = "Employer")]
        public async Task<ActionResult<IEnumerable<Job>>> PostJob([FromBody] Job job)
        {
            var existingJob = await context.Jobs.FindAsync(job.Id);
            if (existingJob != null)
            {
                throw new InvalidOperationException(Messages.JobIdWithSameId);
            }

            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job);
        }
        /// <summary>
        /// Updates an existing job posting.
        /// </summary>
        /// <param name="id">The unique identifier of the job to update.</param>
        /// <param name="job">The job object containing updated information.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpPut("Edit/{job_id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> PutJob([FromRoute] int id, [FromBody] Job job)
        {
            if (id != job.Id)
            {
                return BadRequest(Messages.JobIdMismatch);
            }

            bool employerExists = await context.IsEmployerExistsAsync(job.EmployerID);
            if (!employerExists)
            {
                return BadRequest(new { Message = Messages.EmplyerIdNotFound }); 
            }

            context.SetModified(job);

            try
            {
                await context.SaveChangesAsync();
                return Ok(job); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.EntityExists<Job>(id))
                {
                    return NotFound(new { Message = Messages.JobIDNotExists }); 
                }
                else
                {
                    return Conflict(new { Message = Messages.ConcurrencyConflict });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = Messages.NotExpectedError });
            }
        }

        /// <summary>
        /// Deletes an existing job posting.
        /// </summary>
        /// <param name="id">The unique identifier of the job to delete.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpDelete("Delete/{job_id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeleteJob([FromRoute] int id)
        {
            var job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound(new { Message = Messages.JobIDNotExists });
            }

            context.Jobs.Remove(job);
            await context.SaveChangesAsync();

            return NoContent(); 
        }
        /*
        [HttpGet("throw")]
        public IActionResult ThrowException()
        {
            throw new Exception("This is a test exception for middleware.");
        }*/
    }
}
