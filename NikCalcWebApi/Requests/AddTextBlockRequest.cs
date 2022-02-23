namespace NikCalcWebApi.Requests;

public class AddTextBlockRequest
{
    public List<TextBlock> Texts { get; set; }
    public string Tab { get; set; }
}
public class TextBlock
{
    public string Language { get; set; }
    public string Text { get; set; }
}