using Park.Core.Models;
using System.Collections.Generic;

namespace Park.Core.Interfaces
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int nationalParkId);

        bool CreateNationalPark(NationalPark nationalPark);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(NationalPark nationalPark);
        bool Save();
    }
}
