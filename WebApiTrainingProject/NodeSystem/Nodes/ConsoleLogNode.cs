using Serilog;
using WebApiTrainingProject.NodeSystem.Base;

namespace WebApiTrainingProject.NodeSystem.Nodes
{
    public class ConsoleLogNode : NodeBase
    {
        public override Task<object> ExecuteAsync(Dictionary<string, object> inputs)
        {
            if (!inputs.TryGetValue("Value", out var value))
                throw new ArgumentException("Input 'Value' is required.");

            Log.Information("ConsoleLogNode: {@Value}", value);

            return Task.FromResult(value);
        }
    }
}
