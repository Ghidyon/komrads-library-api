﻿using AutoMapper;
using LibraryApi.Data.Interfaces;
using LibraryApi.Models.Dtos;
using LibraryApi.Models.Entities;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;


        public AuthorsController(IAuthorService authorService, IBookService bookService)
        {
            this.authorService = authorService;
            this.bookService = bookService;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await authorService.GetAuthorsAsync();

            return Ok(authors);
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetSingleAuthor(Guid id)
        {
            var author = await authorService.GetAuthorByIdAsync(id);

            return Ok(author);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> AddSingleAuthor([FromBody]AuthorForCreationDto authorForCreationDto)
        {
            if (authorForCreationDto == null) return BadRequest();

            var authorToReturn = await authorService.CreateAuthorAsync(authorForCreationDto);

            if (authorToReturn is null) return BadRequest("Invalid User Id!");

            return CreatedAtRoute("GetAuthor", new { id = authorToReturn.Id },  authorToReturn);
        }

        [HttpPost("{id}/books")]
        [Authorize(Policy = "RequireAuthorOrAdminRole")]
        public async Task<IActionResult> AddAuthorBook(Guid id, [FromBody]BookForCreationDto bookForCreationDto)
        {
            if (bookForCreationDto == null) return BadRequest();

            var author = authorService.GetAuthorByIdAsync(id);
            if (author == null) return BadRequest("AuthorId is invalid or null!");

            var book = await bookService.CreateBookAsync(bookForCreationDto);

            return CreatedAtRoute("GetAuthor", new { id = book.AuthorId }, book);
        }


        /* Gideon's Review
         * Davidson advised during the review to use Patch instead of Put
         * To be able to update properties excluding the "Name" property in the Author class.
         * Please implement the Update method using PATCH
         */
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAuthorOrAdminRole")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody]AuthorForUpdateDto authorForUpdateDto)
        {
            if (authorForUpdateDto == null) return BadRequest("authorDto is null!");

            var author = await authorService.GetAuthorByIdAsync(id);
            if (author == null) return BadRequest("Author dosen't exist!");

            authorService.UpdateAuthor(id, authorForUpdateDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult DeleteAuthor(Guid id)
        {
            authorService.DeleteAuthor(id);

            return NoContent();
        }

        [HttpPut("{authorId}/books/{id}")]
        [Authorize(Policy = "RequireAuthorOrAdminRole")]
        public async Task<IActionResult> UpdateAuthorBook(Guid authorId, [FromBody]BookForUpdateDto bookForUpdate, Guid id)
        {
            var author = await authorService.GetAuthorByIdAsync(authorId);
            if (author == null) return BadRequest("AuthorId is invalid!");

            if (bookForUpdate == null) return BadRequest("Book is null"); 

            bookService.UpdateBook(id, bookForUpdate);

            return NoContent();
        }

        [HttpGet("{id}/books")]
        [Authorize(Policy = "RequireAuthorOrAdminRole")]
        public async Task<IActionResult> GetAuthorBooks(Guid id)
        {
            var books = await bookService.GetBooksByAuthorIdAsync(id);
            
            if (books == null) return BadRequest("AuthorId is invalid!");

            return Ok(books);
        }
    }
}
