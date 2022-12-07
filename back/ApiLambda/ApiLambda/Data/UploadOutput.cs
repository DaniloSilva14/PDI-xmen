using ApiLambda.Data.Interfaces;

namespace ApiLambda.Data
{
    public class UploadOutput : IBaseOutput
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public UploadOutput( bool success=false, string message="")
        {
            Success = success;
            Message = message;
        }
    }
}
