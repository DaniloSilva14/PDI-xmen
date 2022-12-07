using ApiLambda.Data.Interfaces;

namespace ApiLambda.Data
{
    public class DownloadOutput : IBaseOutput
    {
        public string Url { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public DownloadOutput(string url="", bool success=false, string message="")
        {
            Url = url;
            Success = success;
            Message = message;
        }
    }
}
