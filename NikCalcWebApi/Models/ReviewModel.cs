using System.ComponentModel.DataAnnotations.Schema;

namespace NikCalcWebApi.Models;

[Table("Reviews")]
public class ReviewModel
{
    public int Id { get; set; }
    public string Review { get; set; }
}