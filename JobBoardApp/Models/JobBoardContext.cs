using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents the database context for the job board application.
    /// </summary>
    public class JobBoardContext : IdentityDbContext<User, UserRoles, int>
    {
        public JobBoardContext() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="JobBoardContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the <see cref="DbContext"/>.</param>
        public JobBoardContext(DbContextOptions<JobBoardContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for <see cref="Application"/> entities.
        /// </summary>
        public virtual DbSet<Application> Applications { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for <see cref="Job"/> entities.
        /// </summary>
        public virtual DbSet<Job> Jobs { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for <see cref="User"/> entities.
        /// </summary>
        
        public  override DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for <see cref="JobSeeker"/> entities.
        /// </summary>
        public virtual DbSet<JobSeeker> JobSeekers { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for <see cref="Employer"/> entities.
        /// </summary>
        public virtual DbSet<Employer> Employers { get; set; }

        /// <summary>
        /// Configures the model and its relationships.
        /// </summary>
        /// <param name="modelBuilder">The builder used to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Table-per-Hierarchy (TPH) inheritance for User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                entity.Ignore(e => e.EmailConfirmed);
                entity.Ignore(e => e.SecurityStamp);
                entity.Ignore(e => e.ConcurrencyStamp);
                entity.Ignore(e => e.PhoneNumberConfirmed);
                entity.Ignore(e => e.TwoFactorEnabled);
                entity.Ignore(e => e.LockoutEnabled);
                entity.Ignore(e => e.LockoutEnd);
                entity.Ignore(e => e.AccessFailedCount);
            });

            // Configure Job entity
            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .IsRequired();

                entity.Property(e => e.Requirements)
                    .IsRequired();

                entity.Property(e => e.Location)
                    .IsRequired();

                entity.Property(e => e.Salary)
                    .IsRequired();

                entity.Property(e => e.ApplicationDeadline)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired();

                entity.HasOne(d => d.Employer)
                   .WithMany(e => e.JobPostings)
                   .HasForeignKey(d => d.EmployerID)
                   .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.Applications)
                    .WithOne(p => p.Job)
                    .HasForeignKey(p => p.JobID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Application entity
            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Date)
                    .IsRequired();

                entity.Property(e => e.ApplicationCVPath)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired();

                entity.Property(e => e.CoverLetter)
                    .HasMaxLength(500);

                entity.HasOne(d => d.Job)
                    .WithMany(j => j.Applications)
                    .HasForeignKey(d => d.JobID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.JobSeeker)
                    .WithMany(js => js.Applications)
                    .HasForeignKey(d => d.JobSeekerID)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            // Configure JobSeeker entity
            modelBuilder.Entity<JobSeeker>(entity =>
            {
                entity.ToTable("JobSeeker");
                entity.HasBaseType<User>();
                entity.HasMany(d => d.Applications)
                    .WithOne(p => p.JobSeeker)
                    .HasForeignKey(p => p.JobSeekerID)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Ignore(e => e.EmailConfirmed);
                entity.Ignore(e => e.SecurityStamp);
                entity.Ignore(e => e.ConcurrencyStamp);
                entity.Ignore(e => e.PhoneNumberConfirmed);
                entity.Ignore(e => e.TwoFactorEnabled);
                entity.Ignore(e => e.LockoutEnabled);
                entity.Ignore(e => e.LockoutEnd);
                entity.Ignore(e => e.AccessFailedCount);

            });

            // Configure Employer entity
            modelBuilder.Entity<Employer>(entity =>
            {
                entity.ToTable("Employer");
                entity.HasBaseType<User>();
                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(d => d.JobPostings)
                    .WithOne(p => p.Employer)
                    .HasForeignKey(p => p.EmployerID)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Ignore(e => e.EmailConfirmed);
                entity.Ignore(e => e.SecurityStamp);
                entity.Ignore(e => e.ConcurrencyStamp);
                entity.Ignore(e => e.PhoneNumberConfirmed);
                entity.Ignore(e => e.TwoFactorEnabled);
                entity.Ignore(e => e.LockoutEnabled);
                entity.Ignore(e => e.LockoutEnd);
                entity.Ignore(e => e.AccessFailedCount);
            });
        }
        public virtual void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public virtual bool EntityExists<TEntity>(int? id) where TEntity : class
        {
            return Set<TEntity>().Find(id) != null;
        }

        public virtual async Task<bool> EntityExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await Set<TEntity>().AnyAsync(predicate);
        }


        public virtual async Task<bool> IsJobSeekerExistsAsync(int? jobSeekerId)
        {
            return await EntityExistsAsync<JobSeeker>(js => js.Id == jobSeekerId);
        }

        public virtual async Task<bool> IsEmployerExistsAsync(int? employerId)
        {
            return await EntityExistsAsync<Employer>(e => e.Id == employerId);
        }

        public virtual async Task<bool> IsJobExistsAsync(int? jobId)
        {
            return await EntityExistsAsync<Job>(js => js.Id == jobId);
        }
    }
}