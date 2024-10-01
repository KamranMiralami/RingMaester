using PanelSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace RingMaester
{
    public class LeaderBoardPanel : Panel
    {
        public int PlayerIndex;
        public string PlayerName;
        [Header("References")]
        [SerializeField] Transform listParent;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] RectTransform listParentRect;

        [Header("Properties")]
        [SerializeField] float defaultScrollCD;

        string fileNameN;
        string fileNameS;
        List<int> randomNumbers;
        List<string> randomStrings;
        private int firstVisibleItemIndex = 0;
        private List<LeaderBoardProfile> items;
        private readonly float itemHeight = 100f;
        float currentScrollCD;
        int totalItems;
        int visibleItems;
        public static LeaderBoardPanel Instance;
        public override void Init()
        {
            Instance = this;
            fileNameN = LeaderboardSettings.Instance.FileNameN;
            fileNameS = LeaderboardSettings.Instance.FileNameS;
            visibleItems = LeaderboardSettings.Instance.VisibleNumbers;
            totalItems = LeaderboardSettings.Instance.NumberOfRandomNumbers;
            PlayerName = PlayerPrefs.GetString("Name", "kamran"); // In a real world application, we would have UUID for each data point, and it would be used here instead of this
            PlayerIndex = -1;
            items = new();
            randomNumbers = new();
            randomStrings = new();
            foreach (Transform t in listParent)
            {
                Destroy(t.gameObject);
            }
            ReadRandomNumberFile();
            ReadRandomStringsFile();
            for (int i = 0; i <visibleItems; i++)
            {
                var profile = Instantiate(MainMenuResourceHolder.Instance.LeaderBoardProfilePrefab, listParent);
                profile.Repaint(randomNumbers[totalItems-i], randomStrings[totalItems - i]);
                items.Add(profile);
            }
            currentScrollCD = -1;
        }

        protected override void OnCloseFinished()
        {
            gameObject.SetActive(false);
        }

        protected override void OnCloseStarted()
        {
        }

        protected override void OnOpenFinished()
        {
        }

        protected override void OnOpenStarted()
        {
            gameObject.SetActive(true);
        }
        void ReadRandomNumberFile()
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileNameN);
            var playerHScore = 20;// PlayerPrefs.GetInt("HighScore",0);
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (int.TryParse(line, out int number))
                    {
                        randomNumbers.Add(number);
                    }
                }
            }
            else
            {
                GameDebug.LogError("File not found: " + filePath);
            }
            for (int i = randomNumbers.Count - 1; i >= 0; i--)
            {
                if (randomNumbers[i] <= playerHScore)
                {
                    if(i==randomNumbers.Count - 1) randomNumbers.Add(playerHScore);
                    else randomNumbers.Insert(i+1, playerHScore);
                    PlayerIndex = i+1;
                    break;
                }
            }

        }
        void ReadRandomStringsFile()
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileNameS);

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string currentLine = lines[i];
                    randomStrings.Add(currentLine);
                }
            }
            else
            {
                GameDebug.LogError($"File not found at: {filePath}");
            }
            if (PlayerIndex != -1)
            {
                randomStrings.Insert(PlayerIndex, PlayerPrefs.GetString("Name","kamran"));
            }
        }
        void Update()
        {
            currentScrollCD -= Time.deltaTime;
            if (currentScrollCD > 0) return;
            if (scrollRect.verticalNormalizedPosition < 0.25f)
            {
                currentScrollCD = defaultScrollCD;
                LoadMoreItems();
            }
            else if (scrollRect.verticalNormalizedPosition > 0.75f)
            {
                currentScrollCD = defaultScrollCD;
                RemoveExtraItems();
            }
        }

        private void RemoveExtraItems()
        {
            if (firstVisibleItemIndex > 0)
            {
                firstVisibleItemIndex-=10;
                UpdateItems();
            }
        }

        private void LoadMoreItems()
        {
            if (firstVisibleItemIndex + visibleItems < totalItems)
            {
                firstVisibleItemIndex+=10;
                UpdateItems();
            }
        }

        void UpdateItems()
        {
            for (int i = 0; i < visibleItems; i++)
            {
                int itemIndex = firstVisibleItemIndex + i;
                if (itemIndex < totalItems)
                {
                    items[i].gameObject.SetActive(true);
                    RectTransform itemRectTransform = items[i].Rect;
                    itemRectTransform.anchoredPosition = new Vector2(0, -itemHeight * i);
                    items[i].Repaint(randomNumbers[totalItems-itemIndex], randomStrings[totalItems-itemIndex]);
                }
                else
                {
                    items[i].gameObject.SetActive(false);
                }
            }
            listParentRect.sizeDelta = new Vector2(listParentRect.sizeDelta.x, itemHeight * totalItems);
        }
    }
}