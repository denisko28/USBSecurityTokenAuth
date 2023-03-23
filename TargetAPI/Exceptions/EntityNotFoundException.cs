namespace TargetAPI.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, string key) : 
        base($"Entity with {entityName}: '{key}' was not found!")
    {
    }
}