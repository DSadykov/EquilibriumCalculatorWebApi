using System.ComponentModel.DataAnnotations.Schema;

namespace NikCalcWebApi.Models;

[Table("Languages")]
public class LanguageModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TextBlockModel>? TextBlockModels { get; set; }
}
