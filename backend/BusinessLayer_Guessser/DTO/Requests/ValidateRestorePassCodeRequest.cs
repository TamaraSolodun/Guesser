using System.ComponentModel.DataAnnotations;

namespace BusinessLayer_Guesser.DTO.Requests
{
    public class ValidateRestorePassCodeRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string RestorePassCode { get; set; }
    }
}
