namespace Questao5.Extensions;

public static class NormalizarException
{
    public static string NormalizarGuidToString(this Guid guid)
    {
        if(guid == Guid.Empty)
            return string.Empty;   

        return guid.ToString().Replace(" ", "").ToUpper();
    }
    
}
