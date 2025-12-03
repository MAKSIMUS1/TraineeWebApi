namespace WebApiTrainingProject.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
