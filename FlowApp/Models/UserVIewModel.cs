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
    }
}