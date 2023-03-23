using System.Text.RegularExpressions;
using USBSecurityTokenAuth.Exceptions;

namespace USBSecurityTokenAuth.Models;

public class LoginAndPassword
{
    private const string CredentialsRegxPattern = @"(.*)\n(.*)";
    
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public static LoginAndPassword ExtractFromText(string credentialsText)
    {
        var rgx = new Regex(CredentialsRegxPattern);
        var match = rgx.Match(credentialsText);
        var groups = match.Groups;

        if (groups.Count != 3)
            throw new InvalidCredentialsTextException("Текст з даними для входу не є коректним!");
        
        return new LoginAndPassword { Login = groups[1].Value, Password = groups[2].Value };
    }
}