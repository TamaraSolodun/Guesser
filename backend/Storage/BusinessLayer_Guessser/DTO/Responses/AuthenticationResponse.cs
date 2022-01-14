namespace BusinessLayer_Guesser.DTO
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public UserResponse UserInfo { get; set; }
    }
}
