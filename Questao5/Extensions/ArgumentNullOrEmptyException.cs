namespace Questao5.Extensions;

public class ArgumentNullOrEmptyException : ArgumentException
{
    public ArgumentNullOrEmptyException(string paramName)
            : base($"Argument cannot be null or empty.", paramName)
    { }
}
