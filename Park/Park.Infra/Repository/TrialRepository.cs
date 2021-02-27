using Microsoft.EntityFrameworkCore;
using Park.Core.Interfaces;
using Park.Core.Models;
using Park.Infra.Data;
using System.Collections.Generic;
using System.Linq;

namespace Park.Infra.Repository
{
    public class TrialRepository : ITrialRepository
    {
        private readonly ApplicationDbContext _context;
        public TrialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<Trail> GetTrails()
        {
            return _context.Trails.Include(c => c.NationalPark).OrderBy(a => a.Name).ToList();
        }
        public Trail GetTrail(int trailId)
        {
            return _context.Trails.Include(c => c.NationalPark).FirstOrDefault(a => a.Id == trailId);
        }
        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _context.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npId).ToList();
        }
        public bool CreateTrail(Trail trail)
        {
            _context.Trails.Add(trail);
            return Save();
        }
        public bool TrailExists(string name)
        {
            bool value = _context.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _context.Trails.Any(a => a.Id == id);
        }

        public bool UpdateTrail(Trail trail)
        {
            _context.Trails.Update(trail);
            return Save();
        }
        public bool DeleteTrail(Trail trail)
        {
            _context.Trails.Remove(trail);
            return Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }
       
    }
}
