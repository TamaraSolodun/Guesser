using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer_Guesser.Models
{
    public enum GameResultType
    {
        Lose,
        Win
    }
    [Table("Game")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public GameResultType GameResultType { get; set; }
        [Required]
        public int PlayerId { get; set; }
        public DateTime PlayedAt { get; set; }

        public virtual User Player { get; set; }
    
    }
}
