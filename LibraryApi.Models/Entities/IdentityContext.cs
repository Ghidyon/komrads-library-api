using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Models.Entities
{
    public class IdentityContext : IdentityDbContext<User, Role, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity => entity.ToTable(name: "Users"));

            builder.Entity<Role>(entity => entity.ToTable(name: "Roles"));

            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"));

            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable(name: "UserClaims"));

            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable(name: "UserLogins"));

            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable(name: "RoleClaims"));

            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable(name: "UserTokens"));

            //builder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            builder.Entity<Author>().HasQueryFilter(a => !a.IsDeleted);

            builder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);
            
            builder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            
            builder.Entity<Activity>().HasQueryFilter(a => !a.Book.IsDeleted);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
