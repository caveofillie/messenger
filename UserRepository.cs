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
                            Id = (int)reader[0],
                            Username = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            Email = reader[3].ToString()
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

            return new User
            {
                Username = username,
                Password = password,
                Email = email
            };
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

            return -1;
        }

        public int AddMessageToDatabase(string message, string sender, string recipient)
        {
            string query = $@"INSERT INTO Messages VALUES('{message}', '{GetIdFromUsername(sender)}', '{GetIdFromUsername(recipient)}');SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, _connection);

            _connection.Open();
            int messageId = Convert.ToInt32(command.ExecuteScalar());

            _connection.Close();

            return messageId;
        }

        public int AddPhotoToDatabase(int messageId, string photoUrl)
        {
            string query = $@"INSERT INTO Photos VALUES('{photoUrl}', GetDate(), {messageId});SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, _connection);

            _connection.Open();
            int photoId = Convert.ToInt32(command.ExecuteScalar());

            _connection.Close();

            return photoId;
        }

        public List<Message> GetMessagesByUser(int userId, bool sentOrReceived)
        {
            List<Message> messages = new List<Message>();

            string query;

            if (sentOrReceived)
            {
                query = $@"SELECT * FROM Messages WHERE SenderId = '{userId}'";
            }
            else
            {
                query = $@"SELECT * FROM Messages WHERE RecipientId = '{userId}'";
            }

            SqlCommand command = new SqlCommand(query, _connection);

            _connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Message message = new Message
                    {
                        Id = (int)reader[0],
                        MessageText = reader[1].ToString(),
                        SenderId = (int)reader[2],
                        RecipientId = (int)reader[3]
                    };
                    messages.Add(message);
                }
            }
            finally
            {
                reader.Close();
                _connection.Close();
            }
            return messages;
        }
    }
}
