using System;
using System.Collections.Generic;
using System.Text;

namespace Illie.Chat
{
    class Cli
    {
        UserService userService = new UserService();
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
                    User loggedInUser = Login();
                    if (loggedInUser != null)
                    {
                        DisplayOptions(loggedInUser);
                    }
                    else
                    {
                        Console.WriteLine("Your login was unsuccessful.");
                    }
                }
            }
            else if (loginOrRegister == "2")
            {
                User loggedInUser = Login();
                if (loggedInUser != null)
                {
                    DisplayOptions(loggedInUser);
                }
                else
                {
                    Console.WriteLine("Your login was unsuccessful.");
                }
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

            if (user != null)
            {
                return user;
            }
            else return null;
        }

        private User Login()
        {
            string username = RequestStringInput("username");
            string password = RequestStringInput("password");
            bool loggedIn = false;
            User user = userService.Login(username, password);

            try
            {
                if (user != null)
                {
                    loggedIn = true;
                    return user;
                }
                else return null;
            }
            finally 
            {
                DisplayLoginMessage(loggedIn);
            }
        }

        private void DisplayOptions(User loggedInUser)
        {
            Console.WriteLine("1) Send a message");
            Console.WriteLine("2) View inbox");
            Console.WriteLine("3) View sent messages");

            int option = int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    SendMessage(loggedInUser);
                    break;
                case 2:
                    Console.WriteLine("Tuesday");
                    break;
                case 3:
                    Console.WriteLine("Wednesday");
                    break;
            }
        }

        private void SendMessage(User loggedInUser)
        {
            string recipient = RequestStringInput("recipient username");
            string message = RequestStringInput("message");

            Console.WriteLine("Do you have a photo to attach? Y/N");
            string photoAttached = Console.ReadLine();

            if (photoAttached == "y" || photoAttached == "Y")
            {
                AttachPhoto();
            }
        }
    }
}
