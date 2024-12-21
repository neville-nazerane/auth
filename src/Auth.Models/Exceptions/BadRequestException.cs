namespace Auth.WebAPI.Exceptions
{
    public class BadRequestException(IEnumerable<string> messages) : Exception
    {
        public IEnumerable<string> Messages { get; } = messages;

    }
}
