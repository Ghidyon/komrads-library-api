using Microsoft.AspNetCore.Identity;
using LibraryApi.Models.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using LibraryApi.Services.Interfaces;

namespace LibraryApi
{
    public static class Seeding
    {
        private const string User1 = "g.akunana";
        private const string User1Password = "Gideon@12345678";
        private const string User2 = "t.sage";
        private const string User2Password = "Sage@12345678";
        private const string User3 = "k.michael";
        private const string User3Password = "Michael@12345678";
        private const string User4 = "q.njideka";
        private const string User4Password = "Njideka@12345678";

        public static async Task InitializeData(IApplicationBuilder app, ILoggerManager logger)
        {

            IdentityContext context = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<IdentityContext>();

            UserManager<User> userManager = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<UserManager<User>>();

            RoleManager<Role> roleManager = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<RoleManager<Role>>();


            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                context.Database.Migrate();
            }

            await SeedRoles(roleManager);
            
            await SeedUsers(userManager);

            await SeedCategories(context, logger);

            await SeedAuthors(context, userManager, logger);

            await SeedBooks(context, logger);
        }

        public static async Task SeedUsers(UserManager<User> userManager)
        {
            var user1 = await userManager.FindByNameAsync(User1);
            if (user1 != null) return;

            user1 = new User
            {
                FirstName = "Gideon",
                LastName = "Akunana",
                Email = "gideonakunana@domain.com",
                UserName = "g.akunana",
                PhoneNumber = "+2348160451288",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1993, 2, 15),
                Gender = Gender.Male,
                CreatedBy = "Gideon",
            };

            var createUser1 = await userManager.CreateAsync(user1, User1Password);
            if (createUser1.Succeeded) 
                await userManager.AddToRoleAsync(user1, AppRole.Admin.ToString());


            var user2 = await userManager.FindByNameAsync(User2);
            if (user2 != null) return;

            user2 = new User
            {
                FirstName = "Tochukwu",
                LastName = "Sage",
                Email = "tochukwusage@domain.com",
                UserName = "t.sage",
                PhoneNumber = "+2348160451288",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1993, 2, 15),
                Gender = Gender.Male,
                CreatedBy = "Gideon",
            };

            var createUser2 = await userManager.CreateAsync(user2, User2Password);
            if (createUser2.Succeeded) 
                await userManager.AddToRoleAsync(user2, AppRole.User.ToString());


            var user3 = await userManager.FindByNameAsync(User3);
            if (user3 != null) return;

            user3 = new User
            {
                FirstName = "Kenechukwu",
                LastName = "Michael",
                Email = "kcmichael@domain.com",
                UserName = "k.michael",
                PhoneNumber = "+2348160451288",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1993, 2, 15),
                Gender = Gender.Male,
                CreatedBy = "Gideon",
            };


            var createUser3 = await userManager.CreateAsync(user3, User3Password);
            if (createUser3.Succeeded)
                await userManager.AddToRoleAsync(user3, AppRole.User.ToString());
            
            var user4 = await userManager.FindByNameAsync(User4);
            if (user4 != null) return;

            user4 = new User
            {
                FirstName = "Queen",
                LastName = "Njideka",
                Email = "queennjideka@domain.com",
                UserName = "q.njideka",
                PhoneNumber = "+2348160451288",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1993, 2, 15),
                Gender = Gender.Female,
                CreatedBy = "Gideon",
            };


            var createUser4 = await userManager.CreateAsync(user4, User4Password);
            if (createUser4.Succeeded)
                await userManager.AddToRoleAsync(user4, AppRole.User.ToString());
        }

        public static async Task SeedCategories(IdentityContext context, ILoggerManager logger)
        {
            if (context.Categories.Any())
            {
                logger.LogInfo("Categories is already seeded!");
                return;
            }

            await context.Categories.AddRangeAsync(Categories());

            await context.SaveChangesAsync();
            logger.LogInfo("Categories successfully seeded!");
        }

        private static async Task CreateRole(RoleManager<Role> roleManager, string name)
        {
            if (!await roleManager.RoleExistsAsync(name))
            {
                var role = new Role { Name = name };
                await roleManager.CreateAsync(role);
            }
        }

        public static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            await CreateRole(roleManager, AppRole.Admin.ToString());
            await CreateRole(roleManager, AppRole.Author.ToString());
            await CreateRole(roleManager, AppRole.User.ToString());
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

        public static async Task SeedAuthors(IdentityContext context, 
            UserManager<User> userManager, ILoggerManager logger)
        {
            var user2 = await userManager.FindByNameAsync(User2);

            if (user2 != null) return;

            context.Authors.Add(new Author
            {
                FirstName = user2.FirstName,
                LastName = user2.LastName,
                UserId = user2.Id
            });
            
            await userManager.AddToRoleAsync(user2, AppRole.Author.ToString());    
            
            
            var user3 = await userManager.FindByNameAsync(User3);

            if (user3 != null) return;

            context.Authors.Add(new Author
            {
                FirstName = user3.FirstName,
                LastName = user3.LastName,
                UserId = user3.Id
            });
            
            await userManager.AddToRoleAsync(user3, AppRole.Author.ToString());

            await context.SaveChangesAsync();

            logger.LogInfo("Authors successfully seeded!");
        }

        public static async Task SeedBooks(IdentityContext context, ILoggerManager logger)
        {
            if (context.Books.Any())
            {
                logger.LogInfo("Books is already seeded!");
                return;
            }

            var authors = await Authors(context);

            if (authors is null) 
            {
                logger.LogError("author has not been seeded, therefore book cannot be seeded!");
                return;
            }

            var books = new List<Book>
            {
                 new Book
                {
                    Title = "Success Comes At A Price",
                    ISBN = "56422299875",
                    YearPublished = new DateTime(2006, 12, 23),
                    AuthorId = authors[0].Id,
                    Category = "Career",
                },

                new Book
                {
                    Title = "Toolkit for Mentorship",
                    ISBN = "234977423470",
                    YearPublished = new DateTime(2018, 2, 18),
                    AuthorId = authors[0].Id,
                    Category = "Career",
                },

                new Book
                {
                    Title = "Principles of Expansion",
                    ISBN = "882457625741",
                    YearPublished = new DateTime(2008, 8, 13),
                    AuthorId = authors[1].Id,
                    Category = "Sales",
                },

                new Book
                {
                    Title = "Keys of Networking",
                    ISBN = "536648497957",
                    YearPublished = new DateTime(2017, 9, 15),
                    AuthorId = authors[1].Id,
                    Category = "Sales",
                }
            };
            
            context.Books.AddRange(books);

            await context.SaveChangesAsync();

            logger.LogInfo("Books successfully seeded!");
        }

        private static async Task<List<Author>> Authors(IdentityContext context)
        {
            return await context.Authors.ToListAsync();
        }
    }
}
