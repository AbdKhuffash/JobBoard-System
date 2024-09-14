using Microsoft.AspNetCore.Identity;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents the roles that users can have in the application.
    /// Inherits IdentityRole
    /// </summary>
    public class UserRoles : IdentityRole<int>
    {
        /// <summary>
        /// The role name for an employer.
        /// </summary>
        public const string Employer = "Employer";

        /// <summary>
        /// The role name for a job seeker.
        /// </summary>
        public const string JobSeeker = "JobSeeker";

        /// <summary>
        /// The role name for an admin.
        /// </summary>
        public const string Admin = "Admin";
    }
}
