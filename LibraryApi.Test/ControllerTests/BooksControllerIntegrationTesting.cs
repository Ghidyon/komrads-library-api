using LibraryApi.Models.Dtos;
using LibraryApi.Test.TestUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace LibraryApi.Test.ControllerTests
{
    public class BooksControllerIntegrationTesting : IClassFixture<CustomApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        public BooksControllerIntegrationTesting(CustomApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetBooks_ReturnsListOfBooks()
        {
            // Arrange
            string url = Endpoints.Books;

            // Act
            var response = await _httpClient.GetAsync(url);

            //response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            var responseResult = JsonConvert.DeserializeObject<IEnumerable<ViewBookDto>>(content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
