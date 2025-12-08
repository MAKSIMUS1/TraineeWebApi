namespace WebApiTrainingProject.NodeSystem.Models
{
    public class NodeModel
    {
        public string Id { get; set; } = "";
        public string Type { get; set; } = "";
        public Dictionary<string, string> Inputs { get; set; } = new();
    }
}
