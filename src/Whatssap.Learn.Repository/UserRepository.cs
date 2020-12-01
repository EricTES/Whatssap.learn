using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Whatssap.Learn.Entities;
using System.Collections.Generic;

namespace Whatssap.Learn.Repository
{
    public class UserRepository
    {
        private string _connectionString;

        public UserRepository(String connectionString)
        {
            this._connectionString = connectionString;
        }

        public int Insert(User user) 
        {
            using (var connection = ConnectionManager.GetConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = @"Insert into Users
                                        (PasswordHash,Email,PhoneNumber, FirstName, LastName, Gender, Username, ProfilePicUrl, [Status], [Description], DateJoined) 
                                        Values(@PasswordHash ,@Email,@PhoneNumber,@FirstName,@LastName, @Gender, @Username, @ProfilePicUrl, @Status, @Description, @DateJoined)  
                                        SELECT SCOPE_IDENTITY()";
                command.CommandType = CommandType.Text;
                
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@Email", user.Email );
                command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Gender",user.Gender);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@ProfilePicUrl", user.ProfilePicUrl);
                command.Parameters.AddWithValue("@Status", user.Status);
                command.Parameters.AddWithValue("@Description", user.Description);
                command.Parameters.AddWithValue("@DateJoined", user.DateJoined);

                connection.Open();

                return (int) command.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            using (var connection = ConnectionManager.GetConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "DELETE FROM USERS WHERE userID = @UserID ";
                command.Parameters.AddWithValue("@UserID", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public User Get(String phoneNumber)
        {
            using (var connection = ConnectionManager.GetConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT * FROM Users WHERE phoneNumber = @PhoneNumber";
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                User user = null;
                if (reader.Read())
                {
                    user = new User
                    {
                        UserId = reader.GetIntFromReader("userID"),
                        PasswordHash = reader.GetStringFromReader("passwordHash"),
                        Email = reader.GetStringFromReader("email"),
                        PhoneNumber = reader.GetStringFromReader("phoneNumber"),
                        FirstName = reader.GetStringFromReader("firstName"),
                        LastName = reader.GetStringFromReader("lastName"),
                        Gender = reader.GetStringFromReader("gender"),
                        Username = reader.GetStringFromReader("username"),
                        ProfilePicUrl = reader.GetStringFromReader("profilePicUrl"),
                        Status = reader.GetStringFromReader("status"),
                        Description = reader.GetStringFromReader("description"),
                        DateJoined = (DateTime)reader["dateJoined"]
                    };
                }
                return user;
            }
        }

        public User Get(int id)
        {
            using (var connection = ConnectionManager.GetConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT * FROM Users WHERE userID = @UserID";
                command.Parameters.AddWithValue("@UserID", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                User user = null;
                if (reader.Read()) 
                {
                    user = new User
                    {
                        UserId = reader.GetIntFromReader("userID"),
                        PasswordHash = reader.GetStringFromReader("passwordHash"),
                        Email = reader.GetStringFromReader("email"),
                        PhoneNumber = reader.GetStringFromReader("phoneNumber"),
                        FirstName = reader.GetStringFromReader("firstName"),
                        LastName = reader.GetStringFromReader("lastName"),
                        Gender = reader.GetStringFromReader("gender"),
                        Username = reader.GetStringFromReader("username"),
                        ProfilePicUrl = reader.GetStringFromReader("profilePicUrl"),
                        Status = reader.GetStringFromReader("status"),
                        Description = reader.GetStringFromReader("description"),
                        DateJoined = (DateTime)reader["dateJoined"]
                    };
                }

                return user;
            }
        }
        
        public void Update(int userId, Dictionary<string,string> columns)
        {
            //read up on LINQ: Iterating without using loops
            using (var connection = ConnectionManager.GetConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                StringBuilder columnsString = new StringBuilder();
                foreach (KeyValuePair<string, string> column in columns)
                {
                    columnsString.Append(column.Key + "= @" + column.Key);
                }

                command.CommandText = "UPDATE Users SET " + columnsString.ToString() + " WHERE @UserID";
                command.Parameters.AddWithValue("@UserID", userId);
                foreach (KeyValuePair<string, string> column in columns)
                {
                    command.Parameters.AddWithValue("@"+column.Key, column.Value);
                }

                connection.Open();
                command.ExecuteNonQuery();
            }
        }  
    }
}
