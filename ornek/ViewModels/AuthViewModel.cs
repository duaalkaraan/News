namespace ornek.Dtos
{
    public class AuthViewModel
    {
        public LoginDto Login { get; set; } = new();
        public RegisterDto Register { get; set; } = new();
    }
}