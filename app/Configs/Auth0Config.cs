namespace app.Configs;

public class Auth0Config
{    
    public static Auth0Config? Current;

    public Auth0Config()
    {
        Current = this;
    }
    public string? Authority { get; set; }
    public string? Audience { get; set; }
}
public class Auth0Settings
{
    public string Authority { get; set; }
    public string Audience { get; set; }
    public string ApiSecret { get; set; }
}