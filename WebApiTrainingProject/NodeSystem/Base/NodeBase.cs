namespace WebApiTrainingProject.NodeSystem.Base
{
    public enum NodeInputKey
    {
        Value,
        A,
        B,
        Str1,
        Str2
    }
    public abstract class NodeBase
    {
        protected T GetInput<T>(
            Dictionary<NodeInputKey, object> inputs,
            NodeInputKey key)
        {
            if (!inputs.TryGetValue(key, out var value))
                throw new ArgumentException($"Input '{key}' is required.");

            return (T)Convert.ChangeType(value, typeof(T));
        }
        public abstract Task<object> ExecuteAsync(
            Dictionary<NodeInputKey, object> inputs);
    }

}
