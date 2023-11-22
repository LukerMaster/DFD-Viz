namespace DataStructure.NamedTree;

public interface ITreeNode<T>
{
    ITreeNode<T>? Parent { get; }
    IReadOnlyCollection<ITreeNode<T>> Children { get; }
    public T Data { get; protected set; }
    /// <summary>
    /// Unique indentifier of the node (at this level).
    /// <see cref="NodeName"/> can repeat across different levels of the tree,
    /// however there cannot be two same ones in the same tree level.
    /// </summary>
    public string NodeName { get; }
    /// <summary>
    /// Gets all the <see cref="NodeName"/> of all the parent nodes concatenated to this
    /// with dot as a separator. This creates unique path from the root of the tree to that node.
    /// Example: topNode.someNode.someDifferentNode.thisNode
    /// </summary>
    public string FullNodeName
    {
        get
        {
            var name = NodeName;
            var currentParent = Parent;
            while (currentParent is not null)
            {
                name = currentParent.NodeName + '.' + name;
                currentParent = currentParent.Parent;
            }
            return name;
        }
    }

    public ITreeNode<T> Root
    {
        get
        {
            ITreeNode<T>? root = this;
            while (root.Parent is not null)
            {
                root = root.Parent;
            }
            return root;
        }
    }
    
    /// <summary>
    /// Copies the entire subtree with new type of data stored inside.
    /// Data conversion can be done by providing a <paramref name="dataConversionFunc"/>.
    /// </summary>
    /// <typeparam name="TNew">New type of data that will be stored in each node.</typeparam>
    /// <param name="dataConversionFunc">Function that takes in old data object and returns new data object.</param>
    /// <param name="newParent">New parent of the new node (optional).</param>
    /// <returns>New subtree with each node containing new data type converted using <paramref name="dataConversionFunc"/>.</returns>
    public ITreeNode<TNew> CopySubtreeAs<TNew>(Func<T, TNew> dataConversionFunc, ITreeNode<TNew> newParent = null);

    /// <summary>
    /// Find node matching name given. In case <paramref name="fullOrPartialNodeName"/> is not full,
    /// it may throw <see cref="AmbiguousNodeMatchException{T}"/> if
    /// two nodes match given name.
    /// </summary>
    /// <param name="fullOrPartialNodeName"></param>
    /// <param name="leavesOnly">Only look for leaves (no child nodes).</param>
    /// <returns>Node that matches the <paramref name="fullOrPartialNodeName"/>.</returns>
    /// <exception cref="AmbiguousNodeMatchException{T}"></exception>
    /// <exception cref="NodeNotFoundException"></exception>
    public ITreeNode<T> FindMatchingNode(string fullOrPartialNodeName, bool leavesOnly = false)
    {
        List<ITreeNode<T>> candidates = new();

        Queue<ITreeNode<T>> queue = new Queue<ITreeNode<T>>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            ITreeNode<T> currentNode = queue.Dequeue();

            // Check if the current node contains the target value
            if (
                (currentNode.CanNameBeThisNode(fullOrPartialNodeName) && !leavesOnly) ||
                (currentNode.CanNameBeThisNode(fullOrPartialNodeName) && currentNode.Children.Count == 0)
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
                foreach (ITreeNode<T> child in currentNode.Children)
                {
                    queue.Enqueue(child);
                }
            }

        }

        if (candidates.Count == 1)
            return candidates[0];
        if (candidates.Count > 1)
            throw new AmbiguousNodeMatchException<T>(fullOrPartialNodeName, candidates.ToArray());
        // Target value not found in the tree
        throw new NodeNotFoundException(fullOrPartialNodeName);
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

        var firstSections = this.FullNodeName.Split('.');
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
}

