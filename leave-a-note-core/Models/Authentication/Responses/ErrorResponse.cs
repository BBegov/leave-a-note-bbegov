namespace leave_a_note_core.Models.Authentication.Responses;

public class ErrorResponse
{
    public IEnumerable<string> ErrorMessages { get; set; }

    public ErrorResponse(string errorMessage)
    {
        ErrorMessages = new List<string> { errorMessage };
    }

    public ErrorResponse(IEnumerable<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
