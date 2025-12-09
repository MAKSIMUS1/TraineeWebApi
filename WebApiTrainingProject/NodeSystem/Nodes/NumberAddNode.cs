using WebApiTrainingProject.NodeSystem.Base;

namespace WebApiTrainingProject.NodeSystem.Nodes
{
    public class NumberAddNode : NodeBase
    {
        public override Task<object> ExecuteAsync(
            Dictionary<NodeInputKey, object> inputs)
        {
            double a = GetInput<double>(inputs, NodeInputKey.A);
            double b = GetInput<double>(inputs, NodeInputKey.B);

            return Task.FromResult<object>(a + b);
        }
    }
}
