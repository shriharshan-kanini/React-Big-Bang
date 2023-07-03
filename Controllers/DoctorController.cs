
// DoctorController.cs
using BigBang2.Models;
using BigBang2.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BigBang2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctor<Doctor> _doctorRepository;

        public DoctorController(IDoctor<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            var doctors = await _doctorRepository.GetAll();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _doctorRepository.GetById(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpGet("search/specialty/{specialty}")]
        public async Task<IActionResult> SearchDoctorsBySpecialty(string specialty)
        {
            try
            {
                var doctors = await _doctorRepository.GetDoctorsBySpecialty(specialty);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT: api/Doctor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromForm] Doctor doctor, IFormFile? imageFile)
        {
            if (id != doctor.DocId)
            {
                return BadRequest();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var imageData = await ConvertImageToByteArray(imageFile);
                doctor.DocImg = imageData;
            }

            else
            {
                doctor.DocImg = null;
            }

            try
            {
                await _doctorRepository.Update(doctor);
            }
            catch
            {
                if (!await _doctorRepository.Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private async Task<byte[]> ConvertImageToByteArray(IFormFile imageFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostDoctor([FromForm] Doctor doctor, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imageData = await ConvertImageToByteArray(imageFile);
                doctor.DocImg = imageData;
            }

            doctor.Status = "Pending";

            await _doctorRepository.Add(doctor);

            // Send approval request to the admin

            return CreatedAtAction("GetDoctor", new { id = doctor.DocId }, doctor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _doctorRepository.GetById(id);
            if (doctor == null)
            {
                return NotFound();
            }

            await _doctorRepository.Delete(doctor);

            return NoContent();
        }

        // GET: api/Doctor/ApprovedDoctors
        [HttpGet("ApprovedDoctors")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetApprovedDoctors()
        {
            var approvedDoctors = await _doctorRepository.GetAll();
            var filteredDoctors = approvedDoctors.Where(d => d.Status == "Approved");
            return Ok(filteredDoctors);
        }

    }
}
