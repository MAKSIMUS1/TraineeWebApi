namespace WebApiTrainingProject.Data.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; } = default!;
        public ICollection<NodeGraph> NodeGraphs { get; set; } = new List<NodeGraph>();
    }
}
