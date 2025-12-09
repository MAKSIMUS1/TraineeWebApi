using Serilog;
using WebApiTrainingProject.NodeSystem.Base;

namespace WebApiTrainingProject.NodeSystem.Nodes
{
    public class ConsoleLogNode : NodeBase
    {
        public override Task<object> ExecuteAsync(
            Dictionary<NodeInputKey, object> inputs)
        {
            var value = GetInput<object>(inputs, NodeInputKey.Value);

            Log.Information("ConsoleLogNode: {@Value}", value);

            return Task.FromResult(value);
        }
    }

}
