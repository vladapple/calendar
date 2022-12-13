using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Frontend.Models
{
    public class User
    { 
        public int Id { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password and confirm password do not match.")]
        public string confirmpwd { get; set; }

        public DateTimeOffset DOB { get; set; }

        [MaxLength(100), AllowNull]
        public string ContactName { get; set; }

        [MaxLength(100), AllowNull]
        public string OrganizationName { get; set; }

        [DefaultValue(false)]
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




