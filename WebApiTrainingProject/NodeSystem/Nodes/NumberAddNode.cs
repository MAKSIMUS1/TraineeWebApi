using WebApiTrainingProject.NodeSystem.Base;

namespace WebApiTrainingProject.NodeSystem.Nodes
{
    public class NumberAddNode : NodeBase
    {
        public override Task<object> ExecuteAsync(Dictionary<string, object> inputs)
        {
            if (!inputs.TryGetValue("A", out var aObj) ||
                !inputs.TryGetValue("B", out var bObj))
                throw new ArgumentException("Inputs 'A' and 'B' are required.");

            if (aObj is not IConvertible || bObj is not IConvertible)
                throw new ArgumentException("Inputs must be numbers.");

            double a = Convert.ToDouble(aObj);
            double b = Convert.ToDouble(bObj);

            double result = a + b;

            return Task.FromResult<object>(result);
        }
    }

}
