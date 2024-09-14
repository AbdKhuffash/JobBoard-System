using JobBoardApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;


namespace JobBoardApp.Controllers
{
    /// <summary>
    /// Controller for handling job Employer operations.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class EmployersController : ControllerBase
    {
        private readonly JobBoardContext context;
        private readonly IMapper mapper;
        private readonly ErrorMessages Messages;
        public EmployersController(JobBoardContext context, IMapper mapper, ErrorMessages Messages)
        {
            this.mapper = mapper;
            this.context = context;
            this.Messages = Messages;
        }
        /// <summary>
        /// Gets the list of all employers.
        /// </summary>
        /// <returns>A list of employers.</returns>
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<Employer>>> GetEmployers()
        {
            var employer = await context.Employers.ToListAsync();

            var employerdto = mapper.Map<List<EmployerDTO>>(employer);
            return Ok(employerdto);
        }

        /// <summary>
        /// Gets an employer by their ID.
        /// </summary>
        /// <param name="id">The ID of the employer.</param>
        /// <returns>The employer with the specified ID, if found.</returns>
        [HttpGet("Get-By-Id/{employer_id}")]
        public async Task<ActionResult<Employer>> GetEmployer(int id)
        {
            var employer = await context.Employers.FindAsync(id);

            if (employer == null)
            {
                return NotFound($"The Requested Employer With Id: {id} does not Exist!");
            }
            var employerdto= mapper.Map<EmployerDTO>(employer);
            return Ok(employerdto);
        }

        /// <summary>
        /// Adds a new employer, if there is an employer with the same Id, an exception will be thrown.
        /// </summary>
        /// <param name="employer">The employer to add.</param>
        /// <returns>The created employer.</returns>
        [HttpPost("Create")]
        public async Task<ActionResult<IEnumerable<Employer>>> PostEmployer([FromBody] Employer employer)
        {
            var existingEmployer = await context.Employers.FindAsync(employer.Id);
            if (existingEmployer != null)
            {
                throw new InvalidOperationException(Messages.EmployerWithSameId);
            }
            await context.Employers.AddAsync(employer);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployer), new { id = employer.Id }, employer);
        }

        /// <summary>
        /// Updates an existing employer.
        /// </summary>
        /// <param name="id">The unique identifier of the employer to update.</param>
        /// <param name="employer">The employer object containing updated information.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpPut("Edit/{employer_id}")]
        public async Task<IActionResult> PutEmployer([FromRoute] int id, [FromBody] Employer employer)
        {
            if (id != employer.Id)
            {
                return BadRequest(Messages.EmployerIDMismatch);
            }

            context.SetModified(employer);

            try
            {
                await context.SaveChangesAsync();
                return Ok(employer); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.EntityExists<Employer>(id))
                {
                    return NotFound(new { Message = Messages.EmplyerIdNotFound });
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
        /// Deletes an existing employer.
        /// </summary>
        /// <param name="id">The unique identifier of the employer to delete.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpDelete("Delete/{employer_id}")]
        public async Task<IActionResult> DeleteEmployer([FromRoute] int id)
        {
            var employer = await context.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound(new { Message = Messages.EmplyerIdNotFound });
            }

            context.Employers.Remove(employer);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
