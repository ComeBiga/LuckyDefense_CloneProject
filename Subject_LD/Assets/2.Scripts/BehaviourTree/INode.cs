using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public interface INode
    {
        public enum EState { Running, Success, Failure }

        public EState Evaluate();
    }
}
