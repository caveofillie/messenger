using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Illie.Chat
{
    class UserService
    {
        private UserRepository userRepo = new UserRepository(new SqlConnection($@"Server=(localdb)\MSSQLLocalDb;Database=IlliesChatDb;Trusted_Connection=True"));

        public User Login(string username, string password)
        {
            try
            {
                if (userRepo.GetUserFromDatabase(username, password) != null)
                {
                    return userRepo.GetUserFromDatabase(username, password);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public User Register(string username, string password, string email)
        {
            try
            {
                if (userRepo.CreateNewUser(username, password, email) != null)
                {
                    return userRepo.CreateNewUser(username, password, email);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public void SendMessage(string message, User sender, string recipient)
        {
            if (message.Length >= 15)
            {
                userRepo.SendMessage(message, sender.username, recipient);
            }
        }
    }
}
