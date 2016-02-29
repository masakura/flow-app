using System.ComponentModel.DataAnnotations;

namespace FlowApp.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public override string ToString()
        {
            return UserName;
        }

        public static UserViewModel Create(ApplicationUser user)
        {
            return user == null
                ? null
                : new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName
                };
        }
    }
}