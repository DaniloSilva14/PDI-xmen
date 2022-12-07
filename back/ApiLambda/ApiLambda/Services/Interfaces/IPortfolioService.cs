using ApiLambda.Data;

namespace ApiLambda.Services
{
    public interface IPortfolioService
    {
        Task<UploadOutput> Upload(string password, IFormFile[] file);
        Task<DownloadOutput> Download();
        //Task<DownloadOutput> Download(string password);
    }
}
