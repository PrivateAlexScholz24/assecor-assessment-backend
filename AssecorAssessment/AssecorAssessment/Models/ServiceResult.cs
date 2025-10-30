namespace AssecorAssessment.Models
{
    public class ServiceResult<T>
    {
        public bool Successful { get; set; }
        public T? Data { get; set; }

        public List<string> ErrorMessages { get; set; } = new List<string>();

        public static ServiceResult<T> Ok(T data) => new() { Successful = true, Data = data };
        public static ServiceResult<T> Fail(List<string> errorMessages) => new() { Successful = false, ErrorMessages = errorMessages };
    }
}