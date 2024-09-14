using JobBoardApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JobBoardApp.Controllers
{
    /// <summary>
    /// Controller for handling Jop Application-related operations.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "JobPolicy")]
    public class ApplicationsController : ControllerBase
    {
        private readonly JobBoardContext context;
        private readonly ErrorMessages Messages;

        public ApplicationsController(JobBoardContext context, ErrorMessages Messages)
        {
            this.context = context;
            this.Messages = Messages;
        }

        /// <summary>
        /// Gets the list of all applications.
        /// </summary>
        /// <returns>A list of applications.</returns>
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            var applications = await context.Applications.ToListAsync();
            return Ok(applications);
        }

        /// <summary>
        /// Gets an application by its ID.
        /// </summary>
        /// <param name="id">The ID of the application.</param>
        /// <returns>The application with the specified ID, if found.</returns>
        [HttpGet("Get-By-Id/{application_id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var application = await context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound($"The Requested Application with Id: {id} does not Exist!");
            }

            return Ok(application);
        }


        /// <summary>
        /// Adds a new application, if there is an Application with the same Id, an exception will be thrown.
        /// </summary>
        /// <param name="application">The application to add.</param>
        /// <returns>The created application.</returns>
        [HttpPost("Create")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<ActionResult<IEnumerable<Application>>> PostApplication([FromBody] Application application)
        {
            var existingApplication = await context.Applications.FindAsync(application.Id);
            if (existingApplication != null)
            {
                throw new InvalidOperationException(Messages.ApplicationWithSameId);
            }

            await context.Applications.AddAsync(application);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
        }

        /// <summary>
        /// Updates an existing application.
        /// </summary>
        /// <param name="id">The unique identifier of the application to update.</param>
        /// <param name="application">The application object containing updated information.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpPut("Edit/{application_id}")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> PutApplication([FromRoute] int id, [FromBody] Application application)
        {
            if (id != application.Id)
            {
                return BadRequest(Messages.ApplicationIdMismatch);
            }

            bool jobExists = await context.IsJobExistsAsync(application.JobID);
            if (!jobExists)
            {
                return BadRequest(Messages.JobIDNotExists);
            }
           
            bool jobSeekerExists = await context.IsJobSeekerExistsAsync(application.JobSeekerID);
            if (!jobSeekerExists)
            {
                return BadRequest(new { Message = Messages.JobSeekerIdNotExist });
            }

            context.SetModified(application);

            try
            {
                await context.SaveChangesAsync();
                return Ok(application);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.EntityExists<Application>(id))
                {
                    return NotFound(new { Message = Messages.ApplicationIdNotFound });
                }
                else
                {
                    return Conflict(new { Message = Messages.ConcurrencyConflict });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message =Messages.NotExpectedError });
            }
        }

        /// <summary>
        /// Deletes an existing application.
        /// </summary>
        /// <param name="id">The unique identifier of the application to delete.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpDelete("Delete/{application_id}")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> DeleteApplication([FromRoute] int id)
        {
            var application = await context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound(new { Message = Messages.ApplicationIdNotFound });
            }

            context.Applications.Remove(application);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
