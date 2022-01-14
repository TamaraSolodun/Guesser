using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer_Guesser.AdditionalModels
{
    public class BaseFilter
    {
        public int CurrentPage { get; set; }
        public int ItemsOnPage { get; set; }
        /// <summary>
        /// True - ascending
        /// False - descending
        /// Null - sorting will not be applied
        /// </summary>
        public bool? SortingDirection { get; set; }
    }
}
