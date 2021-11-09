using LibraryApi.Models.Dtos;
using LibraryApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CreateBookAsync(BookForCreationDto book);
        Task<IEnumerable<ViewBookDto>> GetBooksByAuthorIdAsync(Guid id);
        Task<ViewBookDto> GetBookByIdAsync(Guid id);
        Task<Book> GetBookByIdForUpdateAsync(Guid id, bool trackChanges);
        Task<IEnumerable<ViewBookDto>> GetBooksAsync();
        Task<IEnumerable<ViewBookDto>> GetBooksByCategoryAsync(string category);
        void UpdateBook(Guid id, BookForUpdateDto bookForUpdateDto);
        void DeleteBook(Guid id);
        Task<ViewActivityDto> RequestBook(Guid userId, Guid bookId);
    }
}
