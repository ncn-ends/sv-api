using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using Newtonsoft.Json;

namespace app.Models;

public class RegisteringUserDto
{
    public string user_id { get; set; }
}

public class TokenUser
{        
    /*  Create a unique id from the letter of each part of the user_id as well as the numbers.
     *      e.g. oauth|discord|914771371647725619 -> od914771371647725619
     */
    public static string ParseUserId(string userId)
    {
        var providers = userId.Split('|');
        var apiUserId = "";
        foreach (var part in providers)
        {
            if (part.Length > 0 && int.TryParse(part[0].ToString(), out _))
            {
                apiUserId += part;
            }
            else
            {
                apiUserId += part[0];
            }
        }
    
        return apiUserId;
    }
    public string? email { get; set; }

    public DateTime? created_at { get; set; }

    public bool? email_verified { get; set; }

    public string? family_name { get; set; }

    public string? given_name { get; set; }

    public string? name { get; set; }

    public string? nickname { get; set; }

    public string? phone_number { get; set; }

    public string? phone_verified { get; set; }

    public string? user_metadata { get; set; }

    public string? username { get; set; }

    public string? profile_id { get; set; }

    public TokenUser(string? user_id)
    {
        profile_id = ParseUserId(user_id);
    }
}