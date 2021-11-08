using LibraryApi.Controllers;
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
        public void GetBooks_ActionExecutes_ReturnsListOfBooks()
        {
            Assert.IsType<Task<IActionResult>>(_booksController.GetBooks());
        }
    }
}
