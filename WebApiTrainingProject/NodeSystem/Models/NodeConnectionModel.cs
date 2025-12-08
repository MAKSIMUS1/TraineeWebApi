namespace WebApiTrainingProject.NodeSystem.Models
{
    public class NodeConnectionModel
    {
        public string FromNodeId { get; set; } = "";
        public string FromOutputName { get; set; } = "";
        public string ToNodeId { get; set; } = "";
        public string ToInputName { get; set; } = "";
    }
}
