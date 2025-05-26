namespace JokesApp.Models
{
        public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecurityQestion { get; set; }
        public string SecurityAnswer { get; set; }

        public User()
        {

        }
    }
}
