using System;
using JobBoardApp.Models;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Represents an application status for a job.
    /// </summary>
    public enum ApplicationStatus
    {
        Pending = 0,
        Reviewed = 1,
        Accepted = 2,
        Rejected = 3
    }
}