using ApiLambda.Data.Interfaces;

namespace ApiLambda.Data
{
    public class AuthenticateOutput : IBaseOutput
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public AuthenticateOutput(bool success=false, string message="")
        {
            Success = success;
            Message = message;
        }
    }
}
