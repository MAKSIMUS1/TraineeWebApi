namespace WebApiTrainingProject.Data.Models
{
    public class CustomNodeType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string InputDefinitions { get; set; } // JSON
        public string OutputDefinitions { get; set; } // JSON
    }
}
