using BigBang2.Interface;
using BigBang2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBang2.Repository
{
    public class AdminRepo : IAdmin
    {
        private readonly HospitalContext cont;

        public AdminRepo(HospitalContext context)
        {
            cont = context;
        }

        public async Task<IEnumerable<Admin>> GetAdmins()
        {
            return await cont.Admins.ToListAsync();
        }

        public async Task<Admin> GetAdminById(int id)
        {
            var admin = await cont.Admins.FindAsync(id);
            return admin;
        }

        public async Task<Admin> PostAdmin(Admin admin)
        {
            cont.Admins.Add(admin);
            await cont.SaveChangesAsync();
            return admin;
        }

        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await cont.Admins.FindAsync(id);
            if (admin == null)
                return new NotFoundResult();

            cont.Admins.Remove(admin);
            await cont.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
