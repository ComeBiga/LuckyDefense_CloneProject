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
    public TextMeshProUGUI txtComposableMythHeroCount;
    public TextMeshProUGUI txtGamblingResult;

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
    public GameObject goGameOverUI;

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

    public void SetMonsterCount(int count, int max)
    {
        _txtMonsterCount.text = $"{count}/{max}";

        sliderMonsterCount.value = (float)count / max ;
    }

    public void SetNextWaveTimer(bool value, float time)
    {
        //_txtNextWaveTimer.gameObject.SetActive(value);
        goNextWaveTimer.gameObject.SetActive(value);

        int seconds = Mathf.FloorToInt(time % 60);

        _txtNextWaveTimer.text = $"{seconds}";
    }
    
    public void SetComposableMythHeroCount(int count)
    {
        txtComposableMythHeroCount.text = $"{count}";
    }

    public void ShowGamblingUI()
    {
        goGamblingUI.SetActive(true);
    }

    public void HideGamblingUI()
    {
        goGamblingUI.SetActive(false);
    }

    public void ShowGamblingResult(bool result)
    {
        txtGamblingResult.text = result ? "¿î»¡ »Ì±â ¼º°ø!" : "¿î»¡ »Ì±â ½ÇÆÐ..";

        StartCoroutine(eShowGamblingResult());
    }

    private void Awake()
    {
        if(mInstance == null)
        {
            mInstance = this;
        }
    }

    private IEnumerator eShowGamblingResult()
    {
        txtGamblingResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        txtGamblingResult.gameObject.SetActive(false);
    }
}
