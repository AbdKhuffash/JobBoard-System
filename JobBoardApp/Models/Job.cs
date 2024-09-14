using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobBoardApp.Models;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents a job posting.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the unique identifier for the job.
        /// </summary>
        [Key]
        public int? Id { get; set; }

        /// <dotsummary>
        /// Gets or sets the title of the job.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the job.
        /// </summary>
        [Required]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the requirements for the job.
        /// </summary>
        [Required]
        public string? Requirements { get; set; }

        /// <summary>
        /// Gets or sets the location of the job.
        /// </summary>
        [Required]
        public string? Location { get; set; }

        /// <summary>
        /// Gets or sets the salary for the job.
        /// </summary>
        [Required]
        public double? Salary { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the employer.
        /// </summary>
        [ForeignKey("Employer")]
        public int? EmployerID { get; set; }

        /// <summary>
        /// Navigation property to the Employer related to the Job.
        /// </summary>
        public virtual Employer? Employer { get; set; }

        /// <summary>
        /// Gets or sets the application deadline for the job.
        /// </summary>
        [Required]
        public DateTime? ApplicationDeadline { get; set; }

        /// <summary>
        /// Gets or sets the status of the job.
        /// </summary>
        [Required]
        public JobStatus? Status { get; set; }

        /// <summary>
        /// Navigation property to related applications.
        /// </summary>
        public ICollection<Application> Applications { get; set; } = new List<Application>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class with specified details.
        /// </summary>
        /// <param name="id">The unique identifier for the job.</param>
        /// <param name="title">The title of the job.</param>
        /// <param name="description">The description of the job.</param>
        /// <param name="requirements">The requirements for the job.</param>
        /// <param name="location">The location of the job.</param>
        /// <param name="salary">The salary for the job.</param>
        /// <param name="status">The status of the job.</param>
        /// <param name="employerID">The foreign key to the employer.</param>
        /// <param name="applicationDeadline">The application deadline for the job.</param>
        public Job(int id, string title, string description, string requirements, string location, double salary, JobStatus status, int employerID, DateTime applicationDeadline)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Requirements = requirements;
            this.Location = location;
            this.Salary = salary;
            this.EmployerID = employerID;
            this.Status = status;
            this.ApplicationDeadline = applicationDeadline;
        }

        /// <summary>
        /// Default constructor for Job Class
        /// </summary>
        public Job()
        {

        }
        /// <summary>
        /// Checks if the application deadline has passed.
        /// </summary>
        /// <returns>True if the current date and time is past the application deadline; otherwise, false.</returns>
        public bool IsApplicationDeadlinePassed()
        {
            return DateTime.Now > ApplicationDeadline;
        }
    }
}
