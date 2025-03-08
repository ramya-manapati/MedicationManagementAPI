using MedicationManagementAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MedicationManagementAPI.Models
{
    public class Medication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User? User { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Dosage is required.")]
        [StringLength(50, ErrorMessage = "Dosage cannot exceed 50 characters.")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Frequency is required.")]
        [StringLength(50, ErrorMessage = "Frequency cannot exceed 50 characters.")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days.")]
        public int Duration { get; set; }

        [StringLength(200, ErrorMessage = "Reason cannot exceed 200 characters.")]
        public string? Reason { get; set; }

        [Required(ErrorMessage = "Date of Issue is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfIssue { get; set; }

        [StringLength(300, ErrorMessage = "Instructions cannot exceed 300 characters.")]
        public string? Instructions { get; set; }
    }
}
