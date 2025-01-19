using System.ComponentModel.DataAnnotations;

namespace productManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; }  // 'Admin' or 'Customer'

        [Required]
        public string Email { get; set; }


    }
}