using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for an Employer.
    /// </summary>
    public class EmployerDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the employer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the employer.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the first name of the employer.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the employer.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets the full name of the employer, combining first and last names.
        /// </summary>
        public string Name => $"{FirstName} {LastName}";

        /// <summary>
        /// Gets or sets the phone number of the employer.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the password for the employer's account.
        /// </summary>
        [JsonIgnore]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the address of the employer.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the name of the company the employer represents.
        /// </summary>
        public string? CompanyName { get; set; }
    }
}
