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
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace ControllersTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="JobSeekersController"/> class.
    /// </summary>
    [TestClass]
    public class JobSeekerTests
    {
        private readonly JobSeekersController controller;
        private readonly Mock<DbSet<JobSeeker>> mockSet;
        private readonly Mock<JobBoardContext> mockContext;
        private readonly Mock<IMapper> mockMapper;


        /// <summary>
        /// Initializes a new instance of the <see cref="JobSeekerTests"/> class.
        /// Sets up mocks and the controller instance.
        /// </summary>
        public JobSeekerTests()
        {
            mockSet = new Mock<DbSet<JobSeeker>>();
            mockContext = new Mock<JobBoardContext>();
            mockMapper = new Mock<IMapper>();

            mockContext.Setup(m => m.JobSeekers).Returns(mockSet.Object);

            controller = new JobSeekersController(mockContext.Object, mockMapper.Object, new ErrorMessages());
        }

        /// <summary>
        /// Tests the <see cref="JobSeekersController.PostJobSeeker"/> method.
        /// Verifies that a newly created job seeker is returned with a <see cref="CreatedAtActionResult"/> status.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task PostJobSeeker_WhenJobSeekerIsValid_ReturnsCreatedAtActionResultWithNewJobSeeker()
        {
            // arrange
            var jobseeker = new JobSeeker
            {
                Id = 27,
                Email = "employer@example.com",
                FirstName = "exam",
                LastName = "ple",
                PhoneNumber = "123-1234",
                Password = "examplePassword1",
                Address = "123 Example Street"
            };

            mockSet.Setup(m => m.FindAsync(jobseeker.Id)).ReturnsAsync(null as JobSeeker);
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(jobseeker.Id);

            // act 
            var result = await controller.PostJobSeeker(jobseeker);

            // assert
            mockSet.Verify(m => m.AddAsync(It.Is<JobSeeker>(e => e == jobseeker), default), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            result.Should().NotBeNull();

            var createdAtActionResult = result.Should().NotBeNull().And.BeOfType<ActionResult<IEnumerable<JobSeeker>>>().Which.Result.Should().BeOfType<CreatedAtActionResult>().Which;

            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be("GetJobSeeker");
            createdAtActionResult.Value.Should().Be(jobseeker);
            createdAtActionResult.StatusCode.Should().Be(201);
        }

        /// <summary>
        /// Tests the <see cref="JobSeekersController.GetJobSeeker"/> method when a job seeker is found.
        /// Verifies that the method returns an <see cref="OkObjectResult"/> with status 200.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetJobSeeker_WhenJobSeekerExists_ReturnsOkWithJobSeeker()
        {
            // arrange
            int id = 12;
            var jobseeker = new JobSeeker
            {
                Id = id
            };

            var jobSeekerDTO = new JobSeekerDTO
            {
                Id = id
            };

            mockContext.Setup(c => c.JobSeekers.FindAsync(id)).ReturnsAsync(jobseeker);
            mockMapper.Setup(m => m.Map<JobSeekerDTO>(It.IsAny<JobSeeker>())).Returns(jobSeekerDTO);

            // act 
            var result = await controller.GetJobSeeker(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<JobSeeker>>();

            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            actionResult.StatusCode.Should().Be(200);

            var returnedValue = actionResult.Value as JobSeekerDTO;
            Assert.IsNotNull(returnedValue);
            returnedValue.Id.Should().Be(id);
        }

        /// <summary>
        /// Tests the <see cref="JobSeekersController.GetJobSeeker"/> method when a job seeker is not found.
        /// Verifies that the method returns a <see cref="NotFoundObjectResult"/> with status 404 and a specific error message.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetJobSeeker_WhenJobSeekerDoesNotExist_ReturnNotFound()
        {
            // arrange
            int id = 12;
            mockContext.Setup(c => c.JobSeekers.FindAsync(id)).ReturnsAsync(null as JobSeeker);

            // act 
            var result = await controller.GetJobSeeker(id);

            // assert
            result.Should().BeOfType<ActionResult<JobSeeker>>();

            var actionResult = result.Result as NotFoundObjectResult;

            Assert.IsNotNull(actionResult);
            actionResult.Value.Should().Be($"The Requested Job Seeker With Id: {id} does not Exist!");
            actionResult.StatusCode.Should().Be(404);
        }

        /// <summary>
        /// Tests the <see cref="JobSeekersController.GetJobSeekers"/> method.
        /// Verifies that the method returns a list of jobseekers with an <see cref="OkObjectResult"/> status.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetJobSeekers_WhenJobSeekersExist_ReturnsOkWithJobSeekersList()
        {
            // Arrange
            var jobseekers = new List<JobSeeker>
            {
                new JobSeeker
                {
                    Id = 26,
                    Email = "jobseeker1@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "123-1234",
                    Password = "examplePassword1",
                    Address = "123 Example Street"
                },
                new JobSeeker
                {
                    Id = 27,
                    Email = "jobseeker2@example.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    PhoneNumber = "567-5678",
                    Password = "examplePassword2",
                    Address = "456 Example Avenue"
                }
            }.AsQueryable();

            var jobseekersDTO = new List<JobSeekerDTO>
            {
                new JobSeekerDTO
                {
                    Id = 26,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "jobseeker1@example.com"
                },
                new JobSeekerDTO
                {
                    Id = 27,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jobseeker2@example.com"
                }
            };

            var asyncEnumerator = new TestAsyncEnumerator<JobSeeker>(jobseekers.GetEnumerator());
            var mockSet = new Mock<DbSet<JobSeeker>>();
            var mockContext = new Mock<JobBoardContext>();
            var mockMapper = new Mock<IMapper>();
            var controller = new JobSeekersController(mockContext.Object, mockMapper.Object, new ErrorMessages());

            mockSet.As<IQueryable<JobSeeker>>().Setup(m => m.Provider).Returns(jobseekers.Provider);
            mockSet.As<IQueryable<JobSeeker>>().Setup(m => m.Expression).Returns(jobseekers.Expression);
            mockSet.As<IQueryable<JobSeeker>>().Setup(m => m.ElementType).Returns(jobseekers.ElementType);
            mockSet.As<IQueryable<JobSeeker>>().Setup(m => m.GetEnumerator()).Returns(jobseekers.GetEnumerator());

            mockSet.As<IAsyncEnumerable<JobSeeker>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(asyncEnumerator);
            mockContext.Setup(c => c.JobSeekers).Returns(mockSet.Object);

            mockMapper.Setup(m => m.Map<List<JobSeekerDTO>>(It.IsAny<List<JobSeeker>>())).Returns(jobseekersDTO);

            // Act
            var result = await controller.GetJobSeekers();

            // Assert
            result.Should().NotBeNull();

            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            actionResult.StatusCode.Should().Be(200);

            var returnedJobSeekers = actionResult.Value as IEnumerable<JobSeekerDTO>;
            Assert.IsNotNull(returnedJobSeekers);
            returnedJobSeekers.Should().HaveCount(2);
            returnedJobSeekers.Should().Contain(e => e.Id == 26);
            returnedJobSeekers.Should().Contain(e => e.Id == 27);
        }


        [TestMethod]
        public async Task DeleteJobSeeker_WhenJobSeekerExists_ReturnsNoContent()
        {
            //Arrange
            int id = 3;
            var jobSeeker = new JobSeeker();
            mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(jobSeeker);
            mockSet.Setup(m => m.Remove(jobSeeker));
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(id);

            // Act
            var result = await controller.DeleteJobSeeker(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.IsNotNull(result);
            mockSet.Verify(m => m.Remove(jobSeeker), Times.Once);
        }

        [TestMethod]
        public async Task DeleteJobSeeker_WhenJobSeekerDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            int id = 10;
            JobSeeker? jobSeeker = null;
            mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(jobSeeker);

            // Act
            var result = await controller.DeleteJobSeeker(id);

            // Assert
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            mockSet.Verify(m => m.Remove(It.IsAny<JobSeeker>()), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutJobSeeker_IDMismatch_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;
            var JobSeeker = new JobSeeker { Id = 2 };

            // Act
            var result = await controller.PutJobSeeker(id, JobSeeker);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("JobSeeker ID mismatch.", badRequestResult.Value);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }


        [TestMethod]
        public async Task PutJobSeeker_WhenUpdateIsSuccessful_ReturnsOk()
        {
            // Arrange
            var JobSeeker = new JobSeeker();
            int id = 1; JobSeeker.Id = id; JobSeeker.Address = "New Address";
            mockContext.Setup(c => c.SetModified(JobSeeker));
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await controller.PutJobSeeker(id, JobSeeker) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(JobSeeker, result.Value);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
            mockContext.Verify(c => c.SetModified(JobSeeker), Times.Once);
        }

        [TestMethod]
        public async Task PutJobSeeker_WhenJobSeekerDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var JobSeeker = new JobSeeker();
            int id = 1; JobSeeker.Id = id;
            mockContext.Setup(m => m.SetModified(JobSeeker)).Verifiable();
            mockContext.Setup(m => m.EntityExists<JobSeeker>(JobSeeker.Id)).Returns(false);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateConcurrencyException());

            //Act
            var result = await controller.PutJobSeeker(id, JobSeeker);

            //Assert
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task PutJobSeeker_WhenJobSeekerExists_ReturnsConflict()
        {
            //Arrange
            var jobSeeker = new JobSeeker();
            int id = 1; jobSeeker.Id = id;
            mockContext.Setup(m => m.SetModified(jobSeeker)).Verifiable();
            mockContext.Setup(m => m.EntityExists<JobSeeker>(jobSeeker.Id)).Returns(true);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateConcurrencyException());

            //Act
            var result = await controller.PutJobSeeker(id, jobSeeker);

            //Assert
            Assert.IsNotNull(result);
            var ConflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(ConflictResult);
            Assert.AreEqual(409, ConflictResult.StatusCode);
        }

        [TestMethod]
        public async Task PutJobSeeker_WhenUnexpectedErrorOccurred_ReturnsStatusCode()
        {
            //Arrange
            var jobSeeker = new JobSeeker();
            int id = 1; jobSeeker.Id = id;
            mockContext.Setup(m => m.SetModified(jobSeeker)).Verifiable();
            mockContext.Setup(m => m.EntityExists<JobSeeker>(jobSeeker.Id)).Returns(true);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new Exception());

            //Act
            var result = await controller.PutJobSeeker(id, jobSeeker);

            //Assert
            Assert.IsNotNull(result);
            var StatusCodeResult = result as ObjectResult;
            Assert.IsNotNull(StatusCodeResult);
            Assert.AreEqual(500, StatusCodeResult.StatusCode);
        }
    }
}
