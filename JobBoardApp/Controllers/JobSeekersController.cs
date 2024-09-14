using AutoMapper;
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
    /// Controller for handling job seeker-related operations.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class JobSeekersController : ControllerBase
    {
        private readonly JobBoardContext context;
        private readonly IMapper mapper;
        private readonly ErrorMessages Messages;
        public JobSeekersController(JobBoardContext context, IMapper mapper, ErrorMessages Messages)
        {
            this.context = context;
            this.mapper = mapper;
            this.Messages = Messages;
        }

        /// <summary>
        /// Gets the list of all job seekers.
        /// </summary>
        /// <returns>A list of job seekers.</returns>
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> GetJobSeekers()
        {
            var jobseekers = await context.JobSeekers.ToListAsync();
            var jobseekersdto = mapper.Map<List<JobSeekerDTO>>(jobseekers);
            return Ok(jobseekersdto);
        }

        /// <summary>
        /// Gets a job seeker by their ID.
        /// </summary>
        /// <param name="id">The ID of the job seeker.</param>
        /// <returns>The job seeker with the specified ID, if found.</returns>
        [HttpGet("Get-By-Id/{jobseeker_id}")]
        public async Task<ActionResult<JobSeeker>> GetJobSeeker(int id)
        {
            var jobSeeker = await context.JobSeekers.FindAsync(id);

            if (jobSeeker == null)
            {
                return NotFound($"The Requested Job Seeker With Id: {id} does not Exist!");
            }
            var jobseekerdto=mapper.Map<JobSeekerDTO>(jobSeeker);
            return Ok(jobseekerdto);
        }

        /// <summary>
        /// Adds a new job seeker, if there is a JobSeeker with the same Id, an exception will be thrown.
        /// </summary>
        /// <param name="jobSeeker">The job seeker to add.</param>
        /// <returns>The created job seeker.</returns>
        [HttpPost("Create")]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> PostJobSeeker([FromBody] JobSeeker jobseeker)
        {
            var existingJobSeeker = await context.JobSeekers.FindAsync(jobseeker.Id);
            if (existingJobSeeker != null)
            {
                throw new InvalidOperationException(Messages.JobSeekerIDExists);
            }

            await context.JobSeekers.AddAsync(jobseeker);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobSeeker), new { id = jobseeker.Id }, jobseeker);
        }
        /// <summary>
        /// Updates an existing job seeker.
        /// </summary>
        /// <param name="id">The unique identifier of the job seeker to update.</param>
        /// <param name="jobSeeker">The job seeker object containing updated information.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpPut("Edit/{jobseeker_id}")]
        public async Task<IActionResult> PutJobSeeker([FromRoute] int id, [FromBody] JobSeeker jobSeeker)
        {
            if (id != jobSeeker.Id)
            {
                return BadRequest(Messages.JboSeekerIdMismatch);
            }

            context.SetModified(jobSeeker);

            try
            {
                await context.SaveChangesAsync();
                return Ok(jobSeeker); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.EntityExists<JobSeeker>(id))
                {
                    return NotFound(new { Message = Messages.JobSeekerIdNotExist });
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
        /// Deletes an existing job seeker.
        /// </summary>
        /// <param name="id">The unique identifier of the job seeker to delete.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpDelete("Delete/{jobseeker_id}")]
        public async Task<IActionResult> DeleteJobSeeker([FromRoute] int id)
        {
            var jobSeeker = await context.JobSeekers.FindAsync(id);
            if (jobSeeker == null)
            {
                return NotFound(new { Message = Messages.JobSeekerIdNotExist });
            }

            context.JobSeekers.Remove(jobSeeker);
            await context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
