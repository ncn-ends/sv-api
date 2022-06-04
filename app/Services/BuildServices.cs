using System.Diagnostics;
using System.Security.Authentication;
using app.Configs;
using app.Models;
using Mapster;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace app.Services;

public class BuildServices
{
    private readonly BuildDbContext _dbContext;
    private readonly HashSet<string> _acceptableFactions = new() {"Protoss", "Terran", "Zerg"};
    private readonly HashSet<string> _acceptableDifficulties = new() {"Beginner", "Intermediate", "Expert"};
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BuildServices(BuildDbContext dbContext, IHttpContextAccessor httpContextAccessorAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessorAccessor;
    }

    public async Task<BuildOrderListDto> Add(BuildOrderListDto buildOrderList)
    {
        if (buildOrderList.Faction != null && !_acceptableFactions.Contains(buildOrderList.Faction))
        {
            throw new ArgumentException("Invalid faction name.");
        }

        if (buildOrderList.Difficulty != null && !_acceptableDifficulties.Contains(buildOrderList.Difficulty))
        {
            throw new ArgumentException("Invalid difficulty tag.");
        }

        foreach (var tag in buildOrderList.Tags)
        {
            tag.Label = tag.Label.ToLower();
        }

        var user = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser ??
                   throw new AuthenticationException();
        var userProfile = _dbContext.UserProfiles.FirstOrDefault(u => u.UserProfileId == user.profile_id);

        if (userProfile == null)
        {
            throw new AuthenticationException("User has not been synced with internal API.");
        }

        var build = buildOrderList.Adapt<BuildOrderList>();

        var addedBuild = _dbContext.BuildOrderLists.Add(build);
        userProfile.BuildOrderLists.Add(build);
        await _dbContext.SaveChangesAsync();

        buildOrderList.BuildOrderListId = addedBuild.Entity.BuildOrderListId;
        return buildOrderList;
    }

    public IEnumerable<BuildOrderList> GetAll()
    {
        return _dbContext.BuildOrderLists
            .Include(d => d.Tags)
            .Include(b => b.Ratings)
            .ToList();
    }


    public BuildOrderListDto GetById(int id)
    {
        var build = _dbContext.BuildOrderLists
            .Include(d => d.Tags)
            .Include(d => d.Ratings)
            .Include(d => d.Comments)
            .Include(d => d.UserProfile)
            .Include(d => d.BuildOrderSteps)
            .FirstOrDefault(b => b.BuildOrderListId == id);

        if (build == null) throw new ArgumentException();
        return build.Adapt<BuildOrderListDto>();
    }

    // private Func<BuildOrderList, FilteredBuildsQuery, bool> validateFields = (list, query) =>
    // {
    //     /* filter out if query text doesnt exist in description or title */
    //     return list.Title.Contains(query.Text) || list.Description.Contains(query.Text);
    // };

    public async Task<FilteredBuildsResponse> GetFiltered(FilteredBuildsQuery query)
    {
        var builds = _dbContext.BuildOrderLists
            .Include(x => x.Tags)
            .Include(x => x.UserProfile)
            .Include(x => x.UserProfile)
            .Include(x => x.BuildOrderSteps)
            /* search text filtering */
            .Where(list => list.Title.Contains(query.Text) ||
                           list.Description.Contains(query.Text))
            /* faction filtering */
            .Where(list => query.Protoss ? list.Faction == "Protoss" : list.Faction != "Protoss")
            .Where(list => query.Zerg ? list.Faction == "Zerg" : list.Faction != "Zerg")
            .Where(list => query.Terran ? list.Faction == "Terran" : list.Faction != "Terran")
            /* difficulty filtering */
            .Where(list => query.Beginner ? list.Difficulty == "Beginner" : list.Difficulty != "Beginner")
            .Where(list =>
                query.Intermediate ? list.Difficulty == "Intermediate" : list.Difficulty != "Intermediate")
            .Where(list => query.Expert ? list.Difficulty == "Expert" : list.Difficulty != "Expert");

            // TODO: implement tag filtering
            
        var totalCount = builds.Count();

        /* current count should be 25 unless maximum count is less than 25 */
        var currentCount = totalCount > 25 ? 25 : totalCount;

        /* Default to last page if page requested surpasses total count */
        int pageNumber;
        bool isLastPage;
        if (query.Page * 25 > totalCount)
        {
            pageNumber = (int) Math.Ceiling(totalCount / 25m);
            isLastPage = true;
        }
        else
        {
            pageNumber = query.Page;
            isLastPage = false;
        }

        var responseObject = new FilteredBuildsResponse
        {
            CurrentCount = currentCount,
            IsLastPage = isLastPage,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            List = new List<BuildOrderList>(builds)
        };
        return responseObject;
    }
}