using DG.Tweening;
using RingMaester;
using SFXSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : PanelSystem.Panel
{
    [Header("References")]
    [SerializeField] Button closeBtn;
    [SerializeField] Button exitBGBtn;
    [SerializeField] Button exportBtn;
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
        exportBtn.onClick.RemoveAllListeners();
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
            SoundSystemManager.Instance.ChangeBGMVolumn(Settings.Instance.MusicMult);
        });
        VibrationSlider.onValueChanged.RemoveAllListeners();
        VibrationSlider.onValueChanged.AddListener((value) =>
        {
            Settings.Instance.VibrationMult = value;
        });

        exportBtn.onClick.RemoveAllListeners();
        exportBtn.onClick.AddListener(ExportToJSON);
    }

    protected override void OnOpenStarted()
    {
        gameObject.SetActive(true);
        SFXSlider.value = Settings.Instance.SFXMult;
        MusicSlider.value = Settings.Instance.MusicMult;
        VibrationSlider.value = Settings.Instance.VibrationMult;
    }
    void ExportToJSON()
    {
        var name = PlayerPrefs.GetString("Name", "kamran");
        var lastScore = PlayerPrefs.GetInt("LastScore", 0);
        var hScore = PlayerPrefs.GetInt("HighScore", 0);
        var cRank = LeaderboardSettings.Instance.NumberOfRandomNumbers - LeaderBoardPanel.Instance.PlayerIndex;
        cRank++;
        PlayerData player = new PlayerData(name,lastScore,hScore,cRank);
        string json = JsonUtility.ToJson(player);
        string path = Application.persistentDataPath + "/playerData.json";
        File.WriteAllText(path, json);
        GameDebug.Log(json);
    }
}
public struct PlayerData
{
    public string name;
    public int lastScore;
    public int hScore;
    public int cRank;
    public float SFXMult;
    public float MusicMult;
    public float VibrationMult;
    public PlayerData(string name,int lastScore,int hScore,int cRank)
    {
        this.name = name;
        this.lastScore = lastScore;
        this.hScore = hScore;
        this.cRank = cRank;
        SFXMult = Settings.Instance.SFXMult;
        MusicMult = Settings.Instance.MusicMult;
        VibrationMult = Settings.Instance.VibrationMult;
    }
}