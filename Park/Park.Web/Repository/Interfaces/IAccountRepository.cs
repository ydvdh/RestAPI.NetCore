using Park.Web.Models;
using System.Threading.Tasks;

namespace Park.Web.Repository.Interfaces
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User objToCreate);
        Task<bool> RegisterAsync(string url, User objToCreate);
    }
}
