namespace AssecorAssessment.Models
{
    public class ValidationResult
    {
        public bool Successful { get; set; }

        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}