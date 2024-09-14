using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobBoardApp.Models;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents an application submitted for a job.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Gets or sets the unique identifier for the application.
        /// </summary>
        [Key]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the applicant.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the applicant.
        /// </summary>
        [Required]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email address of the applicant.
        /// </summary>
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the job the application is for.
        /// </summary>
        [ForeignKey("Job")]
        public int? JobID { get; set; } 

        /// <summary>
        /// Navigation property to the job related to the application.
        /// </summary>
        public virtual Job? Job { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the job seeker who submitted the application.
        /// </summary>
        [ForeignKey("JobSeeker")]
        public int? JobSeekerID { get; set; } 

        /// <summary>
        /// Navigation property to the jobSeeker related to the application.
        /// </summary>
        public virtual JobSeeker? JobSeeker { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the application was submitted.
        /// </summary>
        //[Required]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the path to the applicant's CV.
        /// </summary>
        [Required]
        public string? ApplicationCVPath { get; set; }

        /// <summary>
        /// Gets or sets the status of the application.
        /// </summary>
        [Required]
        public ApplicationStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the cover letter submitted with the application.
        /// </summary>
        [StringLength(500)]
        public string? CoverLetter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class with specified details.
        /// </summary>
        /// <param name="id">The unique identifier for the application.</param>
        /// <param name="name">The name of the applicant.</param>
        /// <param name="phoneNumber">The phone number of the applicant.</param>
        /// <param name="email">The email address of the applicant.</param>
        /// <param name="jobID">The foreign key to the job the application is for.</param>
        /// <param name="jobSeekerID">The foreign key to the job seeker who submitted the application.</param>
        /// <param name="applicationCVPath">The path to the applicant's CV.</param>
        /// <param name="status">The status of the application.</param>
        /// <param name="coverLetter">The cover letter submitted with the application.</param>
        public Application(int id, string name, string phoneNumber, string email, int jobID, int jobSeekerID, string applicationCVPath, ApplicationStatus status, string coverLetter)
        {
            this.Id = id;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.JobID = jobID;
            this.JobSeekerID = jobSeekerID;
            this.Date = DateTime.Now;
            this.ApplicationCVPath = applicationCVPath;
            this.Status = status;
            this.CoverLetter = coverLetter;
        }
        /// <summary>
        /// Default Constructor for Application class.
        /// </summary>
        public Application()
        {
            
        }
        /// <summary>
        /// Gets the details of the application.
        /// </summary>
        /// <returns>The current instance of the <see cref="Application"/> class.</returns>
        public Application GetApplicationDetails()
        {
            return this;
        }
    }
}
