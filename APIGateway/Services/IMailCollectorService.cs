namespace APIGateway.Services
{
    public interface IMailCollectorService
    {
        public Task StartCollecting();
        public Task StopCollecting();
    }
}
