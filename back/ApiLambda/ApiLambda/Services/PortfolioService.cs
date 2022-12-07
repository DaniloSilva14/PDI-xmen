using Amazon.S3;
using Amazon.S3.Model;
using ApiLambda.Data;

namespace ApiLambda.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IAmazonS3 _s3;
        private readonly string _bucketName;
        private readonly string _fileName;

        public PortfolioService(IAmazonS3 s3)
        {
            _s3 = s3;
            _bucketName = Environment.GetEnvironmentVariable("BucketName");
            _fileName = Environment.GetEnvironmentVariable("FileName");
        }

        public async Task<DownloadOutput> Download()
        { 
            var output = new DownloadOutput();
            output.Url = GetPreSignedUrl();
            output.Success = !string.IsNullOrWhiteSpace(output.Url);

            return output;
        }

        //public async Task<DownloadOutput> Download(string password)
        //{
        //    var auth = Authenticate(password);

        //    var output = new DownloadOutput("", auth.Success, auth.Message);

        //    if (auth.Success)
        //    {
        //        output.Url = GetPreSignedUrl();
        //        auth.Success = !string.IsNullOrWhiteSpace(output.Url);
        //    }

        //    return output;
        //}

        public async Task<UploadOutput> Upload(string password, IFormFile[] file)
        {
            var auth = Authenticate(password);

            var output = new UploadOutput(auth.Success, auth.Message);

            if (auth.Success)
            {
                var response = await UploadFileS3(file[0]);
                output.Success = response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }

            return output;
        }

        private static AuthenticateOutput Authenticate(string password)
        {
            var output = new AuthenticateOutput();

            if (string.IsNullOrWhiteSpace(password))
            {
                output.Message = "Senha não informada.";
                return output;
            }
            else if (password != Environment.GetEnvironmentVariable("Password"))
            {
                output.Message = "Senha incorreta.";
                return output;
            }

            output.Success = true;
            return output;
        }

        private string GetPreSignedUrl()
        {
            int expiresTime = int.Parse(Environment.GetEnvironmentVariable("expiresTime"));

            var request = new GetPreSignedUrlRequest()
            {
                BucketName  = _bucketName,
                Key         = _fileName,
                Expires     = DateTime.Now.AddMinutes(expiresTime)
            };

            var response = _s3.GetPreSignedURL(request);

            return response;
        }

        private async Task<PutObjectResponse> UploadFileS3(IFormFile file)
        {
            var request = new PutObjectRequest()
            {
                BucketName  = _bucketName,
                Key         = _fileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType
            };

            var response = await _s3.PutObjectAsync(request);

            return response;
        }
    }
}
