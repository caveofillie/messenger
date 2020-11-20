using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Illie.Chat
{
    class UserService
    {
        private readonly UserRepository userRepo = new UserRepository(new SqlConnection($@"Server=(localdb)\MSSQLLocalDb;Database=IlliesChatDb;Trusted_Connection=True"));

        public User Login(string username, string password)
        {
            User user = userRepo.GetUserFromDatabase(username, password);
            return user;
        }

        public User Register(string username, string password, string email)
        {
            User user = userRepo.CreateNewUser(username, password, email);
            return user;
        }

        public bool SendMessage(string message, User sender, string recipient, string photoUrl)
        {
            if (message.Length < 15)
            {
                return false;
            }
            int messageId = userRepo.AddMessageToDatabase(message, sender.Username, recipient);
            if (photoUrl != null)
            {
                userRepo.AddPhotoToDatabase(messageId, photoUrl);
            }
            return true;
        }

        public List<Message> ViewMessages(User user, bool sentOrReceived)
        {
            return userRepo.GetMessagesByUser(user.Id, sentOrReceived);
        }
    }
}
