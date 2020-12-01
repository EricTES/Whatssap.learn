using System;
using Whatssap.Learn.Repository;
using Whatssap.Learn.Entities;


namespace Whatssap.Learn.Services
{
    public class UserService
    { 
        private readonly UserRepository userRepository;
        public UserService(string connectionString)
        {
            userRepository = new UserRepository(connectionString);
        }

        #region public methods
        public ServiceResponse<User> SignUpUser(User user) 
        {
            try
            {
                user.UserId = userRepository.Insert(user);
                if (string.IsNullOrWhiteSpace(user.PasswordHash)) return new ServiceResponse<User> { ErrorMessage = "Password cannot be empty", Success = false };
                user.PasswordHash = ComputeHash(user.PasswordHash);
                if (string.IsNullOrWhiteSpace(user.PhoneNumber)) return new ServiceResponse<User> { ErrorMessage = "Phone number cannot be empty", Success = false };
                if (string.IsNullOrWhiteSpace(user.FirstName)) return new ServiceResponse<User> { ErrorMessage = "First Name cannot be empty", Success = false };
                if (string.IsNullOrWhiteSpace(user.LastName)) return new ServiceResponse<User> { ErrorMessage = "Last Name cannot be empty", Success = false };
                user.DateJoined = DateTime.Now;        

                return new ServiceResponse<User> { ErrorMessage = string.Empty, Response = user, Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<User> { ErrorMessage = ex.Message, Success = false };
            }
        }
        
        public ServiceResponse<User> Signin(string phoneNumber, string password) 
        {
            try 
            {
                User user = userRepository.Get(phoneNumber);
                var passwordHash = ComputeHash(password);

                if (user == null) return new ServiceResponse<User> { ErrorMessage = "Phone number doesn't exist", Success = false };
                if (passwordHash != user.PasswordHash) return new ServiceResponse<User> { ErrorMessage = "Password incorrect", Success = false };

                return new ServiceResponse<User> { ErrorMessage = string.Empty, Response = user, Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<User> { ErrorMessage = ex.Message, Success = false };
            }
        }
        #endregion

        #region private methods

        // organise code : private should be in different file or at the bottom
        private string ComputeHash(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
                return Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }
        #endregion
    }
}
