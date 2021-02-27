using Park.Core.Models;
using System.Collections.Generic;

namespace Park.Core.Interfaces
{
    public interface ITrialRepository
    {
        ICollection<Trail> GetTrails();
        Trail GetTrail(int trailId);
        ICollection<Trail> GetTrailsInNationalPark(int npId);
        bool CreateTrail(Trail trail);
        bool TrailExists(string name);
        bool TrailExists(int id);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(Trail trail);
        bool Save();
    }
}
