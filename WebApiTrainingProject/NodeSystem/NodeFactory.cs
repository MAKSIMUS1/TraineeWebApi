using WebApiTrainingProject.NodeSystem.Base;
using WebApiTrainingProject.NodeSystem.Nodes;

namespace WebApiTrainingProject.NodeSystem
{
    public class NodeFactory
    {
        private readonly Dictionary<string, Type> _nodeTypes;

        public NodeFactory()
        {
            _nodeTypes = new Dictionary<string, Type>
        {
            { "NumberAdd", typeof(NumberAddNode) },
            { "StringConcat", typeof(StringConcatNode) },
            { "ConsoleLog", typeof(ConsoleLogNode) }
        };
        }

        public NodeBase Create(string typeName)
        {
            if (!_nodeTypes.TryGetValue(typeName, out var type))
                throw new Exception($"Node type '{typeName}' is not registered in NodeFactory.");

            return (NodeBase)Activator.CreateInstance(type)!;
        }
    }
}
