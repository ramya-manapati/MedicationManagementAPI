using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Security.Claims;
using MedicationManagementAPI.Data;
using MedicationManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicationManagementAPI.Controllers
{
    [Route("api/medications")]
    [ApiController]
    [Authorize] 
    public class MedicationController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MedicationController(AppDbContext context)
        {
            _context = context;
        }

        // Get logged-in user's ID from JWT token
        private int? GetLoggedInUserId()
        {
            try
            {
                Console.WriteLine("Extracting User ID from Token...");

                // Log all claims for debugging
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }

                // Try to get User ID from JWT token 
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    Console.WriteLine($"User ID from Token: {userId}");
                    return userId;
                }

                // If User ID is missing, try to get Email instead
                var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(emailClaim))
                {
                    Console.WriteLine($"Email from Token: {emailClaim}");
                    var user = _context.Users.FirstOrDefault(u => u.Email == emailClaim);
                    return user?.Id;
                }

                Console.WriteLine("No User ID or Email found in token.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting user ID: {ex.Message}");
                return null;
            }
        }



        // Add a new medication for logged-in user
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddMedication([FromBody] Medication med)
        {
            try
            {
                var userId = GetLoggedInUserId();
                if (userId == null) return Unauthorized(new { message = "User not found" });

                // Assign the logged-in user's ID
                med.UserId = userId.Value;
                med.User = null;

                _context.Medications.Add(med);
                _context.SaveChanges();

                return Ok(new { message = "Medication added successfully", medication = med });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding medication", error = ex.Message });
            }
        }

        // Get all medications for logged-in user
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMedications([FromQuery] DateTime? afterDate, [FromQuery] string? search)
        {
            try
            {
                var userId = GetLoggedInUserId();
                if (userId == null) return Unauthorized(new { message = "User not found" });

                var meds = _context.Medications.Where(m => m.UserId == userId.Value);

                if (afterDate.HasValue)
                    meds = meds.Where(m => m.DateOfIssue > afterDate.Value);

                if (!string.IsNullOrEmpty(search))
                    meds = meds.Where(m => m.Description.Contains(search));

                return Ok(meds.ToList());
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

