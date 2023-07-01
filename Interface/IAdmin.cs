

using BigBang2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBang2.Interface
{
    public interface IAdmin
    {
        Task<IEnumerable<Admin>> GetAdmins();
        Task<Admin> GetAdminById(int id);
        Task<Admin> PostAdmin(Admin admin);
        Task<IActionResult> DeleteAdmin(int id);

    }
}