using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public int ID => _ID;

    [SerializeField]
    private int _ID;
}
