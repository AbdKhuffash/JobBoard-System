using System.ComponentModel.DataAnnotations;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents the model used for user login requests.
    /// Data transfer Model.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <value>
        /// The email address of the user. This field is required.
        /// </value>
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
    }
}
