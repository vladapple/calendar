using System.Diagnostics.CodeAnalysis;

namespace Backend.Dtos
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTimeOffset DOB { get; set; } = DateTimeOffset.UtcNow.AddYears(-18);

        [AllowNull]
        public string ContactName { get; set; } = string.Empty;

        [AllowNull]
        public string OrganizationName { get; set; } = string.Empty;
    }
}
