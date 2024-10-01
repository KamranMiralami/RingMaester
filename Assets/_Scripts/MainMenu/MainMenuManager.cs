using DG.Tweening;
using PanelSystem;
using SFXSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RingMaester
{
    public class MainMenuManager : SingletonBehaviour<MainMenuManager>
    {
        public enum Panels
        {
            Play,
            Shop,
            Leaderboard,
        }
        [Header("Properties")]
        [SerializeField] Panels defaultPanel;
        [Header("References")]
        [SerializeField] Button PlayPanelBtn;
        [SerializeField] Button ShopPanelBtn;
        [SerializeField] Button LeaderboardPanelBtn;
        [SerializeField] Button SettingsPanelBtn;
        [SerializeField] Panel LeaderboardPanel;
        [SerializeField] Panel PlayPanel;
        [SerializeField] Panel ShopPanel;
        [SerializeField] Panel SettingsPanel;

        Panel curPanel;
        private void Start()
        {
            SetupButtons();
            InitializePanels();
            OpenPanel(defaultPanel);
        }

        private void InitializePanels()
        {
            LeaderboardPanel.Init();
            PlayPanel.Init();
            ShopPanel.Init();
            SettingsPanel.Init();
        }

        void SetupButtons()
        {
            PlayPanelBtn.onClick.RemoveAllListeners();
            ShopPanelBtn.onClick.RemoveAllListeners();
            LeaderboardPanelBtn.onClick.RemoveAllListeners();
            SettingsPanelBtn.onClick.RemoveAllListeners();
            PlayPanelBtn.onClick.AddListener(() => OpenPanel(Panels.Play));
            ShopPanelBtn.onClick.AddListener(() => OpenPanel(Panels.Shop));
            LeaderboardPanelBtn.onClick.AddListener(() => OpenPanel(Panels.Leaderboard));
            SettingsPanelBtn.onClick.AddListener(() =>
            {
                SettingsPanelBtn.transform.DOKill(true);
                SettingsPanelBtn.transform.DOPunchScale(SettingsPanelBtn.transform.localScale * 0.1f, 0.1f);
                SettingsPanel.Open();
            });
        }
        public void OpenPanel(Panels panelType)
        {
            var panel = GetPanel(panelType);
            if (panel == null)
            {
                GameDebug.Log("We have no such panel");
                return;
            }
            if (panel == curPanel) return;
            SetFooterButton(curPanel, true);
            curPanel?.Close();
            curPanel = panel;
            curPanel.Open();
            SetFooterButton(curPanel, false);
        }

        public void SetFooterButton(Panel type, bool close)
        {
            Button b = null;
            if (type == PlayPanel)
            {
                b = PlayPanelBtn;
            }
            if (type == ShopPanel)
            {
                b = ShopPanelBtn;
            }
            if (type == LeaderboardPanel)
            {
                b = LeaderboardPanelBtn;
            }
            if (b == null) return;
            var rect = b.GetComponent<RectTransform>();
            rect.DOComplete(true);
            var text = b.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            Vector2 size = rect.sizeDelta;
            var dif = close ? -25 : 25;
            rect.DOSizeDelta(new Vector2(size.x, size.y + dif), .25f);
            text.color = dif == -25 ? MainMenuResourceHolder.Instance.DeselectedColor : MainMenuResourceHolder.Instance.SelectedColor;
            SoundSystemManager.Instance.PlaySFX("Hover");
        }

        Panel GetPanel(Panels type)
        {
            switch (type)
            {
                case Panels.Play: return PlayPanel;
                case Panels.Shop: return ShopPanel;
                case Panels.Leaderboard: return LeaderboardPanel;
                default: return null;
            }
        }
    }
}