﻿using LibraryApi.Models.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Models.Dtos
{
    public class UserForRegistrationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        //public DateTime DateOfBirth { get; set; } = DateTime.Now;
        //public Gender Gender { get; set; } = Gender.Male;
        //public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public DateTime UpdatedAt { get; set; } = DateTime.Now;
        //public string CreatedBy { get; set; } = "Tochukwu";
        //public string UpdatedBy { get; set; } = "Tochukwu";
    }
}