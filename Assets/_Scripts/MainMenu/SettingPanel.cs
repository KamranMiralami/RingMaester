using DG.Tweening;
using RingMaester;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : PanelSystem.Panel
{
    [Header("References")]
    [SerializeField] Button closeBtn;
    [SerializeField] Button exitBGBtn;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider VibrationSlider;
    public override void Init()
    {
    }

    protected override void OnCloseFinished()
    {
        gameObject.SetActive(false);
    }

    protected override void OnCloseStarted()
    {
        exitBGBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        GameDebug.Log("Values in settings is "+Settings.Instance.SFXMult+" "+Settings.Instance.MusicMult+" "+Settings.Instance.VibrationMult);
        PlayerPrefs.SetFloat("SFXMult",Settings.Instance.SFXMult);
        PlayerPrefs.SetFloat("MusicMult", Settings.Instance.MusicMult);
        PlayerPrefs.SetFloat("VibrationMult", Settings.Instance.VibrationMult);
    }

    protected override void OnOpenFinished()
    {
        exitBGBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        exitBGBtn.onClick.AddListener(Close);
        closeBtn.onClick.AddListener(() =>
        {
            closeBtn.transform.DOKill(true);
            closeBtn.transform.DOPunchScale(closeBtn.transform.localScale * 0.1f, 0.1f);
            Close();
        });
        SFXSlider.onValueChanged.RemoveAllListeners();
        SFXSlider.onValueChanged.AddListener((value) =>
        {
            Settings.Instance.SFXMult=value;
        });
        MusicSlider.onValueChanged.RemoveAllListeners();
        MusicSlider.onValueChanged.AddListener((value) =>
        {
            Settings.Instance.MusicMult = value;
        });
        VibrationSlider.onValueChanged.RemoveAllListeners();
        VibrationSlider.onValueChanged.AddListener((value) =>
        {
            Settings.Instance.VibrationMult = value;
        });
    }

    protected override void OnOpenStarted()
    {
        gameObject.SetActive(true);
        SFXSlider.value = Settings.Instance.SFXMult;
        MusicSlider.value = Settings.Instance.MusicMult;
        VibrationSlider.value = Settings.Instance.VibrationMult;
    }
}
