using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCanvas : MonoBehaviour
{
    [SerializeField]
    private Slider _sliderHPBar;

    public void SetHPBar(int currentHP, int maxHP)
    {
        float hpRate = (float)currentHP / (float)maxHP;

        _sliderHPBar.value = hpRate;
    }
}
