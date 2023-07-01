using BigBang2.Models;
using BigBang2.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigBang2.Repoitory
{
    public class PatientRepo : IPatient
    {
        private readonly HospitalContext _context;

        public PatientRepo(HospitalContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.Include(x => x.Doctors).ToListAsync();
        }

        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            return patient;
        }

        public async Task<ActionResult<Patient>> PostPatient(Patient pt)
        {
            _context.Patients.Add(pt);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult("GetPatient", "Patient", new { id = pt.PatientId }, pt);
        }

        public async Task<IActionResult> PutPatient(int id, Patient pt)
        {
            _context.Entry(pt).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public async Task<IActionResult> DeletePatient(int id)
        {
            var pt = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(pt);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Patients.AnyAsync(e => e.PatientId == id);
        }

    }

}