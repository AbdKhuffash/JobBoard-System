using AutoMapper;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents the AutoMapper profile configuration for mapping between domain models and DTOs.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// Configures the mappings between the domain models and their corresponding DTOs.
        /// </summary>
        public MappingProfile()
        {
            /// <summary>
            /// Maps an <see cref="Employer"/> to an <see cref="EmployerDTO"/>.
            /// </summary>
            CreateMap<Employer, EmployerDTO>();

            /// <summary>
            /// Maps a <see cref="JobSeeker"/> to a <see cref="JobSeekerDTO"/>.
            /// </summary>
            CreateMap<JobSeeker, JobSeekerDTO>();
        }
    }
}
