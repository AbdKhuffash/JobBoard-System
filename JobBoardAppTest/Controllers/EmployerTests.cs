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

namespace ControllersTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="EmployersController"/> class.
    /// </summary>
    [TestClass]
    public class EmployerTests
    {
        private readonly EmployersController controller;
        private readonly Mock<DbSet<Employer>> mockSet;
        private readonly Mock<JobBoardContext> mockContext;
        private readonly Mock<IMapper> mockMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployerTests"/> class.
        /// Sets up mocks and the controller instance.
        /// </summary>
        public EmployerTests()
        {
            mockSet = new Mock<DbSet<Employer>>();
            mockContext = new Mock<JobBoardContext>();
            mockMapper = new Mock<IMapper>();

            mockContext.Setup(m => m.Employers).Returns(mockSet.Object);
            controller = new EmployersController(mockContext.Object, mockMapper.Object, new ErrorMessages());
        }

        /// <summary>
        /// Tests the <see cref="EmployersController.PostEmployer"/> method.
        /// Verifies that a newly created employer is returned with a <see cref="CreatedAtActionResult"/> status.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task PostEmployer_WhenEmployerIsValid_ReturnsCreatedAtActionResultWithNewEmployer()
        {
            // arrange
            var employer = new Employer
            {
                Id = 26,
                Email = "employer@example.com",
                FirstName = "exam",
                LastName = "ple",
                PhoneNumber = "123-1234",
                Password = "examplePassword1",
                Address = "123 Example Street",
                CompanyName = "Example Ltd."
            };

            mockSet.Setup(m => m.FindAsync(employer.Id)).ReturnsAsync(null as Employer);
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(employer.Id);

            // act
            var result = await controller.PostEmployer(employer);

            // assert
            mockSet.Verify(m => m.AddAsync(It.Is<Employer>(e => e == employer), default), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            result.Should().NotBeNull();

            var createdAtActionResult = result.Should().NotBeNull().And.BeOfType<ActionResult<IEnumerable<Employer>>>().Which.Result.Should().BeOfType<CreatedAtActionResult>().Which;

            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be("GetEmployer");
            createdAtActionResult.Value.Should().Be(employer);
            createdAtActionResult.StatusCode.Should().Be(201);


        }

        /// <summary>
        /// Tests the <see cref="EmployersController.GetEmployer"/> method when an employer is found.
        /// Verifies that the method returns an <see cref="OkObjectResult"/> with status 200.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetEmployer_WhenEmployerExists_ReturnsOkWithEmployer()
        {
            // arrange
            int id = 1;
            var employer = new Employer
            {
                Id = id
            };

            var employerDTO = new EmployerDTO
            {
                Id = id
            };

            mockContext.Setup(c => c.Employers.FindAsync(id)).ReturnsAsync(employer);
            mockMapper.Setup(m => m.Map<EmployerDTO>(It.IsAny<Employer>())).Returns(employerDTO);

            // act 
            var result = await controller.GetEmployer(id);

            // assert
            result.Should().NotBeNull();

            result.Should().BeOfType<ActionResult<Employer>>();

            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(200);

            var returnedValue = actionResult.Value as EmployerDTO;
            Assert.IsNotNull(returnedValue);
            returnedValue.Id.Should().Be(id);
        }

        /// <summary>
        /// Tests the <see cref="EmployersController.GetEmployer"/> method when an employer is not found.
        /// Verifies that the method returns a <see cref="NotFoundObjectResult"/> with status 404 and a specific error message.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetEmployer_WhenEmployerDoesNotExist_ReturnNotFound()
        {
            // arrange
            int id = 12;
            mockContext.Setup(c => c.Employers.FindAsync(id)).ReturnsAsync(null as Employer);

            // act 
            var result = await controller.GetEmployer(id);

            // assert
            result.Should().BeOfType<ActionResult<Employer>>();

            var actionResult = result.Result as NotFoundObjectResult;

            actionResult.Should().NotBeNull();
            Assert.IsNotNull(actionResult);
            actionResult.Value.Should().Be($"The Requested Employer With Id: {id} does not Exist!");
            actionResult.StatusCode.Should().Be(404);
        }

        /// <summary>
        /// Tests the <see cref="EmployersController.GetEmployers"/> method.
        /// Verifies that the method returns a list of employers with an <see cref="OkObjectResult"/> status.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetEmployers_WhenEmployersExist_ReturnsOkWithEmployersList()
        {
            // Arrange
            var employers = new List<Employer>
            {
                new Employer
                {
                    Id = 26,
                    Email = "employer1@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "123-1234",
                    Password = "examplePassword1",
                    Address = "123 Example Street",
                    CompanyName = "Example Ltd."
                },
                new Employer
                {
                    Id = 27,
                    Email = "employer2@example.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    PhoneNumber = "456-5678",
                    Password = "examplePassword2",
                    Address = "456 Example Avenue",
                    CompanyName = "Example Inc."
                }
            }.AsQueryable();

            var employersDTO = new List<EmployerDTO>
            {
                new EmployerDTO
                {
                    Id = 26,
                    FirstName = "John",
                    LastName = "Doe",
                    CompanyName = "Example Ltd."
                },
                new EmployerDTO
                {
                    Id = 27,
                    FirstName = "Jane",
                    LastName = "Smith",
                    CompanyName = "Example Inc."
                }
            };

            var asyncEnumerator = new TestAsyncEnumerator<Employer>(employers.GetEnumerator());
            var mockSet = new Mock<DbSet<Employer>>();
            var mockContext = new Mock<JobBoardContext>();
            var mockMapper = new Mock<IMapper>();
            var controller = new EmployersController(mockContext.Object, mockMapper.Object, new ErrorMessages());

            mockSet.As<IQueryable<Employer>>().Setup(m => m.Provider).Returns(employers.Provider);
            mockSet.As<IQueryable<Employer>>().Setup(m => m.Expression).Returns(employers.Expression);
            mockSet.As<IQueryable<Employer>>().Setup(m => m.ElementType).Returns(employers.ElementType);
            mockSet.As<IQueryable<Employer>>().Setup(m => m.GetEnumerator()).Returns(employers.GetEnumerator());

            mockSet.As<IAsyncEnumerable<Employer>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(asyncEnumerator);
            mockContext.Setup(c => c.Employers).Returns(mockSet.Object);

            mockMapper.Setup(m => m.Map<List<EmployerDTO>>(It.IsAny<List<Employer>>())).Returns(employersDTO);

            // Act
            var result = await controller.GetEmployers();

            // Assert
            result.Should().NotBeNull();

            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            actionResult.StatusCode.Should().Be(200);

            var returnedEmployers = actionResult.Value as IEnumerable<EmployerDTO>;
            returnedEmployers.Should().NotBeNull();
            returnedEmployers.Should().HaveCount(2);
            returnedEmployers.Should().Contain(e => e.Id == 26);
            returnedEmployers.Should().Contain(e => e.Id == 27);

        }

        [TestMethod]
        public async Task DeleteEmployer_WhenEmployerExists_ReturnsNoContent()
        {
            //Arrange
            int id = 2;
            var employer = new Employer();
            mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(employer);
            mockSet.Setup(m => m.Remove(employer));
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(id);

            // Act
            var result = await controller.DeleteEmployer(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.IsNotNull(result);
            mockSet.Verify(m => m.Remove(employer), Times.Once);
        }

        [TestMethod]
        public async Task DeleteEmployer_WhenEmployerNotExist_ReturnsNotFound()
        {
            // Arrange
            int id = 10;
            Employer? employer = null;
            mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(employer);

            // Act
            var result = await controller.DeleteEmployer(id);

            // Assert
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            mockSet.Verify(m => m.Remove(It.IsAny<Employer>()), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [TestMethod]
        public async Task PutEmployer_IDMismatch_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;
            var employer = new Employer { Id = 2 };

            // Act
            var result = await controller.PutEmployer(id, employer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Employer ID mismatch.", badRequestResult.Value);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }


        [TestMethod]
        public async Task PutEmployer_WhenUpdateIsSuccessful_ReturnsOk()
        {
            // Arrange
            var employer = new Employer();
            int id = 1; employer.Id = id;
            mockContext.Setup(c => c.SetModified(employer));
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(id);

            // Act
            var result = await controller.PutEmployer(id, employer) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(employer, result.Value);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
            mockContext.Verify(c => c.SetModified(employer), Times.Once);
        }

        [TestMethod]
        public async Task PutEmployer_WhenEmployerDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var employer = new Employer();
            int id = 1; employer.Id = id;
            mockContext.Setup(m => m.SetModified(employer)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Employer>(employer.Id)).Returns(false);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateConcurrencyException());

            //Act
            var result = await controller.PutEmployer(id, employer);

            //Assert
            Assert.IsNotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task PutEmployer_WhenEmployerExists_ReturnsConflict()
        {
            //Arrange
            var employer = new Employer();
            int id = 1; employer.Id = id;
            mockContext.Setup(m => m.SetModified(employer)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Employer>(employer.Id)).Returns(true);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateConcurrencyException());

            //Act
            var result = await controller.PutEmployer(id, employer);

            //Assert
            Assert.IsNotNull(result);
            var ConflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(ConflictResult);
            Assert.AreEqual(409, ConflictResult.StatusCode);
        }

        [TestMethod]
        public async Task PutEmployer_WhenUnexpectedErrorOccurred_ReturnsStatusCode()
        {
            //Arrange
            var employer = new Employer();
            int id = 1; employer.Id = id;
            mockContext.Setup(m => m.SetModified(employer)).Verifiable();
            mockContext.Setup(m => m.EntityExists<Employer>(employer.Id)).Returns(true);
            mockContext.Setup(m => m.SaveChangesAsync(default))
                       .ThrowsAsync(new Exception());

            //Act
            var result = await controller.PutEmployer(id, employer);

            //Assert
            Assert.IsNotNull(result);
            var StatusCodeResult = result as ObjectResult;
            Assert.IsNotNull(StatusCodeResult);
            Assert.AreEqual(500, StatusCodeResult.StatusCode);
        }
    }
}
