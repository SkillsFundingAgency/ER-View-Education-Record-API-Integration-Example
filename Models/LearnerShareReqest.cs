using System.ComponentModel.DataAnnotations;

namespace VERAExample.Models
{
    public class LearnerShareRequest
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        public string MobileNumber { get; set; }
    }
}
