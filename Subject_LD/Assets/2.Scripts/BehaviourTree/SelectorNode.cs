using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SelectorNode : INode
    {
        private List<INode> _childs;

        public SelectorNode(List<INode> childs)
        {
            _childs = childs;
        }

        public INode.EState Evaluate()
        {
            if (_childs == null)
                return INode.EState.Failure;

            foreach (var child in _childs)
            {
                switch (child.Evaluate())
                {
                    case INode.EState.Running:
                        return INode.EState.Running;
                    case INode.EState.Success:
                        return INode.EState.Success;
                }
            }

            return INode.EState.Failure;
        }
    }
}
