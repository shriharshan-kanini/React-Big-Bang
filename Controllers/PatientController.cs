
using BigBang2.Interface;
using BigBang2.Models;
using BigBang2.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

    public async Task<IActionResult> PutPatient(int id, [FromForm] Patient patient, IFormFile? imageFile)
    {
        if (id != patient.PatientId)
        {
            return BadRequest();
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            var imageData = await ConvertImageToByteArray(imageFile);
            patient.PatientImg = imageData;
        }
        else
        {
            patient.PatientImg = null;
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
    public async Task<ActionResult<Patient>> PostPatient([FromForm] Patient patient, IFormFile? imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            var imageData = await ConvertImageToByteArray(imageFile);
            patient.PatientImg = imageData;
        }

        var createdPatient = await _patientRepository.PostPatient(patient);

        return createdPatient;
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
