using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

public class BuildOrderStep : BaseEntity
{
    [Key]
    public int BuildOrderStepId { get; set; }

    [Required]
    public int GameDataId { get; set; }
    
    [Required]
    public int WorkerCount { get; set; }

    public int BuildOrderListId { get; set; }
    public BuildOrderList BuildOrderList { get; set; }
}

public class BuildOrderStepDto
{
    public int? GameDataId { get; set; }
    public int? WorkerCount { get; set; }
}