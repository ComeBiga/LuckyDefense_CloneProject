using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SequenceNode : INode
    {
        private List<INode> _childs;

        public SequenceNode(List<INode> childs)
        {
            _childs = childs;
        }

        public INode.EState Evaluate()
        {
            if (_childs == null || _childs.Count == 0)
                return INode.EState.Failure;

            foreach (var child in _childs)
            {
                switch (child.Evaluate())
                {
                    case INode.EState.Running:
                        return INode.EState.Running;
                    case INode.EState.Success:
                        continue;
                    case INode.EState.Failure:
                        return INode.EState.Failure;
                }
            }

            return INode.EState.Success;
        }
    }
}
