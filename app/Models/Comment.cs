using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

public class CommentEndpointBody
{
    public string? body { get; set; }
    public int? listId { get; set; }
    public int? commentId { get; set; }
}

public class Comment : BaseEntity
{
    [Key]
    public int CommentId { get; set; }

    [Required]
    public string Body { get; set; }

    public int? BuildOrderListId { get; set; }
    public BuildOrderList? BuildOrderList { get; set; }

    public string? UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}

public class CommentDto
{
    public int? CommentId { get; set; }
    public string Body { get; set; }
}