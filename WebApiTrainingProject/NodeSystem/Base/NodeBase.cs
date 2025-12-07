namespace WebApiTrainingProject.NodeSystem.Base
{
    public abstract class NodeBase
    {
        public abstract Task<object> ExecuteAsync(Dictionary<string, object> inputs);
    }

}
