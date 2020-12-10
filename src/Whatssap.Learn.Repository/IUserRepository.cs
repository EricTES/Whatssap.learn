using Whatssap.Learn.Entities;

namespace Whatssap.Learn.Repository
{
    public interface IUserRepository
    {
        void Delete(int id);
        User Get(int id);
        User GetByEmail(string email);
        User GetByPhoneNumber(string phoneNumber);
        int Insert(User user);
        void UpdatePersonalInfo(PersonalInfo personalInfo, int userID);
        void UpdateProfile(ProfileInfo profileInfo, int userID);
    }
}