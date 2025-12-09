using System.Text.Json;
using Serilog;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.NodeSystem;
using WebApiTrainingProject.NodeSystem.Base;
using WebApiTrainingProject.NodeSystem.Models;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services
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
            try
            {
                Log.Information("Starting graph execution");

                NodeGraphModel graph;
                try
                {
                    graph = JsonSerializer.Deserialize<NodeGraphModel>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? throw new Exception("Invalid graph JSON");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to deserialize graph JSON");
                    throw;
                }

                var indegree = new Dictionary<string, int>();
                foreach (var node in graph.Nodes)
                    if (!indegree.TryAdd(node.Id, 0))
                    {
                        Log.Warning("Duplicate node ID detected: {NodeId}", node.Id);
                    }

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
                {
                    Log.Error("Cycle detected in graph. Nodes: {NodeCount}, Sorted: {SortedCount}",
                              graph.Nodes.Count, sorted.Count);
                    throw new Exception("Cycle detected in graph");
                }

                Log.Information("Topological sort completed successfully. Execution order: {Order}",
                                string.Join(", ", sorted));

                var results = new Dictionary<string, object?>();

                foreach (var nodeId in sorted)
                {
                    var node = graph.Nodes.First(n => n.Id == nodeId);

                    Log.Information("Executing node {NodeId} of type {NodeType}", node.Id, node.Type);

                    var inputs = new Dictionary<NodeInputKey, object>();

                    foreach (var inputPair in node.Inputs)
                    {
                        string inputName = inputPair.Key;
                        string linkOrValue = inputPair.Value;
                        if (!Enum.TryParse<NodeInputKey>(inputName, true, out var inputKey))
                        {
                            Log.Error("Unknown input key '{InputName}' for node {NodeId}", inputName, node.Id);
                            throw new Exception($"Unknown input key: {inputName}");
                        }

                        if (linkOrValue.StartsWith("$"))
                        {
                            var parts = linkOrValue.Substring(1).Split('.');
                            string fromNodeId = parts[0];

                            if (!results.ContainsKey(fromNodeId))
                            {
                                Log.Error("Node {NodeId} depends on {FromNodeId} but it has no result",
                                          node.Id, fromNodeId);
                                throw new Exception($"Node '{node.Id}' depends on result of '{fromNodeId}', which is not executed");
                            }

                            inputs[inputKey] = results[fromNodeId];
                        }
                        else
                        {
                            inputs[inputKey] = linkOrValue;
                        }
                    }

                    NodeBase nodeInstance;

                    try
                    {
                        nodeInstance = _factory.Create(node.Type);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Failed to create node instance of type {NodeType}", node.Type);
                        throw;
                    }

                    try
                    {
                        var result = await nodeInstance.ExecuteAsync(inputs);
                        if (!results.TryAdd(nodeId, result))
                        {
                            Log.Error("Result for node {NodeId} already exists. Duplicate execution?", nodeId);
                            throw new Exception($"Duplicate node execution detected: {nodeId}");
                        }


                        Log.Information("Node {NodeId} executed successfully", nodeId);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Node {NodeId} execution failed", nodeId);
                        throw;
                    }
                }

                string lastNodeId = sorted.Last();

                Log.Information("Graph executed successfully. Final node: {NodeId}", lastNodeId);

                return results[lastNodeId];
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Graph execution failed");
                throw;
            }
        }
    }
}
