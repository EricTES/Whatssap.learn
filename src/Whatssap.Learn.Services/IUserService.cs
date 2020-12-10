using Whatssap.Learn.Entities;

namespace Whatssap.Learn.Services
{
    public interface IUserService
    {
        (string message, bool isValidEmail) IsValidEmail(string email);
        ServiceResponse<User> Signin(string phoneNumber, string password);
        ServiceResponse<User> SignUpUser(NewUser newUser);
        ServiceResponse<PersonalInfo> UpdatePersonalInfo(PersonalInfo personalInfo, int userID);
        ServiceResponse<ProfileInfo> UpdateProfile(ProfileInfo profileInfo, int userID);
    }
}