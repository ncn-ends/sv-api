namespace app.Models;

// export const initialState: SearchState = {
// text: "",
// isLoading: false,
// factions: { protoss: "active", zerg: "active", terran: "active" },
// difficulty: { beginner: "active", intermediate: "active", expert: "active" },
// tags: []
// }

public class FilteredBuildsFaction
{
    public bool Protoss { get; set; } = true;
    public bool Zerg { get; set; } = true;
    public bool Terran { get; set; } = true;
}

public class FilteredBuildsDifficulty
{
    public bool Beginner { get; set; } = true;
    public bool Intermediate { get; set; } = true;
    public bool Expert { get; set; } = true;
}

public class FilteredBuildsQuery
{
    public int Page { get; set; } = 1;
    public string Text { get; set; } = "";
    public bool Protoss { get; set; } = true;
    public bool Zerg { get; set; } = true;
    public bool Terran { get; set; } = true;
    public bool Beginner { get; set; } = true;
    public bool Intermediate { get; set; } = true;
    public bool Expert { get; set; } = true;
    public string[] Tags { get; set; } = Array.Empty<string>();
}