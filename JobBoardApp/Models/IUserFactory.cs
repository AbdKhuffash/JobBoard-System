namespace JobBoardApp.Models
{
    /// <summary>
    /// Defines a factory interface for creating user instances.
    /// </summary>
    public interface IUserFactory
    {
        /// <summary>
        /// Creates a user instance based on the provided registration model.
        /// </summary>
        /// <param name="model">The registration model containing user information.</param>
        /// <returns>A <see cref="User"/> object populated with the data from the registration model.</returns>
        User CreateUser(RegistrationModel model);
    }
}
