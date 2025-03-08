using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public event Action onAttack = null;

    public void OnAttackEvent()
    {
        onAttack?.Invoke();
    }
}
