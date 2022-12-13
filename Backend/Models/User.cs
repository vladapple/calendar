using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Backend.Models
{
    public class User
    {
        public User()
        {

        }
        public User(string userName, string email, string password, DateTimeOffset dOB, string contactName, string organizationName)
        {
            //For Registration
            UserName = userName;
            Email = email;
            Password = password;
            DOB = dOB;
            ContactName = contactName;
            OrganizationName = organizationName;
            IsBanned = false;
            Role = 0;
        }

        [Key]
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string Password { get; set; }

        [Required]
        public DateTimeOffset DOB { get; set; }

        [MaxLength(100), AllowNull]
        public string ContactName { get; set; }

        [MaxLength(100), AllowNull]
        public string OrganizationName { get; set; }

        [Required, DefaultValue(false)]
        public bool IsBanned { get; set; }

        [EnumDataType(typeof(Roles)), DefaultValue(Roles.user)]
        public Roles Role { get; set; }

        public enum Roles
        {
            user,
            admin
        }
    }
}




