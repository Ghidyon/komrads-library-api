using LibraryApi.Controllers;
using LibraryApi.Models.Dtos;
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
    //Creating a Mock Object
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BooksController _booksController;

        public BookControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _booksController = new BooksController(_mockBookService.Object);
        }

        [Fact]
        public void GetBooks_ActionExecutes_ReturnsIActionResult()
        {
            Assert.IsType<Task<IActionResult>>(_booksController.GetBooks());
        }

        [Fact]
        public void GetBook_ActionExecutes_ReturnsIActionResult()
        {
            Assert.IsType<Task<IActionResult>>
                (_booksController.GetBook(Guid.Parse("854ca127-dbee-4419-6bfc-08d9a23de3c9")));
        }

        [Fact]
        public void GetBooksByCategory_ActionExecutes_ReturnsIActionResult()
        {
            Assert.IsType<Task<IActionResult>>(_booksController.GetBooksByCategory("career"));
        }

        [Fact]
        public void RequestBook_ActionExecutes_ReturnsIActionResult()
        {
            Assert.IsType<Task<IActionResult>>
                (_booksController.RequestBook(Guid.Parse("854ca127-dbee-4419-6bfc-08d9a23de3c9")));
        }

        [Fact]
        public void DeleteBook_ActionExecutes_ReturnsNoContentResult()
        {
            Assert.IsType<NoContentResult>
                (_booksController.DeleteBook(Guid.Parse("cc569c3c-1431-411b-6bfe-08d9a23de3c9")));
        }
    }
}
