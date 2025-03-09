using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance => mInstance;
    private static UIManager mInstance = null;

    public TextMeshProUGUI _txtRemainWaveTime;
    public TextMeshProUGUI _txtCurrentWave;
    public TextMeshProUGUI _txtGoldCount;
    public TextMeshProUGUI _txtDiaCount;
    public TextMeshProUGUI _txtSummonHeroPrice;
    public TextMeshProUGUI _txtMonsterCount;
    public TextMeshProUGUI _txtNextWaveTimer;

    public Button btnSummonHero;
    public Button btnGamblingUI;
    public Button btnNormalGamble;
    public Button btnHeroGamble;
    public Button btnMythGamble;
    public Button btnComposeHero;
    public Button btnSellHero;
    public Button btnComposeMythHero;

    public Slider sliderMonsterCount;

    public GameObject goGamblingUI;
    public GameObject goNextWaveTimer;

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
        _txtGoldCount.text = $"{count}";
    }

    public void SetDiaCount(int count)
    {
        _txtDiaCount.text = $"{count}";
    }

    public void SetSummonHeroPrice(int price)
    {
        _txtSummonHeroPrice.text = $"{price}";
    }

    public void SetMonsterCount(int count)
    {
        _txtMonsterCount.text = $"{count}/100";

        sliderMonsterCount.value = count / 100f ;
    }

    public void SetNextWaveTimer(bool value, float time)
    {
        //_txtNextWaveTimer.gameObject.SetActive(value);
        goNextWaveTimer.gameObject.SetActive(value);

        int seconds = Mathf.FloorToInt(time % 60);

        _txtNextWaveTimer.text = $"{seconds}";
    }

    public void ShowGamblingUI()
    {
        goGamblingUI.SetActive(true);
    }

    public void HideGamblingUI()
    {
        goGamblingUI.SetActive(false);
    }

    private void Awake()
    {
        if(mInstance == null)
        {
            mInstance = this;
        }
    }
}
