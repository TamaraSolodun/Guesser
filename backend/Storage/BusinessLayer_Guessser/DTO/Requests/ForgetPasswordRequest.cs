using System.ComponentModel.DataAnnotations;

namespace BusinessLayer_Guesser.DTO.Requests
{
    public class ForgetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
