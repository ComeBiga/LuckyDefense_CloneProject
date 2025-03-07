using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance => mInstance;
    private static UIManager mInstance = null;

    [SerializeField]
    private TextMeshProUGUI _txtRemainWaveTime;
    [SerializeField]
    private TextMeshProUGUI _txtCurrentWave;
    [SerializeField]
    private TextMeshProUGUI _txtGoldCount;
    [SerializeField]
    private TextMeshProUGUI _txtDiaCount;
    [SerializeField]
    private TextMeshProUGUI _txtSummonHeroPrice;
    [SerializeField]
    private TextMeshProUGUI _txtMonsterCount;
    [SerializeField]
    private TextMeshProUGUI _txtNextWaveTimer;

    public void SetRemainWaveTime(float remainWaveTime)
    {
        int minutes = Mathf.FloorToInt(remainWaveTime / 60);
        int seconds = Mathf.FloorToInt(remainWaveTime % 60);

        _txtRemainWaveTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetCurrentWave(int currentWave)
    {
        _txtCurrentWave.text = $"Wave {currentWave}";
    }

    public void SetGoldCount(int count)
    {
        _txtGoldCount.text = $"Gold {count}";
    }

    public void SetDiaCount(int count)
    {
        _txtDiaCount.text = $"Dia {count}";
    }

    public void SetSummonHeroPrice(int price)
    {
        _txtSummonHeroPrice.text = $"[{price}]";
    }

    public void SetMonsterCount(int count)
    {
        _txtMonsterCount.text = $"{count}";
    }

    public void SetNextWaveTimer(bool value, float time)
    {
        _txtNextWaveTimer.gameObject.SetActive(value);

        int seconds = Mathf.FloorToInt(time % 60);

        _txtNextWaveTimer.text = $"{seconds}";
    }

    private void Awake()
    {
        if(mInstance == null)
        {
            mInstance = this;
        }
    }
}
