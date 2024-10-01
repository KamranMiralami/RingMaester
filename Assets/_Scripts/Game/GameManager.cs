using Cysharp.Threading.Tasks;
using DG.Tweening;
using PanelSystem;
using SFXSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RingMaester.Managers
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [Header("Properties")]
        [SerializeField] float scoreBonusCD;
        [SerializeField] int scoreBonusAmount;
        [SerializeField] AnimationCurve gameSpeedCurve;
        [SerializeField] float maxGameSpeed;
        [SerializeField] float maxGameSpeedTime;
        [SerializeField] int targetStreakForBonus;
        [SerializeField] float defaultGameSpeed;
        [Header("Manager References")]
        [SerializeField] PlayerManager playerManager;
        [SerializeField] KnobManager knobManager;
        [SerializeField] RewardManager rewardManager;
        [SerializeField] GameOverScreen gameOverScreen;
        [SerializeField] QuitGamePanel QuitGamePanel;

        [Header("References")]
        [SerializeField] TextMeshProUGUI scoreTxt;
        [SerializeField] TextMeshProUGUI bonusTxt;
        [SerializeField] Image BG;
        [SerializeField] Button pauseBtn;
        [SerializeField] Image pauseImage;
        [SerializeField] GameObject gameParent;
        [HideInInspector]
        public int Score;
        [HideInInspector]
        public bool IsPaused;
        [HideInInspector]
        public float GameSpeed;
        public Action PlayerGotReward;
        public Action PlayerGotBonus;

        float curScoreBonusCD;
        bool increaseGameSpeed;
        int streak;
        float giveRewardCD = 0.2f;
        float gameTime;
        float bgDefaultRed;
        bool gameEnded;
        private void Start()
        {
            playerManager.Init();
            knobManager.Init(3);
            rewardManager.Init();
            gameOverScreen.Init();
            QuitGamePanel.Init();

            rewardManager.MakeReward();
            SetScore(0);
            curScoreBonusCD = -1;
            bgDefaultRed = BG.color.r;
            GameSpeed = defaultGameSpeed;
            increaseGameSpeed = true;

            pauseBtn.onClick.RemoveAllListeners();
            pauseBtn.onClick.AddListener(() =>
            {
                if (!IsPaused) PauseGame();
                else ResumeGame();
            });
        }
        public async UniTask EndGame()
        {
            if (gameEnded) return;
            gameEnded = true;
            GameDebug.Log("Player Lost");
            playerManager.Kill();
            await UniTask.Delay(1500);
            gameParent.transform.DOKill(true);
            gameParent.transform.DOMoveY(gameParent.transform.position.y + 10, 1f);
            gameOverScreen.Open();
            gameOverScreen.Repaint(Score,BG.color);
        }
        public void PauseGame()
        {
            pauseBtn.transform.DOPunchScale(pauseBtn.transform.localScale * 0.1f, 0.2f);
            pauseImage.sprite = GameResourceHolder.Instance.PlaySprite;
            IsPaused = true;
            playerManager.ChangeMovement(!IsPaused);
            rewardManager.PauseRewards();
        }
        public void ResumeGame()
        {
            pauseBtn.transform.DOPunchScale(pauseBtn.transform.localScale * 0.1f, 0.2f);
            pauseImage.sprite = GameResourceHolder.Instance.PauseSprite;
            IsPaused = false;
            playerManager.ChangeMovement(!IsPaused);
            rewardManager.ResumeRewards();
        }
        public void GiveReward(int amount)
        {
            if (giveRewardCD > 0) return;
            giveRewardCD = 0.2f;
            streak++;
            if (curScoreBonusCD>0 && streak >= targetStreakForBonus)
            {
                amount += scoreBonusAmount;
                GiveBonus();
            }
            SetScore(Score + amount);
            rewardManager.MakeReward();
            SoundSystemManager.Instance.PlaySFX("Reward");
            PlayerGotReward?.Invoke();
        }
        void GiveBonus()
        {
            streak = 0;
            bonusTxt.text = "+" + scoreBonusAmount.ToString();
            bonusTxt.transform.parent.gameObject.SetActive(true);
            Color imageColor = bonusTxt.color;
            var bonusObj = bonusTxt.transform.parent;
            bonusTxt.DOColor(new Color(imageColor.r, imageColor.g, imageColor.b, 0f), 1.2f);
            bonusObj.DOMoveY(bonusObj.position.y - .75f, 1.25f).OnComplete(() =>
            {
                bonusTxt.transform.parent.gameObject.SetActive(false);
                bonusTxt.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1f);
                bonusObj.position += new Vector3(0, .75f, 0);
            });
            Color bgColor = BG.color;
            BG.DOComplete();
            BG.DOColor(new Color(1, bgColor.g, bgColor.b, bgColor.a), 1f);
            PlayerGotBonus?.Invoke();
            GameDebug.Log("Player Got Bonus");
        }
        void SetScore(int val)
        {
            Score = val;
            scoreTxt.text = Score.ToString();
            curScoreBonusCD = scoreBonusCD;
        }
        private void Update()
        {
            if (gameEnded) return;
            if (IsPaused) return;
            gameTime += Time.deltaTime;
            if (curScoreBonusCD > 0)
            {
                curScoreBonusCD -= Time.deltaTime;
            }
            else
            {
                LostStreak();
            }
            if (increaseGameSpeed)
            {
                var gameSpeedCurveIndex = Mathf.Clamp01(gameTime / maxGameSpeedTime);
                var increaseSpeed = gameSpeedCurve.Evaluate(gameSpeedCurveIndex);
                GameSpeed =Mathf.Lerp(defaultGameSpeed,maxGameSpeed,increaseSpeed);
                if (GameSpeed >= maxGameSpeed) increaseGameSpeed = false;
            }
            if (giveRewardCD > 0)
            {
                giveRewardCD -= Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGamePanel.Instance.Open();
            }
        }
        void LostStreak()
        {
            if (streak <= 0) return;
            streak = 0;
            Color bgColor = BG.color;
            BG.DOComplete();
            BG.DOColor(new Color(bgDefaultRed, bgColor.g, bgColor.b, bgColor.a), 1f);
        }
        public float GetPlayerAngle() => playerManager.CurAngle;
    }
}