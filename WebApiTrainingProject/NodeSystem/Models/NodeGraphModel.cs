namespace WebApiTrainingProject.NodeSystem.Models
{
    public class NodeGraphModel
    {
        public List<NodeModel> Nodes { get; set; } = new();
        public List<NodeConnectionModel> Connections { get; set; } = new();
    }
}
