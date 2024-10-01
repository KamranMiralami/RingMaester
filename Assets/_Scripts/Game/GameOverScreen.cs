using DG.Tweening;
using RingMaester;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : PanelSystem.Panel
{
    [Header("References")]
    [SerializeField] Button GoMainMenuBtn;
    [SerializeField] Button ShareBtn;
    [SerializeField] TextMeshProUGUI highScoreTxt;
    [SerializeField] Image BGImage;
    [SerializeField] TextMeshProUGUI curScoreTxt;
    [SerializeField] TextMeshProUGUI newHighScore;
    public override void Init()
    {

    }

    protected override void OnCloseFinished()
    {
        gameObject.SetActive(false);
    }

    protected override void OnCloseStarted()
    {
        GoMainMenuBtn.onClick.RemoveAllListeners();
    }

    protected override void OnOpenFinished()
    {

    }

    protected override void OnOpenStarted()
    {
        gameObject.SetActive(true);
        GoMainMenuBtn.onClick.RemoveAllListeners();
        GoMainMenuBtn.onClick.AddListener(() =>
        {
            GoMainMenuBtn.transform.DOKill(true);
            GoMainMenuBtn.transform.DOPunchScale(GoMainMenuBtn.transform.localScale * 0.2f, 0.2f);
            SceneManager.LoadSceneAsync(SceneNames.Instance.MainMenuSceneName);
        });
        ShareBtn.onClick.RemoveAllListeners();
        ShareBtn.onClick.AddListener(() =>
        {
            ShareBtn.transform.DOKill(true);
            ShareBtn.transform.DOPunchScale(ShareBtn.transform.localScale*0.2f,0.2f);
            AndroidShare.ShareText("Hey guys! I have " + highScoreTxt.text+" Score!");
        });
    }

    public void Repaint(int curScore,Color BGColor)
    {
        var highScore = PlayerPrefs.GetInt("HighScore",0);
        if (curScore > highScore)
        {
            highScore = curScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            newHighScore.gameObject.SetActive(true);
        }
        else
        {
            newHighScore.gameObject.SetActive(false);
        }
        BGImage.color = BGColor;
        curScoreTxt.text=curScore.ToString();
        highScoreTxt.text=highScore.ToString();
        PlayerPrefs.SetInt("LastScore",curScore);
    }
}
