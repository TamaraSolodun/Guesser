using DataLayer_Guesser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLayer_Guesser.DTO.Requests
{
    public class SetUserStatusRequest
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public UserStatus? UserStatus { get; set; }
        public string TokenToActivate { get; set; }
    }
}
