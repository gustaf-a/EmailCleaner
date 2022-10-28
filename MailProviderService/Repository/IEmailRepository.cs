using MailProviderService.Data;

namespace MailProviderService.Repository
{
    public interface IEmailRepository
    {
        public Task Add(List<Email> emails);
        public Task<List<Email>> GetAll();
        public Task Delete(List<Email> emails);
    }
}
