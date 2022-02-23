using System.ComponentModel.DataAnnotations.Schema;

namespace NikCalcWebApi.Models;

[Table("Tabs")]
public class TabModel
{
    public int Id { get; set; }
    public string TabName { get; set; }
    public List<TextBlockModel>? TextBlockModels { get; set; }
}
