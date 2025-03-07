using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public enum EGrade { Normal, Rare, Hero, Myth }

    public int ID => _ID;
    public EGrade Grade => _grade;

    [SerializeField]
    private int _ID;
    [SerializeField]
    private EGrade _grade;
}
