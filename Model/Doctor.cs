
using System.ComponentModel.DataAnnotations;



namespace BigBang2.Models
{
    public class Doctor
    {
        [Key]
        public int? DocId { get; set; }
        public string? DocName { get; set; }
        public string? DocSpecialty { get; set; }
        public string? DocEmail { get; set; }

        public string? DocPas { get; set; }
        public bool? DocActive { get; set; }
        public byte[]? DocImg { get; set; }
        public virtual ICollection<Patient>? Patients { get; set; }
    }
}
