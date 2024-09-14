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

namespace ControllersTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="ApplicationsController"/> class.
    /// </summary>
    [TestClass]
    public class ApplicationTests
    {
        private readonly ApplicationsController controller;
        private readonly Mock<DbSet<Application>> mockSet;
        private readonly Mock<JobBoardContext> mockContext;
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTests"/> class.
        /// Sets up mocks and the controller instance.
        /// </summary>
        public ApplicationTests()
        {
            mockSet = new Mock<DbSet<Application>>();
            mockContext = new Mock<JobBoardContext>();
            mockContext.Setup(m => m.Applications).Returns(mockSet.Object);
 
            controller = new ApplicationsController(mockContext.Object, new ErrorMessages());
        }

        /// <summary>
        /// Tests the <see cref="ApplicationsController.PostApplication"/> method.
        /// Verifies that a newly created application is returned with a <see cref="CreatedAtActionResult"/> status.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task PostApplication_WhenApplicationIsValid_ReturnsCreatedAtActionResultWithNewApplication()
        {
            // arrange
            var application = new Application
            {
                Id = 5,
                Name = "Example",
                PhoneNumber = "123-1234",
                Email = "example@example.com",
                JobID = 123,
                JobSeekerID = 123,
                ApplicationCVPath = "/exa/mple/examplecv.pdf",
                Status = 0,
                CoverLetter = "Example Example Example",
                Date = DateTime.Parse("2024-08-14T12:34:56Z")
            };
            mockSet.Setup(m => m.FindAsync(application.Id)).ReturnsAsync(null as Application);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // act
            var result = await controller.PostApplication(application);

            // assert
            mockSet.Verify(m => m.AddAsync(It.Is<Application>(e => e == application), default), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            result.Should().NotBeNull();

            var createdAtActionResult = result.Should().NotBeNull().And.BeOfType<ActionResult<IEnumerable<Application>>>().Which.Result.Should().BeOfType<CreatedAtActionResult>().Which;

            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be("GetApplication");
            createdAtActionResult.Value.Should().Be(application);
            createdAtActionResult.StatusCode.Should().Be(201);
        }

        /// <summary>
        /// Tests the <see cref="ApplicationsController.GetApplication"/> method when an application is found.
        /// Verifies that the method returns an <see cref="OkObjectResult"/> with status 200.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetApplication_WhenApplicationExists_ReturnsOkWithApplication()
        {
            // arrange
            int id = 12;
            var application = new Application
            {
                Id = id
            };

            mockContext.Setup(c => c.Applications.FindAsync(id)).ReturnsAsync(application);

            // act 
            var result = await controller.GetApplication(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<Application>>();

            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            actionResult.StatusCode.Should().Be(200);

            var returnedValue = actionResult.Value as Application;
            Assert.IsNotNull(returnedValue);
            returnedValue.Id.Should().Be(id);
        }

        /// <summary>
        /// Tests the <see cref="ApplicationsController.GetApplication"/> method when an application is not found.
        /// Verifies that the method returns a <see cref="NotFoundObjectResult"/> with status 404 and a specific error message.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetApplication_WhenApplicationDoesNotExist_ReturnNotFound()
        {
            // arrange
            int id = 12;
            mockContext.Setup(c => c.Applications.FindAsync(id)).ReturnsAsync(null as Application);

            // act 
            var result = await controller.GetApplication(id);

            // assert
            result.Should().BeOfType<ActionResult<Application>>();

            var actionResult = result.Result as NotFoundObjectResult;

            Assert.IsNotNull(actionResult);
            actionResult.Value.Should().Be($"The Requested Application with Id: {id} does not Exist!");
            actionResult.StatusCode.Should().Be(404);
        }

        /// <summary>
        /// Tests the <see cref="ApplicationsController.GetApplications"/> method.
        /// Verifies that the method returns a list of Applications with an <see cref="OkObjectResult"/> status.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetApplications_WhenApplicationsExist_ReturnsOkWithApplicationsList()
        {
            // arrange
            var application = new List<Application>
            {
                new Application {
                Id = 26,
                Name = "Example",
                PhoneNumber = "123-1234",
                Email = "example@example.com",
                JobID = 123,
                JobSeekerID = 123,
                ApplicationCVPath = "/exa/mple/examplecv.pdf",
                Status = 0,
                CoverLetter = "Example Example Example",
                Date = DateTime.Parse("2024-08-14T12:34:56Z")
                },
                new Application {
                Id = 27,
                Name = "Example",
                PhoneNumber = "123-1234",
                Email = "example@example.com",
                JobID = 123,
                JobSeekerID = 123,
                ApplicationCVPath = "/exa/mple/examplecv.pdf",
                Status = 0,
                CoverLetter = "Example Example Example",
                Date = DateTime.Parse("2024-08-14T12:34:56Z")
                 }
            }.AsQueryable();

            var asyncEnumerator = new TestAsyncEnumerator<Application>(application.GetEnumerator());
            var mockSet = new Mock<DbSet<Application>>();
            var mockContext = new Mock<JobBoardContext>();
            var controller = new ApplicationsController(mockContext.Object, new ErrorMessages());

            mockSet.As<IQueryable<Application>>().Setup(m => m.Provider).Returns(application.Provider);
            mockSet.As<IQueryable<Application>>().Setup(m => m.Expression).Returns(application.Expression);
            mockSet.As<IQueryable<Application>>().Setup(m => m.ElementType).Returns(application.ElementType);
            mockSet.As<IQueryable<Application>>().Setup(m => m.GetEnumerator()).Returns(application.GetEnumerator());
            mockSet.As<IAsyncEnumerable<Application>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(asyncEnumerator);
            mockContext.Setup(c => c.Applications).Returns(mockSet.Object);

            // act
            var result = await controller.GetApplications();

            // assert
            result.Should().NotBeNull();
            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            actionResult.StatusCode.Should().Be(200);

            var returnedJobSeekers = actionResult.Value as IEnumerable<Application>;

            returnedJobSeekers.Should().NotBeNull();
            returnedJobSeekers.Should().HaveCount(2);
            returnedJobSeekers.Should().Contain(e => e.Id == 26);
            returnedJobSeekers.Should().Contain(e => e.Id == 27);
        }

        [TestMethod]
        public async Task DeleteApplication_WhenApplicationExists_ReturnsNoContent()
        {
            //Arrange
            int id = 3;
            var application = new Application();
            mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(application);
            mockSet.Setup(m => m.Remove(application));
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(id);

            // Act
            var result = await controller.DeleteApplication(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.IsNotNull(result);
            mockSet.Verify(m => m.Remove(application), Times.Once);
        }

        [TestMethod]
        public async Task DeleteApplication_WhenApplicationDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            int id = 10;
            Application? application = null;
            mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(application);

            // Act
            var result = await controller.DeleteApplication(id);

            // Assert
            Assert.IsNotNull(result);
            var NotFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(NotFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, NotFoundResult.StatusCode);
            mockSet.Verify(m => m.Remove(It.IsAny<Application>()), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutApplication_IDMismatch_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;
            var Application = new Application { Id = 2 };

            // Act
            var result = await controller.PutApplication(id, Application);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Application ID mismatch.", badRequestResult.Value);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutApplication_WhenUpdateIsSuccessful_ReturnsOk()
        {
            // Arrange
            var Application = new Application();
            int id = 1; Application.Id = id; Application.JobSeekerID = 3; Application.JobID = 3;
            mockContext.Setup(c => c.IsJobSeekerExistsAsync(Application.JobSeekerID)).ReturnsAsync(true);
            mockContext.Setup(c => c.IsJobExistsAsync(Application.JobID)).ReturnsAsync(true);
            mockContext.Setup(c => c.SetModified(Application));
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(id);

            // Act
            var result = await controller.PutApplication(id, Application) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(Application, result.Value);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
            mockContext.Verify(c => c.SetModified(Application), Times.Once);
        }

        [TestMethod]
        public async Task PutApplication_WhenJobDoesNotExist_ReturnsBadRequest()
        {
            //Arrange
            var Application = new Application();
            int id = 1; Application.Id = id;
            mockContext.Setup(c => c.IsJobExistsAsync(Application.JobID)).ReturnsAsync(false);

            //Act
            var result = await controller.PutApplication(id, Application);

            //Assert
            Assert.IsNotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            mockContext.Verify(c => c.SetModified(Application), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutApplication_WhenJobSeekerDoesNotExist_ReturnsBadRequest()
        {
            //Arrange
            var Application = new Application();
            int id = 1; Application.Id = id;
            mockContext.Setup(c => c.IsJobSeekerExistsAsync(Application.JobSeekerID)).ReturnsAsync(false);

            //Act
            var result = await controller.PutApplication(id, Application);

            //Assert
            Assert.IsNotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            mockContext.Verify(c => c.SetModified(Application), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutApplication_WhenApplicationDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var Application = new Application();
            int id = 1; Application.Id = id; Application.JobSeekerID = 3; Application.JobID = 3;
            mockContext.Setup(c => c.IsJobSeekerExistsAsync(Application.JobSeekerID)).ReturnsAsync(true);
            mockContext.Setup(c => c.IsJobExistsAsync(Application.JobID)).ReturnsAsync(true);
            mockContext.Setup(m => m.SetModified(Application)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Application>(Application.Id)).Returns(false);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateConcurrencyException());

            //Act
            var result = await controller.PutApplication(id, Application);

            //Assert
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task PutApplication_WhenApplicationExists_ReturnsConflict()
        {
            //Arrange
            var Application = new Application();
            int id = 1; Application.Id = id; Application.JobSeekerID = 3; Application.JobID = 3;
            mockContext.Setup(c => c.IsJobSeekerExistsAsync(Application.JobSeekerID)).ReturnsAsync(true);
            mockContext.Setup(c => c.IsJobExistsAsync(Application.JobID)).ReturnsAsync(true);
            mockContext.Setup(m => m.SetModified(Application)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Application>(Application.Id)).Returns(true);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateConcurrencyException());

            //Act
            var result = await controller.PutApplication(id, Application);

            //Assert
            Assert.IsNotNull(result);
            var ConflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(ConflictResult);
            Assert.AreEqual(409, ConflictResult.StatusCode);
        }

        [TestMethod]
        public async Task PutApplication_WhenUnexpectedErrorOccurred_ReturnsStatusCode()
        {
            //Arrange
            var Application = new Application();
            int id = 1; Application.Id = id; Application.JobSeekerID = 3; Application.JobID = 3;
            mockContext.Setup(c => c.IsJobSeekerExistsAsync(Application.JobSeekerID)).ReturnsAsync(true);
            mockContext.Setup(c => c.IsJobExistsAsync(Application.JobID)).ReturnsAsync(true);
            mockContext.Setup(m => m.SetModified(Application)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Application>(Application.Id)).Returns(true);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new Exception());

            //Act
            var result = await controller.PutApplication(id, Application);

            //Assert
            Assert.IsNotNull(result);
            var StatusCodeResult = result as ObjectResult;
            Assert.IsNotNull(StatusCodeResult);
            Assert.AreEqual(500, StatusCodeResult.StatusCode);
        }
    }
}
