using WebApiTrainingProject.NodeSystem.Base;

namespace WebApiTrainingProject.NodeSystem.Nodes
{
    public class StringConcatNode : NodeBase
    {
        public override Task<object> ExecuteAsync(
            Dictionary<NodeInputKey, object> inputs)
        {
            string str1 = GetInput<object>(inputs, NodeInputKey.Str1)?.ToString() ?? "";
            string str2 = GetInput<object>(inputs, NodeInputKey.Str2)?.ToString() ?? "";

            return Task.FromResult<object>(str1 + str2);
        }
    }


}
