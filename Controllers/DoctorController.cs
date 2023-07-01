
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

        [HttpGet("search/active/{isActive}")]
        public async Task<IActionResult> SearchDoctorsByActiveStatus(bool isActive)
        {
            try
            {
                var doctors = await _doctorRepository.GetDoctorsByActiveStatus(isActive);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
        public async Task<IActionResult> UpdateDoctor(int id, [FromForm] Doctor doctor, IFormFile imageFile)
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
        public async Task<ActionResult<Doctor>> PostDoctor(IFormFile imageFile, [FromForm] Doctor doctor)
        {
            if (imageFile == null || imageFile.Length <= 0)
            {
                return BadRequest("Image file is required.");
            }

            var imageData = await ConvertImageToByteArray(imageFile);
            doctor.DocImg = imageData;

            await _doctorRepository.Add(doctor);

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
    }
}
