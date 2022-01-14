using DataLayer_Guesser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer_Guesser.DTO.Responses
{
    public class GameResponse
    {
        public int Id { get; set; }
        public GameResultType GameResultType { get; set; }
        public int PlayerId { get; set; }
        public DateTime PlayedAt { get; set; }

        public virtual UserResponse Player { get; set; }
    }
}
