using WebApiTrainingProject.NodeSystem.Base;

namespace WebApiTrainingProject.NodeSystem.Nodes
{
    public class StringConcatNode : NodeBase
    {
        public override Task<object> ExecuteAsync(Dictionary<string, object> inputs)
        {
            if (!inputs.TryGetValue("Str1", out var s1) ||
                !inputs.TryGetValue("Str2", out var s2))
                throw new ArgumentException("Inputs 'Str1' and 'Str2' are required.");

            string str1 = s1?.ToString() ?? string.Empty;
            string str2 = s2?.ToString() ?? string.Empty;

            string result = str1 + str2;

            return Task.FromResult<object>(result);
        }
    }

}
