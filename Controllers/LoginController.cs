using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BigBang2.Models;

namespace RealEstate2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HospitalContext _context;

        private const string DoctorRole = "Doctor";
        private const string AdminRole = "Admin";
        private const string PatientRole = "Patient";

        public LoginController(IConfiguration configuration, HospitalContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Doctor")]
        public async Task<IActionResult> Post(Doctor _userData)
        {
            if (_userData != null && !string.IsNullOrEmpty(_userData.DocEmail) && !string.IsNullOrEmpty(_userData.DocPas))
            {
                var user = await GetUser(_userData.DocEmail, _userData.DocPas);

                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("DocId", user.DocId.ToString()),
                        new Claim("DocEmail", user.DocEmail),
                        new Claim(ClaimTypes.Role, DoctorRole)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(29),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("Admin")]
        public async Task<IActionResult> PostAdmin(Admin adminData)
        {
            if (adminData != null && !string.IsNullOrEmpty(adminData.AdminName) && !string.IsNullOrEmpty(adminData.AdminPassword))
            {
                var admin = await GetAdmin(adminData.AdminName, adminData.AdminPassword);

                if (admin != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("AdminName", admin.AdminName),
                        new Claim("AdminPassword", admin.AdminPassword),
                        new Claim(ClaimTypes.Role,AdminRole)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(5),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }




        [HttpPost("Patient")]
        public async Task<IActionResult> PostPatient(Patient adminData)
        {
            if (adminData != null && !string.IsNullOrEmpty(adminData.PatientEmail) && !string.IsNullOrEmpty(adminData.PatientPass))
            {
                var adminn = await GetPatient(adminData.PatientEmail, adminData.PatientPass);

                if (adminn != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("PatientEmail", adminn.PatientEmail),
                        new Claim("PatientPass", adminn.PatientPass),
                        new Claim(ClaimTypes.Role, PatientRole)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(95),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }






        private async Task<Doctor> GetUser(string email, string password)
        {
            return await _context.Doctors.FirstOrDefaultAsync(u => u.DocEmail == email && u.DocPas == password);
        }

        private async Task<Admin> GetAdmin(string email, string password)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.AdminName == email && a.AdminPassword == password);
        }

        private async Task<Patient> GetPatient(string email, string password)
        {
            return await _context.Patients.FirstOrDefaultAsync(a => a.PatientEmail == email && a.PatientPass == password);
        }
    }
}