namespace USBSecurityTokenAuth.Models;

public class NewUser
{
    public string Login { get; set; }
    
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string Hash { get; set; }
}