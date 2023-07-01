
using BigBang2.Interface;
using BigBang2.Models;
using BigBang2.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatient _patientRepository;

    public PatientController(IPatient patientRepository)
    {
        _patientRepository = patientRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
    {
        var patients = await _patientRepository.GetPatients();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        var patient = await _patientRepository.GetPatientById(id);

        if (patient == null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPatient(int id, Patient patient)
    {
        if (id != patient.PatientId)
        {
            return BadRequest();
        }

        try
        {
            await _patientRepository.PutPatient(id, patient);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _patientRepository.Exists(id))
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

    [HttpPost]
    public async Task<ActionResult<Patient>> PostPatient(IFormFile imageFile, [FromForm] Patient patient)
    {
        if (imageFile == null || imageFile.Length <= 0)
        {
            return BadRequest("Image file is required.");
        }

        var imageData = await ConvertImageToByteArray(imageFile);
        patient.PatientImg = imageData;

        var createdPatient = await _patientRepository.PostPatient(patient);

        return createdPatient.Result;
    }

    private async Task<byte[]> ConvertImageToByteArray(IFormFile imageFile)
    {
        using (var memoryStream = new MemoryStream())
        {
            await imageFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _patientRepository.GetPatientById(id);
        if (patient == null)
        {
            return NotFound();
        }

        await _patientRepository.DeletePatient(id);

        return NoContent();
    }
}