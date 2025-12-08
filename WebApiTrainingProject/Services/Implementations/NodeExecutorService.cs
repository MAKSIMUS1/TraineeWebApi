using System.Text.Json;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.NodeSystem;
using WebApiTrainingProject.NodeSystem.Base;
using WebApiTrainingProject.NodeSystem.Models;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services.Implementations
{
    public class NodeExecutorService : INodeExecutorService
    {
        private readonly NodeFactory _factory;

        public NodeExecutorService(NodeFactory factory)
        {
            _factory = factory;
        }

        public async Task<object?> ExecuteGraphAsync(string jsonData)
        {
            var graph = JsonSerializer.Deserialize<NodeGraphModel>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new Exception("Invalid graph JSON");


            var indegree = new Dictionary<string, int>();
            foreach (var node in graph.Nodes)
                indegree[node.Id] = 0;

            foreach (var conn in graph.Connections)
                indegree[conn.ToNodeId]++;

            var queue = new Queue<string>(indegree.Where(x => x.Value == 0).Select(x => x.Key));
            var sorted = new List<string>();

            while (queue.Count > 0)
            {
                var id = queue.Dequeue();
                sorted.Add(id);

                foreach (var edge in graph.Connections.Where(c => c.FromNodeId == id))
                {
                    indegree[edge.ToNodeId]--;
                    if (indegree[edge.ToNodeId] == 0)
                        queue.Enqueue(edge.ToNodeId);
                }
            }

            if (sorted.Count != graph.Nodes.Count)
                throw new Exception("Cycle detected in graph");

            var results = new Dictionary<string, object?>();

            foreach (var nodeId in sorted)
            {
                var node = graph.Nodes.First(n => n.Id == nodeId);

                var inputs = new Dictionary<string, object?>();

                foreach (var inputPair in node.Inputs)
                {
                    string inputName = inputPair.Key;
                    string linkOrValue = inputPair.Value;

                    if (linkOrValue.StartsWith("$"))
                    {
                        var parts = linkOrValue.Substring(1).Split('.');
                        string fromNodeId = parts[0];
                        if (!results.ContainsKey(fromNodeId))
                            throw new Exception($"Node '{node.Id}' depends on result of '{fromNodeId}', which is not executed");

                        inputs[inputName] = results[fromNodeId];
                    }
                    else
                    {
                        inputs[inputName] = linkOrValue;
                    }
                }

                NodeBase nodeInstance = _factory.Create(node.Type);

                var result = await nodeInstance.ExecuteAsync(inputs);

                results[nodeId] = result;
            }

            string lastNodeId = sorted.Last();
            return results[lastNodeId];
        }
    }
}
