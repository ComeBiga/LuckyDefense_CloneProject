using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class BehaviourTreeRunner
    {
        private INode mRootNode;

        public BehaviourTreeRunner(INode rootNode) 
        {
            mRootNode = rootNode;
        }

        public void Operate()
        {
            mRootNode.Evaluate();
        }
    }
}
