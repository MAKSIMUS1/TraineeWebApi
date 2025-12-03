namespace WebApiTrainingProject.Data.Models
{
    public class NodeGraph
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public string JsonData {  get; set; } // JSON

        public Project Project { get; set; }
    }
}
