using JobBoardApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using JobBoardApp.Middleware;
using Scalar.AspNetCore; // 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IUserFactory, UserFactory>();
builder.Services.AddSingleton<ErrorMessages>();
builder.Services.AddEndpointsApiExplorer();

// Keep SwaggerGen (to generate OpenAPI JSON)
builder.Services.AddSwaggerGen(options =>
{
    // Optional: JWT Bearer security for Authorize button in Scalar
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// DbContext
builder.Services.AddDbContext<JobBoardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JobBoardConnection")));

// Identity
builder.Services.AddIdentity<User, UserRoles>()
                .AddEntityFrameworkStores<JobBoardContext>()
                .AddDefaultTokenProviders();

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(UserRoles.Admin));
    options.AddPolicy("EmployerOnly", policy => policy.RequireRole(UserRoles.Employer));
    options.AddPolicy("JobSeekerOnly", policy => policy.RequireRole(UserRoles.JobSeeker));
    options.AddPolicy("JobPolicy", policy => policy.RequireRole(UserRoles.JobSeeker, UserRoles.Employer));
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
        ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Middlewares
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoginMiddleware>();

// Expose OpenAPI JSON
app.MapSwagger("/openapi/{documentName}.json");

// Scalar UI
app.MapScalarApiReference(options =>
{
    options.WithTitle("JobBoard API")
           .WithDarkMode(true)
           .WithOpenApiRoutePattern("/openapi/{documentName}.json");
    // Uncomment if you want auth persistence in dev:
    // .WithPersistentAuthentication();
});

// MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
