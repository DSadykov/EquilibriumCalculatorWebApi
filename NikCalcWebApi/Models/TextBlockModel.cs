using System.ComponentModel.DataAnnotations.Schema;

namespace NikCalcWebApi.Models;

[Table("Texts")]
public class TextBlockModel
{
    public int Id { get; set; }

    [ForeignKey("Tabs")]
    public int TabId { get; set; }

    [ForeignKey("Languages")]
    public int LanguageId { get; set; }
    public LanguageModel Language { get; set; }
    public TabModel Tab { get; set; }
    public string Text { get; set; }
    public int Position { get; set; }
}
