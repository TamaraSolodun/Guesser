using System.ComponentModel.DataAnnotations;

namespace BusinessLayer_Guesser.DTO.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        public int? UserId { get; set; }
        [Required]
        public string OldPass { get; set; }
        [Required]
        public string NewPass { get; set; }
    }
}
