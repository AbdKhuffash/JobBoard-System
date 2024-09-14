namespace JobBoardApp.Models
{
    /// <summary>
    /// Factory class responsible for creating instances of <see cref="User"/> based on the provided <see cref="RegistrationModel"/>.
    /// </summary>
    public class UserFactory : IUserFactory
    {
        /// <summary>
        /// Contains error messages related to user registration and role validation.
        /// </summary>
        private readonly ErrorMessages Messages = new ErrorMessages() ?? throw new ArgumentNullException(nameof(Messages));

        /// <summary>
        /// Creates a <see cref="User"/> object based on the specified <see cref="RegistrationModel"/>.
        /// </summary>
        /// <param name="model">The registration model containing user details.</param>
        /// <returns>A <see cref="User"/> object that represents either an <see cref="Employer"/> or a <see cref="JobSeeker"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the role in the registration model is invalid.</exception>
        public User CreateUser(RegistrationModel model)
        {
            User user;

            if (model.Role == UserRoles.Employer || model.Role == UserRoles.Admin)
            {
                user = new Employer
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserName = model.Email,
                    CompanyName = ""
                };
            }
            else if (model.Role == UserRoles.JobSeeker)
            {
                user = new JobSeeker
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserName = model.Email
                };
            }
            else
            {
                throw new ArgumentException(Messages.RoleInvalid, nameof(model.Role));
            }

            return user;
        }
    }
}