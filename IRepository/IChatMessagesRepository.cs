using fgssr.Models;

namespace fgssr.IRepository
{
    public interface IChatMessagesRepository
    {
        Task<IEnumerable<ChatMessages>> GetAllAsync();

    }
}
