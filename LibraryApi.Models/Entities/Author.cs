using System;
using LibraryApi.Models.Enumerators;
using System.Collections.Generic;
using LibraryApi.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Models.Entities
{
    public class Author : ITracking, ISoftDelete
    {
        public Author()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            CreatedBy = AppRole.Admin.ToString();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
