using DG.Tweening;
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
        [SerializeField] float gameSpeedIncreaseRate;
        [SerializeField] float maxGameSpeed;
        [SerializeField] int targetStreakForBonus;
        public float GameSpeed = 1f;
        [Header("Manager References")]
        [SerializeField] PlayerManager playerManager;
        [SerializeField] KnobManager knobManager;
        [SerializeField] RewardManager rewardManager;

        [Header("References")]
        [SerializeField] TextMeshProUGUI scoreTxt;
        [SerializeField] TextMeshProUGUI bonusTxt;
        [SerializeField] Button pauseBtn;
        [SerializeField] Image pauseImage;
        [HideInInspector]
        public int Score;
        [HideInInspector]
        public bool IsPaused;
        public Action PlayerGotReward;
        public Action PlayerGotBonus;

        float curScoreBonusCD;
        bool increaseGameSpeed;
        int streak;
        float giveRewardCD = 0.2f;
        private void Start()
        {
            playerManager.Init();
            knobManager.Init(3);
            rewardManager.Init();
            rewardManager.MakeReward();
            SetScore(0);
            curScoreBonusCD = -1;
            increaseGameSpeed = true;
            pauseBtn.onClick.RemoveAllListeners();
            pauseBtn.onClick.AddListener(() =>
            {
                if (!IsPaused) PauseGame();
                else ResumeGame();
            });
        }
        public void EndGame()
        {
            playerManager.ChangeMovement(false);
        }
        void PauseGame()
        {
            pauseBtn.transform.DOPunchScale(pauseBtn.transform.localScale * 0.1f, 0.2f);
            pauseImage.sprite = GameResourceHolder.Instance.PlaySprite;
            IsPaused = true;
            playerManager.ChangeMovement(!IsPaused);
            rewardManager.PauseRewards();
        }
        void ResumeGame()
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
            else
            {
                streak = 1;
            }
            SetScore(Score + amount);
            rewardManager.MakeReward();
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
            if (curScoreBonusCD > 0)
            {
                curScoreBonusCD -= Time.deltaTime;
            }
            if (increaseGameSpeed)
            {
                GameSpeed += Time.deltaTime * gameSpeedIncreaseRate;
                if (GameSpeed >= maxGameSpeed) increaseGameSpeed = false;
            }
            if (giveRewardCD > 0)
            {
                giveRewardCD -= Time.deltaTime;
            }
        }
        public float GetPlayerAngle() => playerManager.CurAngle;
    }
}