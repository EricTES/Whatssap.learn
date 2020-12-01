using System;
using System.Collections.Generic;
using System.Text;


namespace Whatssap.Learn.Entities
{
    public class User
    {
        public int? UserId { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string PhoneNumber {get;set;}
        public string FirstName {get;set; }
        public string LastName {get;set; }
        public string Gender {get;set; }
        public string Username {get;set; }
        public string ProfilePicUrl {get;set; }
        public string Status {get;set; }
        public string Description {get;set; }
        public DateTime DateJoined {get;set; }

    }
}
