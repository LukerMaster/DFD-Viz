namespace DFD.DataStructures.Interfaces
{
    public interface INodeRef<T>
    {
        T Data { get; }
        string Name { get; }
        INodeRef<T>? Parent { get; }
        IReadOnlyCollection<INodeRef<T>> Children { get; }
        string FullPath { get; }
        string HexHash { get; }

        /// <summary>
        /// Find node matching path given. In case <paramref name="fullOrPartialNodePath"/> is not full,
        /// it may throw <see cref="AmbiguousNodeMatchException{T}"/> if
        /// two nodes match given name.
        /// </summary>
        /// <param name="fullOrPartialNodePath"></param>
        /// <param name="leavesOnly">Only look for leaves (no child nodes).</param>
        /// <returns>Node that matches the <paramref name="fullOrPartialNodePath"/>.</returns>
        /// <exception cref="AmbiguousNodeMatchException{T}"></exception>
        /// <exception cref="NodeNotFoundException"></exception>
        public INodeRef<T> FindMatchingNode(string fullOrPartialNodePath, bool leavesOnly = false)
        {
            List<INodeRef<T>> candidates = new();

            Queue<INodeRef<T>> queue = new Queue<INodeRef<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                INodeRef<T> currentNode = queue.Dequeue();

                // Check if the current node contains the target value
                if (
                    (currentNode.CanNameBeThisNode(fullOrPartialNodePath) && !leavesOnly) ||
                    (currentNode.CanNameBeThisNode(fullOrPartialNodePath) && currentNode.Children.Count == 0)
                    )
                {
                    candidates.Add(currentNode);
                }

                // If we already found matching leaf, do not enqueue lower levels
                // We just need to check whether there is any other node
                // matching on THE SAME level, so we can throw ambiguity error.
                // Does not apply when looking for any node, not just a leaf.
                if (candidates.Count == 0 || !leavesOnly)
                {
                    // Enqueue children for further exploration
                    foreach (INodeRef<T> child in currentNode.Children)
                    {
                        queue.Enqueue(child);
                    }
                }

            }

            if (candidates.Count == 1)
                return candidates[0];
            if (candidates.Count > 1)
                throw new AmbiguousNodeMatchException<T>(fullOrPartialNodePath, candidates.ToArray());
            // Target value not found in the tree
            throw new NodeNotFoundException(fullOrPartialNodePath, this.FullPath);
        }


        /// <summary>
        /// Find node matching hexhash given.
        /// </summary>
        /// <param name="hexHash"></param>
        /// <returns>Node that matches the <paramref name="hexHash"/>.</returns>
        /// <exception cref="AmbiguousNodeMatchException{T}"></exception>
        /// <exception cref="NodeNotFoundException"></exception>
        public INodeRef<T>? FindMatchingNode(string hexHash)
        {
            Queue<INodeRef<T>> queue = new Queue<INodeRef<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                INodeRef<T> currentNode = queue.Dequeue();

                // Check if the current node contains the target value
                if (currentNode.HexHash == hexHash)
                {
                    return currentNode;
                }

                // Enqueue children for further exploration
                foreach (INodeRef<T> child in currentNode.Children)
                {
                    queue.Enqueue(child);
                }
            }
            return null;
        }
        public bool CanNameBeThisNode(string nodeName)
        {
            // Check whether two nodes have matching names
            // Example: Namespace.Process.Subprocess
            //                    Process.Subprocess
            //                    ^ Matching

            // Namespace.Process.Subprocess
            //     Other.Process.Subprocess
            //     ^ Not matching

            var firstSections = this.FullPath.Split('.');
            var secondSections = nodeName.Split('.');

            // Reverse so we check from the innermost
            firstSections = firstSections.Reverse().ToArray();
            secondSections = secondSections.Reverse().ToArray();

            var checkLimit = Math.Min(firstSections.Length, secondSections.Length);

            for (int i = 0; i < checkLimit; i++)
            {
                if (firstSections[i] != secondSections[i])
                    return false;
            }
            return true;
        }

        public INodeRef<T>? FindEarliestAncestorThatMatches(Func<T, bool> predicate)
        {
            var stack = new Stack<INodeRef<T>>();

            var currentNode = this;

            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;

                if (predicate(currentNode.Data))
                {
                    stack.Push(currentNode);
                }
            }

            return stack.Peek();
        }

        public int GetAncestorCount()
        {
            var count = 0;
            var currentNode = this;
            while (currentNode.Parent is not null)
            {
                currentNode = currentNode.Parent;
                count++;
            }
            return count;
        }
    }
}
