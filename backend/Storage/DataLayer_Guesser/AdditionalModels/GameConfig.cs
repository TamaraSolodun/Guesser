using DataLayer_Guesser.Models;
using System;

namespace DataLayer_Guesser.AdditionalModels
{
    public class GameConfigs : BaseFilter
    {
        public GameResultType? ResultType { get; set; }
        public DateTime? PlayedDateFrom { get; set; }
        public DateTime? PlayedDateTo { get; set; }
        public int? PlayerId { get; set; }
    }
}
