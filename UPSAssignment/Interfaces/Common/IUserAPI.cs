using System.Threading.Tasks;
using UPSAssignment.Common;
using UPSAssignment.Models;

namespace UPSAssignment.Interfaces.Common
{
    public interface IUserAPI
    {
        Task<UserData> GetUsers(string searchName = "", int pageNumber = 1);
        Task<int> AddUpdateUser(Operations operationType, object jsonUserObject, int id = 0);
        Task<int> DeleteUser(int id);
    }
}
