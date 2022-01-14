using BusinessLayer_Guesser.DTO;
using DataLayer_Guesser.Models;

namespace BusinessLayer_Guesser.DTO
{
    public class UserResponse
    {
        public int Id { get; set; }
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}
