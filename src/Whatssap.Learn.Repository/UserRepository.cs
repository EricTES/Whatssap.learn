using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Whatssap.Learn.Entities;
using System.Collections.Generic;

namespace Whatssap.Learn.Repository
{
    public class UserRepository : IUserRepository
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
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Gender", user.Gender);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@ProfilePicUrl", user.ProfilePicUrl);
                command.Parameters.AddWithValue("@Status", user.Status);
                command.Parameters.AddWithValue("@Description", user.Description);
                command.Parameters.AddWithValue("@DateJoined", user.DateJoined);

                connection.Open();

                return int.Parse(command.ExecuteScalar().ToString());
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

        public User GetByPhoneNumber(String phoneNumber)
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
                        DateJoined = (DateTime)reader.GetDateFromReader("dateJoined")
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
                        DateJoined = (DateTime)reader.GetDateFromReader("dateJoined")
                    };
                }

                return user;
            }
        }

        public User GetByEmail(String email)
        {
            using (var connection = ConnectionManager.GetConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT * FROM Users WHERE email = @Email";
                command.Parameters.AddWithValue("@Email", email);

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
                        DateJoined = (DateTime)reader.GetDateFromReader("dateJoined")
                    };
                }
                return user;
            }
        }

        public void UpdateProfile(ProfileInfo profileInfo, int userID)
        {
            using (var connection = ConnectionManager.GetConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = @"UPDATE Users 
                                        SET FirstName = @FirstName, LastName = @LastName, Username = @Username, ProfilePicUrl = @ProfilePicUrl, Status = @Status, Description = @Description
                                        WHERE UserID = @UserID";

                command.Parameters.AddWithValue("@FirstName", profileInfo.FirstName);
                command.Parameters.AddWithValue("@LastName", profileInfo.LastName);
                command.Parameters.AddWithValue("@Username", profileInfo.Username);
                command.Parameters.AddWithValue("@ProfilePicUrl", profileInfo.ProfilePicUrl);
                command.Parameters.AddWithValue("@Status", profileInfo.Status);
                command.Parameters.AddWithValue("@Description", profileInfo.Description);
                command.Parameters.AddWithValue("@UserID", userID);

                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdatePersonalInfo(PersonalInfo personalInfo, int userID)
        {
            using (var connection = ConnectionManager.GetConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = @"UPDATE Users
                                        SET PasswordHash = @PasswordHash, Email = @Email, PhoneNumber = @PhoneNumber, Gender = @Gender
                                        WHERE UserID = @UserID";

                command.Parameters.AddWithValue("@PasswordHash", personalInfo.PasswordHash);
                command.Parameters.AddWithValue("@Email", personalInfo.Email);
                command.Parameters.AddWithValue("@PhoneNumber", personalInfo.PhoneNumber);
                command.Parameters.AddWithValue("Gender", personalInfo.Gender);
                command.Parameters.AddWithValue("@UserID", userID);

                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        #region old methods
        //public void Update(int userId, Dictionary<string,string> columns)
        //{
        //    //read up on LINQ: Iterating without using loops
        //    using (var connection = ConnectionManager.GetConnection(_connectionString))
        //    {
        //        var command = connection.CreateCommand();
        //        command.CommandType = CommandType.Text;

        //        StringBuilder columnsString = new StringBuilder();
        //        foreach (KeyValuePair<string, string> column in columns)
        //        {
        //            columnsString.Append(column.Key + "= @" + column.Key);
        //        }

        //        command.CommandText = "UPDATE Users SET " + columnsString.ToString() + " WHERE @UserID";
        //        command.Parameters.AddWithValue("@UserID", userId);
        //        foreach (KeyValuePair<string, string> column in columns)
        //        {
        //            command.Parameters.AddWithValue("@"+column.Key, column.Value);
        //        }

        //        connection.Open();
        //        command.ExecuteNonQuery();
        //    }
        //}
        #endregion
    }
}
