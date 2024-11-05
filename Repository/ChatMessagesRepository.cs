using fgssr.Data;
using fgssr.IRepository;
using fgssr.Models;
using fgssr.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace fgssr.Repository
{
    public class ChatMessagesRepository:IChatMessagesRepository
    {

        private readonly ApplicationDbContext _context;

        public ChatMessagesRepository(ApplicationDbContext context) 
        {
            _context = context;  
        }

        public async Task<IEnumerable<ChatMessages>> GetAllAsync()
        {
           var msg = await _context.ChatMessages.Include(m=>m.Sender).OrderBy(m=>m.Timestamp).ToListAsync();
           return msg;
        }


    }
}
