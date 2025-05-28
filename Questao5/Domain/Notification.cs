namespace Questao5.Domain;

public class Notification
{
    public string Mensagem { get; }
    public string Tipo { get; }

    public Notification(string mensagem, string tipo)
    {
        Mensagem = mensagem;
        Tipo = tipo;
    }
}

