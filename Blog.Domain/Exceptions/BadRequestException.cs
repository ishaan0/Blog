namespace Blog.Domain.Exceptions
{
    public class BadRequestException(string message) : CustomException(message)
    {
        public override string Title => "Bad Request";
    }
}
