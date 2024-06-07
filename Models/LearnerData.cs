namespace VERAExample.Models
{
    public class LearnerData
    {
        public int uln { get; set; }
        public string middleNames { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string postcode { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set;}
        public string addressLine3 { get; set; }
        public string addressLine4 { get; set; }
        public string addressLine5 { get; set;}
        public string senProvision { get; set; }
        public string senProvisionDescription { get; set; }
        public string primarySENType { get; set; }
        public string primarySENTypeDescription { get; set; }
        public string secondarySENType { get; set;}
        public string secondarySENTypeDescription { get; set; }
        public int freeSchoolMealEligible {  get; set; }
        public FreeSchoolMealEligibleLatest freeSchoolMealEligibleLatest { get; set; }
        public IEnumerable<LearnerQualification> qualifications { get; set; }
        public IEnumerable<LearnerSchool> schools { get; set; }

    }
}

