﻿using LibraryApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> CreateAuthor(Author author);
        Task<Author> GetAuthorByIdAsync(Guid authorId);
        Task<IEnumerable<Author>> GetAuthors();
        void UpdateAuthor(Author author);
        void DeleteAuthor(Author author);
    }
}
