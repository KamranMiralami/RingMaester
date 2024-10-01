using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RingMaester
{
    public class PlayPanel : PanelSystem.Panel
    {
        [Header("References")]
        [SerializeField] Button StartBtn;
        [SerializeField] TextMeshProUGUI highScoreTxt;
        public override void Init()
        {
        }

        protected override void OnCloseFinished()
        {
            gameObject.SetActive(false);
        }

        protected override void OnCloseStarted()
        {
            StartBtn.onClick.RemoveAllListeners();
        }

        protected override void OnOpenFinished()
        {
            StartBtn.onClick.RemoveAllListeners();
            StartBtn.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            StartBtn.transform.DOKill(true);
            StartBtn.transform.DOPunchScale(StartBtn.transform.localScale*0.2f,0.2f);
            Close();
            SceneManager.LoadSceneAsync(SceneNames.Instance.GameSceneName);
        }

        protected override void OnOpenStarted()
        {
            gameObject.SetActive(true);
            highScoreTxt.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }
}