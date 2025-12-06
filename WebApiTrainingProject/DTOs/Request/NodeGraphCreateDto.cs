namespace WebApiTrainingProject.DTOs.Request
{
    public class NodeGraphCreateDto
    {
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public string JsonData { get; set; }
    }
}
