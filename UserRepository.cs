using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Illie.Chat
{
    class UserRepository
    {
        private SqlConnection _connection { get; set; }
        public UserRepository(SqlConnection connectionString)
        {
            _connection = connectionString;
        }

        public User GetUserFromDatabase(string givenUsername, string givenPassword)
        {
            string query = @$"SELECT * FROM Users WHERE Username = '{givenUsername}'";
            SqlCommand command = new SqlCommand(query, _connection);

            _connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    if (reader[1].ToString() == givenUsername && reader[2].ToString() == givenPassword)
                    {
                        User user = new User
                        {
                            username = reader[1].ToString(),
                            password = reader[2].ToString(),
                            email = reader[3].ToString()
                        };
                        reader.Close();
                        UpdateLastLoggedInTime(givenUsername);
                        return user;
                    }
                }
                finally
                {
                    reader.Close();
                    _connection.Close();
                }
            }
            return null;
        }

        private void AddUserToDatabase(string username, string password, string email)
        {
            string query = $@"INSERT INTO Users VALUES('{username}', '{password}', '{email}', GetDate(), GetDate())";
            SqlCommand command = new SqlCommand(query, _connection);

            _connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            reader.Close();
            _connection.Close();
        }

        private void UpdateLastLoggedInTime(string username)
        {
            string query = $@"UPDATE Users SET LastLoggedIn = GetDate() WHERE Username = '{username}';";
            SqlCommand command = new SqlCommand(query, _connection);

            command.ExecuteReader();
        }

        public User CreateNewUser(string username, string password, string email)
        {
            AddUserToDatabase(username, password, email);

            User user = new User
            {
                username = username,
                password = password,
                email = email
            };

            return user;
        }

        private int GetIdFromUsername(string username)
        {
            string query = @$"SELECT * FROM Users WHERE Username = '{username}'";
            SqlCommand command = new SqlCommand(query, _connection);

            _connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    return int.Parse(reader[0].ToString());
                }

                finally
                {
                    reader.Close();
                    _connection.Close();
                }
            }

            return 0;
        }

        public void SendMessage(string message, string sender, string recipient)
        {
            string query = $@"INSERT INTO Messages VALUES('{message}', '{GetIdFromUsername(sender)}', '{GetIdFromUsername(recipient)}')";
            SqlCommand command = new SqlCommand(query, _connection);

            _connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            reader.Close();
            _connection.Close();
        }
    }
}
