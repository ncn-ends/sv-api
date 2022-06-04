using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace app.Models;

public class Rating : BaseEntity
{
    [Key]
    public int RatingId { get; set; }

    [Required]
    public bool Value { get; set; }

    public int BuildOrderListId { get; set; }
    public BuildOrderList BuildOrderList { get; set; }

    public string? UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}

public class RatingDto
{
    public int? RatingId { get; set; }
    public bool Value { get; set; }
}
