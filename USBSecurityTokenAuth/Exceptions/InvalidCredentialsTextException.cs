namespace USBSecurityTokenAuth.Exceptions;

public class InvalidCredentialsTextException : Exception
{
    public InvalidCredentialsTextException(string message) : base(message)
    {
    }
}