using fgssr.Controllers;
using fgssr.Data;
using fgssr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace fgssr.Hubs
{
    public class ChatHub:Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public ChatHub(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessage(string currentUser, string message)
        {
            var getUser = await _userManager.FindByNameAsync(currentUser);
            var user = getUser?.UserName;
            var profileImage = getUser?.ProfileImage;
            var msg = new ChatMessages
            {
                Content = message,
                SenderId = getUser?.Id,
                SenderName = currentUser,
                ProfileImage=profileImage
                
            };
            _context.ChatMessages.Add(msg);
            _context.SaveChanges();
            await Clients.All.SendAsync("ReceiveMessage",user, message,profileImage);
        }

       

    }
}
