using System.Text.Json.Serialization;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a Job Seeker.
    /// </summary>
    public class JobSeekerDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the job seeker.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the job seeker.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the first name of the job seeker.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the job seeker.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets the full name of the job seeker, combining first and last names.
        /// </summary>
        public string Name => $"{FirstName} {LastName}";

        /// <summary>
        /// Gets or sets the phone number of the job seeker.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the password for the job seeker's account.
        /// </summary>
        [JsonIgnore]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the address of the job seeker.
        /// </summary>
        public string? Address { get; set; }
    }
}
