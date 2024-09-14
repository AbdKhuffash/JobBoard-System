using System;
using System.Collections.Generic;
using JobBoardApp.Models;
using System.ComponentModel.DataAnnotations;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents a job seeker in the job board application.
    /// Inherits from the User class.
    /// </summary>
    public class JobSeeker : User
    {
        /// <summary>
        /// Gets or sets the list of applications submitted by the job seeker.
        /// </summary>
        public List<Application> Applications { get; set; }= new List<Application>();

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSeeker"/> class with specified details.
        /// </summary>
        /// <param name="Id">The unique identifier for the user.</param>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="FirstName">The first name of the user.</param>
        /// <param name="LastName">The last name of the user.</param>
        /// <param name="PhoneNumber">The phone number of the user.</param>
        /// <param name="Password">The password of the user.</param>
        /// <param name="Address">The address of the user.</param>
        public JobSeeker(int Id, string Email, string FirstName, string LastName, string PhoneNumber, string Password, string Address)
            : base(Id, Email, FirstName, LastName, PhoneNumber, Password, Address)
        {

        }

        /// <summary>
        /// Default constructor for JobSeeker Class.
        /// </summary>
        public JobSeeker()
            : base()
        {

        }

        /// <summary>
        /// Applies for the specified job posting if the job is open and the application deadline has not passed.
        /// </summary>
        /// <param name="jobPosting">The job posting to apply for.</param>
        /// <param name="application">The application details.</param>
        /// <exception cref="InvalidOperationException">Thrown if the job is closed or the application deadline has passed.</exception>
        public void ApplyForJob(Job jobPosting, Application application)
        {
            ErrorMessages Messages = new ErrorMessages();
            if (jobPosting.Status == JobStatus.Active)
            {
                throw new InvalidOperationException(Messages.JobNotActive);
            }

            if (jobPosting.Status == JobStatus.Filled)
            {
                throw new InvalidOperationException(Messages.JobFilled);
            }

            if (jobPosting.IsApplicationDeadlinePassed())
            {
                throw new InvalidOperationException(Messages.JobDeadLinePassed);
            }

            Applications.Add(application);
            jobPosting.Applications.Add(application);
        }
    }
}
