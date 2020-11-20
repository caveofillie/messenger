using System;
using System.Collections.Generic;
using System.Text;

namespace Illie.Chat
{
    class Cli
    {
        private readonly UserService userService = new UserService();
        public void Run()
        {
            Console.WriteLine("1) Register");
            Console.WriteLine("2) Login");
            string loginOrRegister = Console.ReadLine();
            if (loginOrRegister == "1")
            {
                User registeredUser = Register();

                if (registeredUser != null)
                {
                    HandleLogin();
                }
                else
                {
                    Console.WriteLine("Your registration was unsuccessful.");
                }
            }
            else if (loginOrRegister == "2")
            {
                HandleLogin();
            }
            else
            {
                Console.WriteLine("This is invalid input, please submit a 1 or 2.");
            }
        }

        private string RequestStringInput(string requiredInput)
        {
            Console.WriteLine($"Please input your {requiredInput}.");
            return Console.ReadLine();
        }

        private void DisplayLoginMessage(bool loggedIn)
        {
            if (loggedIn == true)
            {
                Console.WriteLine("Successfully logged in!");
            }
            else
            {
                Console.WriteLine("Incorrect credentials.");
            }
        }

        private User Register()
        {
            string username = RequestStringInput("username");
            string password = RequestStringInput("password");
            string emailAddress = RequestStringInput("email address");

            User user = userService.Register(username, password, emailAddress);

            return user;
        }

        private void HandleLogin()
        {
            User loggedInUser = Login();
            while (loggedInUser != null)
            {
                DisplayOptions(loggedInUser);
            }
            Console.WriteLine("Your login was unsuccessful.");
        }

        private User Login()
        {
            string username = RequestStringInput("username");
            string password = RequestStringInput("password");
            User user = userService.Login(username, password);
            bool loggedIn = user != null;

            DisplayLoginMessage(loggedIn);
            return user;
        }

        private void DisplayOptions(User loggedInUser)
        {
            Console.WriteLine("1) Send a message");
            Console.WriteLine("2) View inbox");
            Console.WriteLine("3) View sent messages");

            bool isParsed = int.TryParse(Console.ReadLine(), out int option);
            switch (option)
            {
                case 1:
                    SendMessage(loggedInUser);
                    break;
                case 2:
                    ViewMessages(loggedInUser, false);
                    break;
                case 3:
                    ViewMessages(loggedInUser, true);
                    break;
            }
        }

        private void SendMessage(User loggedInUser)
        {
            string recipient = RequestStringInput("recipient username");
            string message = RequestStringInput("message");
            string photoUrl = null;

            Console.WriteLine("Do you have a photo to attach? Y/N");
            string photoAttached = Console.ReadLine();

            if (photoAttached == "y" || photoAttached == "Y")
            {
                photoUrl = RequestStringInput("photo URL");
            }

            bool messageSent = userService.SendMessage(message, loggedInUser, recipient, photoUrl);

            if (messageSent == true)
            {
                Console.WriteLine("Your message was sent successfully!");
            }
            else
            {
                Console.WriteLine("Your message was not sent.");
            }
        }

        private void ViewMessages(User loggedInUser, bool sentOrReceived)
        {
            List<Message> messages;

            if (sentOrReceived)
            {
                Console.WriteLine($"Messages sent by {loggedInUser.Username}:");
                messages = userService.ViewMessages(loggedInUser, sentOrReceived);

                foreach (Message message in messages)
                {
                    Console.WriteLine($"{message.MessageText} : {message.RecipientId}");
                }
            }
            else
            {
                Console.WriteLine($"{loggedInUser.Username}'s Inbox:");
                messages = userService.ViewMessages(loggedInUser, sentOrReceived);

                foreach (Message message in messages)
                {
                    Console.WriteLine($"{message.MessageText} : {message.SenderId}");
                }
            }
        }
    }
}
