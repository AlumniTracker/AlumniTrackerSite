using AlumniTrackerSite.Contexts;
using AlumniTrackerSite.Models;

namespace AlumniTrackerSite.Services
{
    public class DataService : IDataService
    {
        private readonly TrackerContext _context;
        private readonly AlumniUser _user;

        public DataService(TrackerContext context)
        {
            _context = context;
        }

        public AlumniUser GetUser(string id)
        {
            var user = (AlumniUser)_context.AlumniUsers
                .Where(x => x.Id == id);
            
            return user;
        }
    }
}
