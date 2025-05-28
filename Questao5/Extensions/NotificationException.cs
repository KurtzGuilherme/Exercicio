namespace Questao5.Extensions;

public class NotificationException : Exception
{
    public string Code { get; }

    public NotificationException(string msg, string code)
            : base(msg)
    { 
        Code = code;
    }
   
}
