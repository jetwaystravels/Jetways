namespace OnionConsumeWebAPI.Models
{
    public class ApiResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool status { get; set; }
        public object Details { get; set; }

        
    }
}
