using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpLine.Services.Migrations.Application.Utils
{
    internal class ExecutionGraph
    {
        public class Node
        {
            public MigrationDescriptor Descriptor { get; }
            private List<Node> _parents = new List<Node>();
            public IEnumerable<Node> Parents => _parents;

            private List<Node> _children = new List<Node>();
            public IEnumerable<Node> Children => _children;

            public Node(MigrationDescriptor descriptor)
            {
                Descriptor = descriptor;
            }

            internal void AddChild(Node node)
            {
                _children.Add(node);
                node._parents.Add(this);
            }

            internal void AddParent(Node node)
            {
                node.AddChild(this);
            }
        }

        private List<Node> _nodes = new List<Node>();
        public IEnumerable<Node> Nodes => _nodes.AsReadOnly();

        public ExecutionGraph(IEnumerable<MigrationDescriptor> descriptors)
        {
            foreach (var migrationDescriptor in descriptors)
                _nodes.Add(new Node(migrationDescriptor));

            var typeToNode = _nodes.ToDictionary(x => x.Descriptor.Type, x => x);
            foreach (var node in _nodes)
            {
                foreach (var dependType in node.Descriptor.DependOn)
                {
                    if (typeToNode.TryGetValue(dependType, out var parentNode))
                    {
                        parentNode.AddChild(node);
                    }
                }
            }

            if (_nodes.All(x => x.Parents.Any()))
            {
                throw new ArgumentException("Infinity migrations... There is cycle!");
            }
        }

        public async Task Traverse(Func<MigrationDescriptor, Task<bool>> visitor)
        {
            var roots = _nodes.Where(x => !x.Parents.Any());
            await Traverse(visitor, roots.ToArray());
        }

        public async Task<bool> Traverse(MigrationDescriptor descriptor, Func<MigrationDescriptor, Task<bool>> visitor)
        {
            var nodeForDescriptor = _nodes.FirstOrDefault(x => x.Descriptor == descriptor);
            if (nodeForDescriptor == null) return false;
            await Traverse(visitor, nodeForDescriptor);
            return true;
        }

        private async Task Traverse(Func<MigrationDescriptor, Task<bool>> visitor, params Node[] nodes)
        {
            var visited = new HashSet<Node>();
            foreach (var node in nodes)
                await Visit(node, visitor, visited);
        }

        private async Task Visit(Node node, Func<MigrationDescriptor, Task<bool>> visitor, HashSet<Node> visited)
        {
            if (node.Parents.Any() && !node.Parents.All(visited.Contains))
                return;

            var success = await visitor(node.Descriptor);
            if (success)
            {
                visited.Add(node);
                foreach (var nodeChild in node.Children)
                {
                    await Visit(nodeChild, visitor, visited);
                }
            }
        }
    }
}
