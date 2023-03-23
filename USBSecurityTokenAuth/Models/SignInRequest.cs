namespace USBSecurityTokenAuth.Models;

public class SignInRequest
{
    public string Login { get; set; }
    
    public string Hash { get; set; }
}