using System.Drawing;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Contains predefined error messages used throughout the JobBoard application.
    /// </summary>
    public class ErrorMessages
    {
        /// <summary>
        /// Error message when a job is not active.
        /// </summary>
        public string JobNotActive = "Cannot apply for this job as it is not Active!";

        /// <summary>
        /// Error message when a job has already been filled.
        /// </summary>
        public string JobFilled = "Cannot apply for this job as it is Filled!";

        /// <summary>
        /// Error message when the application deadline for a job has passed.
        /// </summary>
        public string JobDeadLinePassed = "Cannot apply for this job as the application deadline has passed.";

        /// <summary>
        /// Error message when the JWT secret is null or empty.
        /// </summary>
        public string JWTSecretNull = "JWT Secret cannot be null or empty.";

        /// <summary>
        /// Error message when the JWT issuer is null or empty.
        /// </summary>
        public string JWTIssuerEmpty = "Valid Issuer cannot be null or empty.";

        /// <summary>
        /// Error message when the JWT audience is null or empty.
        /// </summary>
        public string JWTAudienceEmpty = "Valid Audience cannot be null or empty.";

        /// <summary>
        /// Error message when the JWT token expiry time is null or empty.
        /// </summary>
        public string JWTTokenExpireEmpty = "Token Expiry Time In Hour cannot be null or empty.";

        /// <summary>
        /// Error message when the JWT token expiry time is not a valid number.
        /// </summary>
        public string JWTTokenExpireDoubleType = "Token Expiry Time In Hour is not a valid number.";

        /// <summary>
        /// Error message when the password is null or empty.
        /// </summary>
        public string PasswordEmpty = "Password cannot be null or empty.";

        /// <summary>
        /// Error message when an application with the same ID already exists.
        /// </summary>
        public string ApplicationWithSameId = "An Application with the same ID already exists.";

        /// <summary>
        /// Error message when there is an application ID mismatch.
        /// </summary>
        public string ApplicationIdMismatch = "Application ID mismatch.";

        /// <summary>
        /// Error message when the job ID does not exist.
        /// </summary>
        public string JobIDNotExists = "Job ID does not exist.";

        /// <summary>
        /// Error message when the job seeker ID does not exist.
        /// </summary>
        public string JobSeekerIdNotExist = "Job Seeker ID does not exist.";

        /// <summary>
        /// Error message when the specified application ID could not be found.
        /// </summary>
        public string ApplicationIdNotFound = "The application with the specified ID could not be found.";

        /// <summary>
        /// Error message when a concurrency conflict occurs.
        /// </summary>
        public string ConcurrencyConflict = "A concurrency conflict occurred.";

        /// <summary>
        /// Generic error message for unexpected errors.
        /// </summary>
        public string NotExpectedError = "An unexpected error occurred. Please try again later.";

        /// <summary>
        /// Error message when the password is required but not provided.
        /// </summary>
        public string PasswordRequired = "Password is required.";

        /// <summary>
        /// Error message when the role is required but not provided.
        /// </summary>
        public string RoleRequired = "Role is required.";

        /// <summary>
        /// Error message when an invalid role is provided.
        /// </summary>
        public string RoleInvalid = "Invalid ROLE!";

        /// <summary>
        /// Error message when the email is required but not provided.
        /// </summary>
        public string EmnailRequired = "Email is required.";

        /// <summary>
        /// Error message when an invalid username is provided.
        /// </summary>
        public string InvalidUserName = "Invalid Username.";

        /// <summary>
        /// Error message when an invalid password is provided.
        /// </summary>
        public string PasswordIvalid = "Invalid Password.";

        /// <summary>
        /// Error message when an employer with the same ID already exists.
        /// </summary>
        public string EmployerWithSameId = "An employer with the same ID already exists.";

        /// <summary>
        /// Error message when there is an employer ID mismatch.
        /// </summary>
        public string EmployerIDMismatch = "Employer ID mismatch.";

        /// <summary>
        /// Error message when the specified employer ID does not exist.
        /// </summary>
        public string EmplyerIdNotFound = "Employer with the specified ID does not exist.";

        /// <summary>
        /// Error message when a job posting with the same ID already exists.
        /// </summary>
        public string JobIdWithSameId = "A Job Posting with the same ID already exists.";

        /// <summary>
        /// Error message when there is a job ID mismatch.
        /// </summary>
        public string JobIdMismatch = "Job ID mismatch.";

        /// <summary>
        /// Error message when a job seeker with the same ID already exists.
        /// </summary>
        public string JobSeekerIDExists = "A JobSeeker with the same ID already exists.";

        /// <summary>
        /// Error message when there is a job seeker ID mismatch.
        /// </summary>
        public string JboSeekerIdMismatch = "JobSeeker ID mismatch.";
    }
}
