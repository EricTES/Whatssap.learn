using System;
using Whatssap.Learn.Repository;
using Whatssap.Learn.Entities;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Whatssap.Learn.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public const string UserStatus_Online = "Online";
        public const string UserStatus_Offline = "Offline";
        public const string UserStatus_Busy = "Busy";
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        #region public methods
        public ServiceResponse<User> SignUpUser(NewUser newUser)
        {
            try
            {
                User user = null;
                var (emailMsg, isValidEmail) = IsValidEmail(newUser.Email);
                if (!isValidEmail) return new ServiceResponse<User> { ErrorMessage = emailMsg, Success = false };

                var (passwordMsg, isValidPassword) = IsValidPassword(newUser.Password);
                if (!isValidPassword) return new ServiceResponse<User> { ErrorMessage = passwordMsg, Success = false };

                var (phoneNumberMsg, isValidPhoneNumber) = IsValidPhoneNumber(newUser.PhoneNumber);
                if (!isValidPhoneNumber) return new ServiceResponse<User> { ErrorMessage = phoneNumberMsg, Success = false };

                if (string.IsNullOrWhiteSpace(newUser.FirstName)) return new ServiceResponse<User> { ErrorMessage = "First Name cannot be empty", Success = false };
                if (string.IsNullOrWhiteSpace(newUser.LastName)) return new ServiceResponse<User> { ErrorMessage = "Last Name cannot be empty", Success = false };

                user = new User();
                user.PasswordHash = ComputeHash(newUser.Password);
                user.Email = newUser.Email;
                user.PhoneNumber = newUser.PhoneNumber;
                user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.Gender = newUser.Gender;
                user.Username = newUser.Username;
                user.ProfilePicUrl = newUser.ProfilePicUrl;
                user.Status = newUser.Status;
                user.Description = newUser.Description;
                user.DateJoined = DateTime.Now;
                user.UserId = userRepository.Insert(user);

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
                User user = userRepository.GetByPhoneNumber(phoneNumber);
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

        public ServiceResponse<ProfileInfo> UpdateProfile(ProfileInfo profileInfo, int userID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(profileInfo.FirstName)) return new ServiceResponse<ProfileInfo> { ErrorMessage = "First Name cannot be empty", Success = false };
                if (string.IsNullOrWhiteSpace(profileInfo.LastName)) return new ServiceResponse<ProfileInfo> { ErrorMessage = "Last Name cannot be empty", Success = false };

                // status should be an entity by itself with x allowable status

                userRepository.UpdateProfile(profileInfo, userID);

                return new ServiceResponse<ProfileInfo> { ErrorMessage = string.Empty, Response = profileInfo, Success = false };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ProfileInfo> { ErrorMessage = ex.Message, Success = false };
            }
        }

        public ServiceResponse<PersonalInfo> UpdatePersonalInfo(PersonalInfo personalInfo, int userID)
        {
            try
            {
                var (emailMsg, isValidEmail) = IsValidEmail(personalInfo.Email);
                if (!isValidEmail) return new ServiceResponse<PersonalInfo> { ErrorMessage = emailMsg, Success = false };

                var (passwordMsg, isValidPassword) = IsValidPassword(personalInfo.PasswordHash);
                if (!isValidPassword) return new ServiceResponse<PersonalInfo> { ErrorMessage = passwordMsg, Success = false };

                var (phoneNumberMsg, isValidPhoneNumber) = IsValidPhoneNumber(personalInfo.PhoneNumber);
                if (!isValidPhoneNumber) return new ServiceResponse<PersonalInfo> { ErrorMessage = phoneNumberMsg, Success = false };

                personalInfo.PasswordHash = ComputeHash(personalInfo.PasswordHash);
                userRepository.UpdatePersonalInfo(personalInfo, userID);

                return new ServiceResponse<PersonalInfo> { ErrorMessage = string.Empty, Response = personalInfo, Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<PersonalInfo> { ErrorMessage = ex.Message, Success = false };
            }
        }

        //updatestatus  method

        // CRUD METHODS FOR ALL THE ENTITIES
        #endregion

        #region private methods
        public string ComputeHash(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
                return Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }

        public (string message, bool isValidEmail) IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return ("Email is empty", false);

            User user = userRepository.GetByEmail(email);
            if (user != null) return ("Email address already exist", false);

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e) { return (e.Message, false); }
            catch (ArgumentException e) { return (e.Message, false); }

            try
            {
                if (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))) return ("", true);
                return ("Email is invalid", false);
            }
            catch (RegexMatchTimeoutException e) { return (e.Message, false); }
        }

        public (string message, bool isValidPassword) IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return ("Password cannot be empty", false);

            // Got the expression off https://stackoverflow.com/questions/5142103/regex-to-validate-password-strength#answer-5142164
            if (Regex.IsMatch(password, "^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$")) return ("Password is too weak", false);
            return ("", true);
        }

        public (string message, bool isValidPhoneNumber) IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return ("Phone number cannot be empty", false);

            var user = userRepository.GetByPhoneNumber(phoneNumber);
            if (user != null) return ("Phone Number already exist", false);

            return (string.Empty, true);
        }
        #endregion
    }
}
