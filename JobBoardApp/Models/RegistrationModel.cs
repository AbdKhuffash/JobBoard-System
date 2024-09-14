using System.ComponentModel.DataAnnotations;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents the model used for user registration requests.
    /// Data transfer Model.
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        /// <value>
        /// The unique identifier for the user. This field is required.
        /// </value>
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        /// <value>
        /// The first name of the user. This field is required.
        /// </value>
        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        /// <value>
        /// The last name of the user. This field is required.
        /// </value>
        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <value>
        /// The email address of the user. This field is required and must be a valid email address.
        /// </value>
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <value>
        /// The password of the user. This field is required.
        /// </value>
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        /// <value>
        /// The phone number of the user. This field is required.
        /// </value>
        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        /// <value>
        /// The address of the user. This field is required.
        /// </value>
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        /// <value>
        /// The role of the user. This field is required.
        /// </value>
        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }
    }
}
