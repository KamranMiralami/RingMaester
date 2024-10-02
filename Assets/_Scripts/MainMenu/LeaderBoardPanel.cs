using Cysharp.Threading.Tasks;
using PanelSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
                //var profile = Instantiate(MainMenuResourceHolder.Instance.LeaderBoardProfilePrefab, listParent);
                var profile = Splash.LeaderBoardProfileObjectPool.Get();
                profile.transform.SetParent(listParent);
                profile.transform.localScale = Vector3.one;
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
            var playerHScore = PlayerPrefs.GetInt("HighScore",0);
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
            Debug.Log(scrollRect.verticalNormalizedPosition);
            currentScrollCD -= Time.deltaTime;
            if (currentScrollCD > 0) return;
            if (scrollRect.verticalNormalizedPosition < 0.1f)
            {
                currentScrollCD = defaultScrollCD;
                LoadMoreItems();
            }
            else if (scrollRect.verticalNormalizedPosition > 0.9f)
            {
                currentScrollCD = defaultScrollCD;
                RemoveExtraItems();
            }
        }

        private void RemoveExtraItems()
        {
            if (firstVisibleItemIndex > 0)
            {
                firstVisibleItemIndex-= 20;
                //UpdateItemsDownward();
            }
        }

        private void LoadMoreItems()
        {
            if (firstVisibleItemIndex + visibleItems < totalItems)
            {
                firstVisibleItemIndex+= 20;
                //UpdateItemsForward();
            }
        }

        async UniTaskVoid UpdateItemsForward()
        {
            for (int i = 0; i < 20; i++)
            {
                if (firstVisibleItemIndex + visibleItems < totalItems)
                {
                    var itemIndex = randomNumbers.Count-1 - (firstVisibleItemIndex-10 + visibleItems + i);
                    var item = items[0];
                    items[0].Repaint(randomNumbers[itemIndex],
                        randomStrings[itemIndex]);
                    items[0].transform.SetParent(null);
                    await UniTask.Yield();
                    AddItems(items[0]);
                    items[0].gameObject.SetActive(true);
                    items.RemoveAt(0);
                    items.Add(item);
                }
                else
                {
                    items[0].gameObject.SetActive(false);
                }
            }
        }
        async UniTaskVoid UpdateItemsDownward()
        {
            for (int i = 0; i < 20; i++)
            {
                var itemIndex = randomNumbers.Count-1  - (firstVisibleItemIndex+10) + i;
                var item = items[items.Count -1];
                items[items.Count - 1].Repaint(randomNumbers[itemIndex],
                    randomStrings[itemIndex]);
                items[items.Count- 1].transform.SetParent(null);
                await UniTask.Yield();
                AddItems(items[items.Count - 1],true);
                items[items.Count - 1].gameObject.SetActive(true);
                items.RemoveAt(items.Count - 1);
                items.Insert(0,item);
            }
        }
        public void AddItems(LeaderBoardProfile newItem, bool isFirstChild=false)
        {
            float mult = 1;
            if (isFirstChild) mult = -1;
            float prevScrollPosition = scrollRect.verticalNormalizedPosition;
            float prevContentHeight = listParentRect.rect.height;
            newItem.transform.SetParent(listParent);
            if (mult<0)
            {
                newItem.transform.SetAsFirstSibling();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(listParentRect);
            float newContentHeight = listParentRect.rect.height;
            float heightDifference = newContentHeight - prevContentHeight;
            if (newContentHeight > scrollRect.viewport.rect.height)
            {
                //float newScrollPosition = prevScrollPosition + (heightDifference / newContentHeight)*mult;
                //newScrollPosition += 0.02f * mult;
                scrollRect.verticalNormalizedPosition += 0.02215094f * mult;
            }
        }
    }
}