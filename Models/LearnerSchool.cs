namespace VERAExample.Models
{
    public class LearnerSchool
    {
        public string name { get; set; }
        public int urn { get; set; }
        public string academicYearStart { get; set; }
        public int? termStart { get; set; }
        public string academicYearEnd { get; set; }
        public int? termEnd { get; set; }
        public int displaySort { get; set; }
    }
}
