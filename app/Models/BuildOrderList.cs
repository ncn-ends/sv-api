using System.ComponentModel.DataAnnotations;

namespace app.Models;

// https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
public class BuildOrderList : BaseEntity
{
    [Key]
    public int BuildOrderListId { get; set; }

    [Required]
    public string Title { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    [Required]
    public string? Faction { get; set; }

    public string? Difficulty { get; set; }

    public string? UserProfileId { get; set; }

    public UserProfile? UserProfile { get; set; }

    public List<Tag> Tags { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<BuildOrderStep> BuildOrderSteps { get; set; } = new();
}

public class BuildOrderListDto
{
    public int? BuildOrderListId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } = String.Empty;
    public string Faction { get; set; }
    public string Difficulty { get; set; }
    
    public List<TagDto> Tags { get; set; } = new();
    public List<RatingDto> Ratings { get; set; } = new();
    public List<CommentDto> Comments { get; set; } = new();
    
    public List<BuildOrderStepDto> BuildOrderSteps { get; set; } = new();
}