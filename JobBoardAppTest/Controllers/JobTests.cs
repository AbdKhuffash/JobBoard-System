using System;
using JobBoardAppTest.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobBoardApp.Controllers;
using JobBoardApp.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace ControllersTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="JobsController"/> class.
    /// </summary>
    [TestClass]
    public class JobTests
    {
        private readonly JobsController controller;
        private readonly Mock<DbSet<Job>> mockSet;
        private readonly Mock<JobBoardContext> mockContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobTests"/> class.
        /// Sets up mocks and the controller instance.
        /// </summary>
        public JobTests()
        {
            mockSet = new Mock<DbSet<Job>>();
            mockContext = new Mock<JobBoardContext>();
            mockContext.Setup(m => m.Jobs).Returns(mockSet.Object);

            controller = new JobsController(mockContext.Object, new ErrorMessages());
        }

        /// <summary>
        /// Tests the <see cref="JobsController.PostJob"/> method.
        /// Verifies that a newly created job is returned with a <see cref="CreatedAtActionResult"/> status.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task PostJob_WhenJobIsValid_ReturnsCreatedAtActionResultWithNewJob()
        {
            // arrange
            var job = new Job
            {
                Id = 1,
                Title = "Example Job",
                Description = "Example Example Example",
                Requirements = "Example Requirements",
                Location = "Example",
                Salary = 1234,
                Status = 0,
                EmployerID = 123,
                ApplicationDeadline = DateTime.Parse("2024-09-30T23:59:59")
            };
            mockSet.Setup(m => m.FindAsync(job.Id)).ReturnsAsync(null as Job);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // act 
            var result = await controller.PostJob(job);

            // assert
            mockSet.Verify(m => m.AddAsync(It.Is<Job>(e => e == job), default), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            result.Should().NotBeNull();

            var createdAtActionResult = result.Should().NotBeNull().And.BeOfType<ActionResult<IEnumerable<Job>>>().Which.Result.Should().BeOfType<CreatedAtActionResult>().Which;

            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be("GetJob");
            createdAtActionResult.Value.Should().Be(job);
            createdAtActionResult.StatusCode.Should().Be(201);
        }

        /// <summary>
        /// Tests the <see cref="JobsController.GetJob"/> method when a job is found.
        /// Verifies that the method returns an <see cref="OkObjectResult"/> with status 200.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetJob_WhenJobExists_ReturnsOkWithJob()
        {
            // arrange
            int id = 12;
            var job = new Job
            {
                Id = id
            };

            mockContext.Setup(c => c.Jobs.FindAsync(id)).ReturnsAsync(job);

            // act 
            var result = await controller.GetJob(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Job>>();

            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            actionResult.StatusCode.Should().Be(200);

            var returnedValue = actionResult.Value as Job;
            Assert.IsNotNull(returnedValue);
            returnedValue.Id.Should().Be(id);
        }

        /// <summary>
        /// Tests the <see cref="JobsController.GetJob"/> method when a job is not found.
        /// Verifies that the method returns a <see cref="NotFoundObjectResult"/> with status 404 and a specific error message.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetJob_WhenJobDoesNotExist_ReturnNotFound()
        {
            // arrange
            int id = 12;
            mockContext.Setup(c => c.Jobs.FindAsync(id)).ReturnsAsync(null as Job);

            // act 
            var result = await controller.GetJob(id);

            // assert
            result.Should().BeOfType<ActionResult<Job>>();

            var actionResult = result.Result as NotFoundObjectResult;

            Assert.IsNotNull(actionResult);
            actionResult.Value.Should().Be($"The Requested Job Posting With Id{id} does not Exist!");
            actionResult.StatusCode.Should().Be(404);
        }

        /// <summary>
        /// Tests the <see cref="JobsController.GetJobs"/> method.
        /// Verifies that the method returns a list of Job postings with an <see cref="OkObjectResult"/> status.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetJobs_WhenJobsExist_ReturnsOkWithJobsList()
        {
            // arrange
            var job = new List<Job>
            {
                new Job {
                Id = 26,
                Title = "Example Job",
                Description = "Example Example Example",
                Requirements = "Example Requirements",
                Location = "Example",
                Salary = 1234,
                Status = 0,
                EmployerID = 123,
                ApplicationDeadline = DateTime.Parse("2024-09-30T23:59:59")
                },
                new Job {
                Id = 27,
                Title = "Example Job",
                Description = "Example Example Example",
                Requirements = "Example Requirements",
                Location = "Example",
                Salary = 1234,
                Status = 0,
                EmployerID = 123,
                ApplicationDeadline = DateTime.Parse("2024-09-30T23:59:59")
                 }
            }.AsQueryable();

            var asyncEnumerator = new TestAsyncEnumerator<Job>(job.GetEnumerator());
            var mockSet = new Mock<DbSet<Job>>();
            var mockContext = new Mock<JobBoardContext>();
            var controller = new JobsController(mockContext.Object, new ErrorMessages());

            mockSet.As<IQueryable<Job>>().Setup(m => m.Provider).Returns(job.Provider);
            mockSet.As<IQueryable<Job>>().Setup(m => m.Expression).Returns(job.Expression);
            mockSet.As<IQueryable<Job>>().Setup(m => m.ElementType).Returns(job.ElementType);
            mockSet.As<IQueryable<Job>>().Setup(m => m.GetEnumerator()).Returns(job.GetEnumerator());
            mockSet.As<IAsyncEnumerable<Job>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(asyncEnumerator);
            mockContext.Setup(c => c.Jobs).Returns(mockSet.Object);

            // act
            var result = await controller.GetJobs();

            // assert
            result.Should().NotBeNull();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            actionResult.StatusCode.Should().Be(200);

            var returnedJobSeekers = actionResult.Value as IEnumerable<Job>;

            returnedJobSeekers.Should().NotBeNull();
            returnedJobSeekers.Should().HaveCount(2);
            returnedJobSeekers.Should().Contain(e => e.Id == 26);
            returnedJobSeekers.Should().Contain(e => e.Id == 27);
        }

        [TestMethod]
        public async Task DeleteJob_WhenJobExists_ReturnsNoContent()
        {
            // Arrange
            int id = 1;
            Job? job = new Job();
            mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(job);
            mockSet.Setup(m => m.Remove(job));
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(id);


            // Act
            var result = await controller.DeleteJob(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockSet.Verify(m => m.Remove(job), Times.Once);
        }

        [TestMethod]
        public async Task DeleteJob_WhenJobDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            int id = 10;
            Job? job = null;
            mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(job);

            // Act
            var result = await controller.DeleteJob(id);

            // Assert
            Assert.IsNotNull(result);
            var NotFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(NotFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, NotFoundResult.StatusCode);
            mockSet.Verify(m => m.Remove(It.IsAny<Job>()), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutJob_IDMismatch_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;
            var job = new Job { Id = 2 };

            // Act
            var result = await controller.PutJob(id, job);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Job ID mismatch.", badRequestResult.Value);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutJob_WhenUpdateIsSuccessful_ReturnsOk()
        {
            // Arrange
            var Job = new Job();
            int id = 1; Job.Id = id; Job.EmployerID = 3;
            mockContext.Setup(c => c.IsEmployerExistsAsync(Job.EmployerID)).ReturnsAsync(true);
            mockContext.Setup(c => c.SetModified(Job));
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(id);

            // Act
            var result = await controller.PutJob(id, Job) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(Job, result.Value);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
            mockContext.Verify(c => c.SetModified(Job), Times.Once);
        }

        [TestMethod]
        public async Task PutJob_WhenEmployerDoesNotExist_ReturnsBadRequest()
        {
            //Arrange
            var Job = new Job();
            int id = 1; Job.Id = id; Job.EmployerID = 99;
            mockContext.Setup(c => c.IsEmployerExistsAsync(Job.EmployerID)).ReturnsAsync(false);

            //Act
            var result = await controller.PutJob(id, Job);

            //Assert
            Assert.IsNotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            mockContext.Verify(c => c.SetModified(Job), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutJob_WhenJobDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var Job = new Job();
            int id = 1; Job.Id = id; Job.EmployerID = 3;
            mockContext.Setup(c => c.IsEmployerExistsAsync(Job.EmployerID)).ReturnsAsync(true);
            mockContext.Setup(m => m.SetModified(Job)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Job>(Job.Id)).Returns(false);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateConcurrencyException());

            //Act
            var result = await controller.PutJob(id, Job);

            //Assert
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task PutJob_WhenJobExists_ReturnsConflict()
        {
            //Arrange
            var Job = new Job();
            int id = 1; Job.Id = id; Job.EmployerID = 3;
            mockContext.Setup(c => c.IsEmployerExistsAsync(Job.EmployerID)).ReturnsAsync(true);
            mockContext.Setup(m => m.SetModified(Job)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Job>(Job.Id)).Returns(true);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateConcurrencyException());

            //Act
            var result = await controller.PutJob(id, Job);

            //Assert
            Assert.IsNotNull(result);
            var ConflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(ConflictResult);
            Assert.AreEqual(409, ConflictResult.StatusCode);
        }

        [TestMethod]
        public async Task PutJob_WhenUnexpectedErrorOccurred_ReturnsStatusCode()
        {
            //Arrange
            var job = new Job();
            int id = 1; job.Id = id; job.EmployerID = 3;
            mockContext.Setup(c => c.IsEmployerExistsAsync(job.EmployerID)).ReturnsAsync(true);
            mockContext.Setup(m => m.SetModified(job)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Job>(job.Id)).Returns(true);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new Exception());

            //Act
            var result = await controller.PutJob(id, job);

            //Assert
            Assert.IsNotNull(result);
            var StatusCodeResult = result as ObjectResult;
            Assert.IsNotNull(StatusCodeResult);
            Assert.AreEqual(500, StatusCodeResult.StatusCode);
        }
    }
}
