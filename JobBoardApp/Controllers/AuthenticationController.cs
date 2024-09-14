using JobBoardApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoardApp.Controllers
{
    /// <summary>
    /// Handles user authentication, including registration and login functionality.Generates JWT tokens.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<UserRoles> roleManager;
        private readonly IConfiguration configuration;
        private readonly ErrorMessages Messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager to handle user operations.</param>
        /// <param name="roleManager">The role manager to handle role operations.</param>
        /// <param name="configuration">The configuration to access application settings.</param>
        /// <param name="Messages",>The error messages to get error messages</param>
        public AuthenticationController(UserManager<User> userManager, RoleManager<UserRoles> roleManager, IConfiguration configuration, ErrorMessages Messages)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.Messages =Messages ?? throw new ArgumentNullException(nameof(Messages));    
        }

        /// <summary>
        /// Registers a new user with the specified details.
        /// </summary>
        /// <param name="model">The registration model containing user details.</param>
        /// <returns>A result indicating the success or failure of the registration.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (string.IsNullOrEmpty(model.Password))
                return BadRequest(Messages.PasswordRequired);
            if (string.IsNullOrEmpty(model.Role))
                return BadRequest(Messages.RoleRequired);

            IUserFactory userFactory = new UserFactory(); ;
           
            var user = userFactory.CreateUser(model);
            var result = await userManager.CreateAsync(user, model.Password);
           
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(model.Role))
                    await roleManager.CreateAsync(new UserRoles { Name = model.Role });
                
                await userManager.AddToRoleAsync(user, model.Role);

                return Ok(new { Status = "Success", Message = "User registered successfully!" });
            }

            return BadRequest(result.Errors);
        }


        /// <summary>
        /// Authenticates a user and returns a JWT token if the login is successful.
        /// </summary>
        /// <param name="model">The login model containing user credentials.</param>
        /// <returns>A result containing the authentication status and token if successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest(Messages.EmnailRequired);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(Messages.InvalidUserName);

            if (string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(Messages.PasswordEmpty);
            }
            if (!await userManager.CheckPasswordAsync(user, model.Password))
                return BadRequest(Messages.PasswordIvalid);

            var userRoles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>() 
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, userRole));

            var tokenG = new TokenGeneration(configuration);

            string token = tokenG.GenerateToken(claims);

            return Ok(new { Status = "Success", Message = $"Successful login with token: {token}" });
        }
    }
}
