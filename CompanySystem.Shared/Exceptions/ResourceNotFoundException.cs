namespace CompanySystem.Shared.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string message)
        : base(message)
    {
    }

    public ResourceNotFoundException(string resource, object id)
        : base($"{resource} not found with ID: {id}.")
    {
    }
}