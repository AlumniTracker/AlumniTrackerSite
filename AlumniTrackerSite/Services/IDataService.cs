using AlumniTrackerSite.Models;

namespace AlumniTrackerSite.Services
{
    public interface IDataService
    {
        AlumniUser GetUser(string id);
    }
}
