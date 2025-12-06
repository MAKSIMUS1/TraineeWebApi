namespace WebApiTrainingProject.DTOs.Response
{
    public class NodeGraphDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public string JsonData { get; set; }
    }
}
