using System.ComponentModel.DataAnnotations;

namespace BusinessLayer_Guesser.DTO.Requests
{
    public class ChangePasswordUsingEmailRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewPass { get; set; }
        [Required]
        public string RestorePassCode { get; set; }
    }
}
