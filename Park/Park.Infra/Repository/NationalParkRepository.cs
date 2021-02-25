using Park.Core.Interfaces;
using Park.Core.Models;
using Park.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Park.Infra.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _context;

        public NationalParkRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICollection<NationalPark> GetNationalParks()
        {
            return _context.NationalParks.OrderBy(n => n.Name).ToList();
        }
        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _context.NationalParks.FirstOrDefault(n=>n.Id == nationalParkId);
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _context.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _context.NationalParks.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            return _context.NationalParks.Any(a=>a.Id == id);
        }
        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _context.NationalParks.Update(nationalPark);
            return Save();
        }
        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _context.NationalParks.Remove(nationalPark);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }
    }
}
