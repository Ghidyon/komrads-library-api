﻿using LibraryApi.Data.Interfaces;
using LibraryApi.Models.Entities;
using LibraryApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ICategoryService _categoryService;

        public UserService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
            _bookService = serviceFactory.GetService<BookService>();
            _authorService = serviceFactory.GetService<AuthorService>();
            _categoryService = serviceFactory.GetService<CategoryService>();
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            return await _authorService.CreateAuthorAsync(author);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            return await _bookService.CreateBookAsync(book);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            return await _categoryService.CreateCategoryAsync(category);
        }

        public void DeleteAuthor(Author author)
        {
            _authorService.DeleteAuthor(author);
        }

        public void DeleteBook(Book book)
        {
            _bookService.DeleteBook(book);
        }

        public void DeleteCategory(Category category)
        {
            _categoryService.DeleteCategory(category);
        }

        public Task<IEnumerable<Activity>> GetActivities()
        {
            throw new NotImplementedException();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _bookService.GetBookByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _bookService.GetBooksAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(string category)
        {
            return await _bookService.GetBooksByCategoryAsync(category);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryService.GetCategoriesAsync();
        }

        public async Task<Category> GetCategory(Guid id)
        {
            return await _categoryService.GetCategoryAsync(id);
        }

        public Task<IEnumerable<Activity>> GetUserActivities(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAuthorAsync(Author author)
        {
            _authorService.UpdateAuthor(author);
        }

        public void UpdateBook(Book book)
        {
            _bookService.UpdateBook(book);
        }

        public void UpdateCategory(Category category)
        {
            _categoryService.UpdateCategory(category);
        }
    }
}