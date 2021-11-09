using AutoMapper;
using LibraryApi.Data.Interfaces;
using LibraryApi.Models.Dtos;
using LibraryApi.Models.Entities;
using LibraryApi.Services.Implementations;
using LibraryApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApi.Test.ServicesTests
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IServiceFactory> _mockServiceFactory;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IOptions<DaySettings>> _mockOptions;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockServiceFactory = new Mock<IServiceFactory>();
            
            _mockMapper = new Mock<IMapper>();
            
            _mockOptions = new Mock<IOptions<DaySettings>>();
            
            _bookService = new BookService(_mockUnitOfWork.Object,
                _mockServiceFactory.Object,
                _mockMapper.Object,
                _mockOptions.Object);
        }

        [Fact]
        public void GetBooksAsync_ReturnsListOfBooksDto()
            => Assert.IsType<Task<IEnumerable<ViewBookDto>>>(_bookService.GetBooksAsync());
    }
}
