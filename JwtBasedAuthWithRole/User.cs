namespace JwtBasedAuthWithRole
{
    public static class RoleConstant
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string AdminUser = "Admin,User";
    }
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            users.Add(new User { Username = "user", Name = "user satu", Password = "user", Role = RoleConstant.User });
            users.Add(new User { Username = "admin", Name = "admin satu", Password = "admin", Role = RoleConstant.Admin });
            
            return users;
        }
    }
}
