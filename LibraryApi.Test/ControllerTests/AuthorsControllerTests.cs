using AutoMapper;
using LibraryApi.Controllers;
using LibraryApi.Models.Entities;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApi.Test.ControllerTests
{
    public class AuthorsControllerTests
    {
        private readonly Mock<IAuthorService> _mockAuthorService;
        private readonly AuthorsController _authorsController;
        private readonly Mock<IBookService> _mockBookService;
        private readonly IMapper _mapper;

        public AuthorsControllerTests()
        {
            _mockAuthorService = new Mock<IAuthorService>();
            _authorsController = new AuthorsController(_mockAuthorService.Object, _mockBookService.Object, _mapper);
        }

        [Fact]
        public void GetAuthors_ActionExecutes_ReturnsIActionResult()
        {
            Assert.IsType<Task<IActionResult>>(_authorsController.GetAuthors());
        }
       
        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {
            var okResult = await _authorsController.GetAuthors();

            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public void GetAuthorById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            var notFoundResult = _authorsController.GetSingleAuthor(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async void GetAuthorById_ExistingGuidPassed_ReturnsOkResult()
        {
            var testGuid = new Guid("");

            var okResult = await _authorsController.GetSingleAuthor(testGuid);

            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async void GetAuthorById_ExistingGuidPassed_ReturnsRightItem()
        {
            var testGuid = new Guid("");

            var okResult = await _authorsController.GetSingleAuthor(testGuid) as OkObjectResult;

            Assert.IsType<Author>(okResult.Value);
            Assert.Equal(testGuid, (okResult.Value as Author).Id);
        }

        //Can't find post in the controller action
        /*[Fact]
        public async void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            var authorMissing = new Author()
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe"
            };
            _authorsController.ModelState.AddModelError("Email", "Required");

            var badResponse = await _authorsController.Post(authorMissing);

            Assert.IsType<BadRequestObjectResult>(badResponse);
        }*/
    }
}
