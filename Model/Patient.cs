using System.ComponentModel.DataAnnotations;

namespace BigBang2.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        public string? PatientName { get; set; }

        public int? PatientAge { get; set; }

        public string? PatientGender { get; set; }
        public string? PatientDescription { get; set; }

        public string PatientEmail { get; set; }
        public string PatientPass { get; set; }

        public byte[]? PatientImg { get; set; }

        public virtual ICollection<Doctor>? Doctors { get; set; }
    }
}
