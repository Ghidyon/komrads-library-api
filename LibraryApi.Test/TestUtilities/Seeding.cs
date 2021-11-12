using LibraryApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Test.TestUtilities
{
    public static class Seeding
    {
        public static async void InitializeData(IdentityContext context)
        {
            await SeedCategories(context);

            await SeedAuthors(context);

            await SeedBooks(context);
        }

        public static async Task SeedCategories(IdentityContext context)
        {
            if (context.Categories.Any()) return;

            await context.Categories.AddRangeAsync(Categories());

            await context.SaveChangesAsync();
        }

        private static List<Category> Categories()
        {
            return new List<Category> {
                new Category
                {
                    Id = new Guid("8bde71e7-b24a-42cd-ad44-08d98a13616c"),
                    Name = "Motivation",

                },

                new Category
                {
                    Id = new Guid("44935b91-3283-4f39-f08d-08d98a1078df"),
                    Name = "Non-fiction",

                },

                new Category
                {
                    Id = new Guid("dff0527f-8f24-4402-215f-08d98a13c3e8"),
                    Name = "Career",

                },

                new Category
                {
                    Id = new Guid("6007d5fa-3a6e-436d-8854-08d98a27ad68"),
                    Name = "Sales",

                }
            };
        }

        public static async Task SeedAuthors(IdentityContext context)
        {
            context.Authors.AddRange(Authors());

            await context.SaveChangesAsync();
        }

        public static async Task SeedBooks(IdentityContext context)
        {
            if (context.Books.Any())
                return;

            var authors = Authors();

            if (authors is null)
                return;

            var books = new List<Book>
            {
                new Book
                {
                    Id = Guid.Parse("E8D812A5-05A3-40B8-970F-BB1D052A61CF"),
                    Title = "Success Comes At A Price",
                    ISBN = "56422299875",
                    YearPublished = new DateTime(2006, 12, 23),
                    AuthorId = authors[0].Id,
                    Category = "Career",
                },

                new Book
                {
                    Id = Guid.Parse("E93AC9EC-DB0C-405F-B0C3-BF812D8A1AAF"),
                    Title = "Toolkit for Mentorship",
                    ISBN = "234977423470",
                    YearPublished = new DateTime(2018, 2, 18),
                    AuthorId = authors[0].Id,
                    Category = "Career",
                },

                new Book
                {
                    Id = Guid.Parse("EC104235-7802-43B2-9BBF-A0D5DE8677DD"),
                    Title = "Principles of Expansion",
                    ISBN = "882457625741",
                    YearPublished = new DateTime(2008, 8, 13),
                    AuthorId = authors[1].Id,
                    Category = "Sales",
                },

                new Book
                {
                    Id = Guid.Parse("B219780E-C8E5-4D67-AB57-AB1D2D789292"),
                    Title = "Keys of Networking",
                    ISBN = "536648497957",
                    YearPublished = new DateTime(2017, 9, 15),
                    AuthorId = authors[1].Id,
                    Category = "Sales",
                }
            };

            context.Books.AddRange(books);

            await context.SaveChangesAsync();
        }

        private static List<Author> Authors()
        {
            return new List<Author> { 
                new Author
                {
                    Id = Guid.Parse("1C7CA81E-FFAF-4175-9207-5F2DD2E0AA38"),
                    FirstName = "Gideon",
                    LastName = "Akunana",
                    UserId = "5A3620F8A9B1474EB7683E4763A06DB8"
                },

                new Author
                {
                    Id = Guid.Parse("371DA9AB-00BE-4733-B7D7-DD03F0C234E5"),
                    FirstName = "Peter",
                    LastName = "Agu",
                    UserId = "479BC698B0834C2D8463BED30C37660F"
                }
            };
        }

    }
}
