using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class ActionNode : INode
    {
        private Func<INode.EState> onUpdate = null;

        public ActionNode(Func<INode.EState> onUpdate)
        {
            this.onUpdate = onUpdate;
        }

        public INode.EState Evaluate()
        {
            return onUpdate?.Invoke() ?? INode.EState.Failure;
        }
    }
}
