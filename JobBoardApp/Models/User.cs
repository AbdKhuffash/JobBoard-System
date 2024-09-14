using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents a user in the job board application.
    /// This is an abstract class to be inherited by specific user types like JobSeeker and Employer.
    /// Inherits IdentityUser with Id to be Int.
    /// </summary>
    public abstract class User : IdentityUser<int>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required]
        [EmailAddress]
        public override string? Email { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [Required]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [Required]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets the full name of the user, derived from first and last name.
        /// </summary>
        public string Name => $"{FirstName} {LastName}";

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        [Required]
        public override string? PhoneNumber { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Gets the password of the user.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        [Required]
        public string? Address { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with specified details.
        /// </summary>
        /// <param name="Id">The unique identifier for the user.</param>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="FirstName">The first name of the user.</param>
        /// <param name="LastName">The last name of the user.</param>
        /// <param name="PhoneNumber">The phone number of the user.</param>
        /// <param name="Password">The password of the user.</param>
        /// <param name="Address">The address of the user.</param>
        protected User(int Id, string Email, string FirstName, string LastName, string PhoneNumber, string Password, string Address)
        {
            this.Id = Id;
            this.Email = Email;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PhoneNumber = PhoneNumber;
            this.Address = Address;
            SetPassword(Password);
        }

        /// <summary>
        /// Default Constructor for User Class.
        /// </summary>
        protected User()
        {

        }

        /// <summary>
        /// Sets the password for the user with necessary validation.
        /// </summary>
        /// <param name="password">The password to set.</param>
        private void SetPassword(string password)
        {
            ErrorMessages Messages = new ErrorMessages();
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(Messages.PasswordEmpty);
            }

            if (password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long");
            }

            if (!password.Any(char.IsUpper))
            {
                throw new ArgumentException("Password must contain at least one uppercase letter");
            }

            if (!password.Any(char.IsLower))
            {
                throw new ArgumentException("Password must contain at least one lowercase letter");
            }

            if (!password.Any(char.IsDigit))
            {
                throw new ArgumentException("Password must contain at least one digit");
            }

            Password = password;
        }
    }
}
