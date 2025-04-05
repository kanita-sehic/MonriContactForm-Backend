namespace MonriContactForm.Core.Exceptions;

public class SendingEmailException : Exception
{
    public SendingEmailException(string message)
        : base(message)
    {
    }

    public SendingEmailException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
