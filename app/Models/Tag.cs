using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

public class Tag
{
    [Key]
    public int TagId { get; set; }

    [Required]
    public string Label { get; set; }

    public int BuildOrderListId { get; set; }
    public BuildOrderList BuildOrderList { get; set; }
}

public class TagDto
{
    public string Label { get; set; }
}