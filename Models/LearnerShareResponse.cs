namespace VERAExample.Models
{
    public class LearnerShareResponse
    {
        public string CorrelationId { get; set; }

        public string Code { get; set; }

        public LearnerData? LearnerData { get; set; }
    }
}
