using AlumniTrackerSite.Contexts;
using AlumniTrackerSite.Models;

namespace AlumniTrackerSite.Services
{
    public class DataService :IDataService
    {
        private readonly TrackerContext _context;

        public DataService(TrackerContext context)
        {
            _context = context;
        }

        //public List<AlumniUser> GetUser(string id)
        //{
        //    var user = _context.AlumniUser
        //        .Where(x => x.Id == id);
        //}
    }
}
