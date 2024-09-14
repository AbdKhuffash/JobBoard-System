using System;
using System.Collections.Generic;
using JobBoardApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents an employer in the job board application.
    /// Inherits from the <see cref="User"/> class.
    /// </summary>
    public class Employer : User
    {
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        [Required]
        public string? CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the list of job postings created by the employer.
        /// </summary>
        public ICollection<Job> JobPostings { get; set; } = new List<Job>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Employer"/> class with specified details.
        /// </summary>
        /// <param name="Id">The unique identifier for the user.</param>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="FirstName">The first name of the user.</param>
        /// <param name="LastName">The last name of the user.</param>
        /// <param name="PhoneNumber">The phone number of the user.</param>
        /// <param name="Password">The password of the user.</param>
        /// <param name="Address">The address of the user.</param>
        /// <param name="CompanyName">The name of the company.</param>
        public Employer(int Id, string Email, string FirstName, string LastName, string PhoneNumber, string Password, string Address, string CompanyName)
            : base(Id, Email, FirstName, LastName, PhoneNumber, Password, Address)
        {
            this.CompanyName = CompanyName;
        }
        /// <summary>
        /// Default Constructor for Employer Class.
        /// </summary>
        public Employer()
            :base()
        {
            
        }

        /// <summary>
        /// Adds a new job posting to the employer's list of job postings.
        /// </summary>
        /// <param name="posting">The job posting to add.</param>
        public void CreateJobPosting(Job posting)
        {
            JobPostings.Add(posting);
        }
    }
}
