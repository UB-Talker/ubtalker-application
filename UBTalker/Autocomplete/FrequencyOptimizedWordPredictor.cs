using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBTalker.Autocomplete
{
    /// <summary>
    /// Trie node for word prediction
    /// </summary>
    class Node
    {
        public char Value { get; }
        public IList<Node> Children { get; }
        public Node Parent { get; }
        public int Depth { get; }
        public int Rank { get; set; }
        
        /// <summary>
        /// Create a new Node
        /// </summary>
        /// <param name="value">Character for this node</param>
        /// <param name="depth">How deep this node is in the Trie</param>
        /// <param name="parent">The parent node</param>
        /// <param name="rank">Rank</param>
        public Node(char value, int depth, Node parent, int rank)
        {
            Value = value;
            Children = new List<Node>();
            Depth = depth;
            Parent = parent;
            Rank = rank;
        }

        /// <summary>
        /// Determine if this node is a leaf
        /// </summary>
        /// <returns>If this node is a leaf or not</returns>
        public bool IsLeaf()
        {
            return Children.Count == 0;
        }

        /// <summary>
        /// Find a child node, if one exists
        /// </summary>
        /// <param name="c">Value of child</param>
        /// <returns>Child if exists, or null</returns>
        public Node FindChildNode(char c)
        {
            foreach (var child in Children)
            {
                if (child.Value == c)
                    return child;
            }

            return null;
        }
    }

    /// <summary>
    /// Trie data structure for word prediction
    /// </summary>
    class Trie
    {
        private static readonly char VALUE_ROOT = '\0';
        private static readonly char VALUE_TERMINATING = '\0';

        private readonly Node _root;

        /// <summary>
        /// Create a new Trie
        /// </summary>
        public Trie()
        {
            _root = new Node(VALUE_ROOT, 0, null, 0);
        }

        /// <summary>
        /// Get the deepest node for a given prefix
        /// </summary>
        /// <param name="s">Prefix to search by</param>
        /// <returns>Deepest node for given prefix</returns>
        private Node Prefix(string s)
        {
            var currentNode = _root;
            var result = currentNode;

            foreach (var c in s)
            {
                currentNode = currentNode.FindChildNode(c);
                if (currentNode == null)
                    break;
                result = currentNode;
            }

            return result;
        }

        /// <summary>
        /// Get the deepest node for a given prefix, while updating the rank
        /// Rank will only be updated if the new rank is smaller than the current rank
        /// </summary>
        /// <param name="s">Prefix to search by</param>
        /// <param name="rank">Rank to update to</param>
        /// <returns></returns>
        private Node PrefixWithRank(string s, int rank)
        {
            var currentNode = _root;
            var result = currentNode;

            foreach (var c in s)
            {
                currentNode.Rank = Math.Min(currentNode.Rank, rank);
                currentNode = currentNode.FindChildNode(c);
                if (currentNode == null)
                    break;
                result = currentNode;
            }

            result.Rank = Math.Min(result.Rank, rank);
            return result;
        }

        /// <summary>
        /// Find all results for a given string
        /// </summary>
        /// <param name="s">Prefix to search by</param>
        /// <returns>All matches for the given string</returns>
        public IList<string> Match(string s)
        {
            IList<string> result = new List<string>();

            Node prefix = Prefix(s);
            DepthFirstSearch(prefix, s, ref result);

            return result;
        }

        /// <summary>
        /// Find all words derived from a given root node
        /// </summary>
        /// <param name="node">Root node</param>
        /// <param name="path">Current string path</param>
        /// <param name="result">The list of strings derived from the given node</param>
        private void DepthFirstSearch(Node node, string path, ref IList<string> result)
        {
            foreach (Node child in node.Children.OrderBy(n => n.Rank))
            {
                if (child.IsLeaf())
                {
                    result.Add(path);
                }
                else
                {
                    DepthFirstSearch(child, path + child.Value, ref result);
                }
            }
        }

        /// <summary>
        /// Insert a string with a given rank
        /// </summary>
        /// <param name="s">String to insert</param>
        /// <param name="rank">Rank to insert with</param>
        public void InsertWithRank(string s, int rank)
        {
            var commonPrefix = PrefixWithRank(s, rank);
            var current = commonPrefix;

            for (var i = current.Depth; i < s.Length; i++)
            {
                var newNode = new Node(s[i], current.Depth + 1, current, rank);
                current.Children.Add(newNode);
                current = newNode;
            }

            current.Children.Add(new Node(VALUE_TERMINATING, current.Depth + 1, current, 0));
        }
    }

    /// <summary>
    /// Word predictor that predicts based on frequency ranking
    /// </summary>
    class FrequencyOptimizedWordPredictor : IWordPredictor
    {
        private Trie trie;

        public FrequencyOptimizedWordPredictor()
        {
            trie = new Trie();
        }

        /// <summary>
        /// Create a new word predictor with a given frequency list
        /// </summary>
        /// <param name="file">Path of frequency list file</param>
        public FrequencyOptimizedWordPredictor(string file)
        {
            trie = new Trie();
            PopulateTrie(file);
        }

        /// <summary>
        /// Populate the internal trie with the given frequency list file
        /// </summary>
        /// <param name="file">Path of frequency list file</param>
        private void PopulateTrie(string file)
        {
            foreach (string line in System.IO.File.ReadAllLines(file))
            {
                var parts = line.Split('\t');
                int frequency = Int32.Parse(parts[0]);
                string word = parts[1];
                trie.InsertWithRank(word, frequency);
            }
        }

        public ICollection<string> SuggestTop(string prefix, int n)
        {
            return trie.Match(prefix).Take(n).ToList();
        }
    }
}
