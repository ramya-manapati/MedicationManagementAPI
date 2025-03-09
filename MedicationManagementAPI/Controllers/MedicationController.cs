using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Security.Claims;
using MedicationManagementAPI.Data;
using MedicationManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MedicationManagementAPI.Interfaces;

namespace MedicationManagementAPI.Controllers
{
    [Route("api/medications")]
    [ApiController]
    // [Authorize] 
    public class MedicationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        public MedicationController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // Add a new medication for logged-in user
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMedication([FromBody] Medication med)
        {
            try
            {
                var userId = _userService.GetUserIdFromClaims(User);

                if (userId == 0) 
                {
                    return Unauthorized(new { message = "User not found" });
                }
                med.UserId = userId;
                med.User = null; 
                await _context.Medications.AddAsync(med);
                await _context.SaveChangesAsync();  

                return Ok(new { message = "Medication added successfully", medication = med });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding medication", error = ex.Message });
            }
        }

        // Get all medications for logged-in user
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMedications([FromQuery] DateTime? afterDate, [FromQuery] string? search)
        {
            try
            {
                
                var userId = _userService.GetUserIdFromClaims(User);

                if (userId == 0)  
                {
                    return Unauthorized(new { message = "User not found" });
                }
                var meds = _context.Medications.Where(m => m.UserId == userId);
                if (afterDate.HasValue)
                {
                    meds = meds.Where(m => m.DateOfIssue > afterDate.Value);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    meds = meds.Where(m => m.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                var medicationList = await meds.ToListAsync();
                return Ok(medicationList);
            }
            catch (Exception ex)
            {
               return StatusCode(500, new { message = "An error occurred while fetching medications", error = ex.Message });
            }
        }


        //  Update medications 
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMedication(int id, [FromBody] Medication med)
        {
            try
            {
                var existingMed = _context.Medications.Find(id);
                if (existingMed == null) return NotFound();

                existingMed.Description = med.Description;
                existingMed.Dosage = med.Dosage;
                existingMed.Frequency = med.Frequency;
                existingMed.Duration = med.Duration;
                existingMed.Reason = med.Reason;
                existingMed.DateOfIssue = med.DateOfIssue;
                existingMed.Instructions = med.Instructions;

                _context.SaveChanges();
                return Ok(new { message = "Medication updated successfully", medication = existingMed });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating medication", error = ex.Message });
            }
        }

        //  Delete medications 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMedication(int id)
        {
            try
            {
                var med = _context.Medications.Find(id);
                if (med == null) return NotFound();
                _context.Medications.Remove(med);
                _context.SaveChanges();
                return Ok(new { message = "Medication deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting medication", error = ex.Message });
            }
        }
    }
}

