using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer_Guesser.Models
{

    public enum UserStatus
    {
        Registered,
        Active,
        Blocked,
        Disabled
    }
    public enum UserType
    {
        Player,
        Admin
    }

    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public UserStatus UserStatus { get; set; }
        [Required]
        public UserType UserType { get; set; }
        
        public virtual List<Game> Games { get; set; } 
    }
}
