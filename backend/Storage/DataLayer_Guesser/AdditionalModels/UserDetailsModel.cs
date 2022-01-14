using System;

namespace DataLayer_Guesser.AdditionalModels
{
    public class UserDetailsModel
    {
        public string ActivationCode { get; set; }
        public int? ActivationMessagePerDayCounter { get; set; }
        public DateTime LastActivationMessageDate { get; set; }
        public DateTime ActivationCodeExpire { get; set; }
        public string RestorePasswordCode { get; set; }
        public int? PasswordRestoringMessagePerDayCounter { get; set; }
        public DateTime LastPasswordRestoringMessageDate { get; set; }
        public DateTime RestorePasswordCodeExpire { get; set; }
    }
}
