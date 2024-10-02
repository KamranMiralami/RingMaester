using DG.Tweening;
using SFXSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RingMaester
{
    public class Splash : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Slider LoadingSlider;
        [SerializeField] Transform DontDestroyObj;
        [SerializeField] Transform DontDestroyCanvas;

        public static ObjectPool<Dot> DotObjectPool;
        public static ObjectPool<LeaderBoardProfile> LeaderBoardProfileObjectPool;
        private async void Start()
        {
            Application.targetFrameRate = 120;
            SoundSystemManager.Instance.Setup();
            LoadingSlider.value = 0f /100f;
            Settings.Instance.SFXMult = PlayerPrefs.GetFloat("SFXMult", 1);
            Settings.Instance.MusicMult = PlayerPrefs.GetFloat("MusicMult", 1);
            Settings.Instance.VibrationMult = PlayerPrefs.GetFloat("VibrationMult", 1);
            SoundSystemManager.Instance.ChangeBGM("GameBG");
            SoundSystemManager.Instance.PlayBGM();
            SoundSystemManager.Instance.ChangeBGMVolumn(Settings.Instance.MusicMult);
            await MoveLoadingBar(20f / 100f);
            if (!PlayerPrefs.HasKey("FirstTime"))
            {
                GenerateRandomNumberFile();
                GenerateRandomStringFile();
                PlayerPrefs.SetInt("FirstTime", 1);
            }
            await MoveLoadingBar(55f / 100f,500);
            DontDestroyOnLoad(DontDestroyObj);
            DontDestroyOnLoad(DontDestroyCanvas);
            MakeObjectPool();
            await MoveLoadingBar(95f / 100f,350);
            GameDebug.Log("Loading Done");
            SceneManager.LoadSceneAsync(SceneNames.Instance.MainMenuSceneName);
        }
        async Task MoveLoadingBar(float value, int delay=250)
        {
            LoadingSlider.DOKill(true);
            LoadingSlider.DOValue(value, 0.2f).SetEase(Ease.Linear);
            await Task.Delay(delay);
        }
        void MakeObjectPool()
        {
            var LeaderBoardProfileObjPool = new ObjectPool<LeaderBoardProfile>
                (MainMenuResourceHolder.Instance.LeaderBoardProfilePrefab, LeaderboardSettings.Instance.NumberOfRandomNumbers, DontDestroyCanvas);
            var DotObjPool = new ObjectPool<Dot>
                (GameResourceHolder.Instance.DotPrefab, 5, DontDestroyObj);
            DotObjectPool = DotObjPool;
            LeaderBoardProfileObjectPool = LeaderBoardProfileObjPool;
        }
        void GenerateRandomNumberFile()
        {
            string filePath = Path.Combine(Application.persistentDataPath, LeaderboardSettings.Instance.FileNameN);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                int randomNumber = 0;
                for (int i = 1; i <= LeaderboardSettings.Instance.NumberOfRandomNumbers-5; i++)
                {
                    randomNumber =Mathf.FloorToInt(LeaderboardSettings.Instance.NumberOfRandomNumbers - 5-i);
                    writer.WriteLine(randomNumber);
                }
                randomNumber = 10;
                writer.WriteLine(randomNumber);
                randomNumber = 15;
                writer.WriteLine(randomNumber);
                randomNumber = 20;
                writer.WriteLine(randomNumber);
                randomNumber = 25;
                writer.WriteLine(randomNumber);
                randomNumber = 30;
                writer.WriteLine(randomNumber);
            }

            GameDebug.Log("File saved at: " + filePath);
        }
        void GenerateRandomStringFile()
        {
            string filePath = Path.Combine(Application.persistentDataPath, LeaderboardSettings.Instance.FileNameS);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string randomString;
                for (int i = 0; i < LeaderboardSettings.Instance.NumberOfRandomNumbers-5; i++)
                {
                    randomString = RandomStringGenerator.GenerateRandomString(8);
                    writer.WriteLine(randomString);
                }
                randomString = "kamran5";
                writer.WriteLine(randomString);
                randomString = "kamran4";
                writer.WriteLine(randomString);
                randomString = "kamran3";
                writer.WriteLine(randomString);
                randomString = "kamran2";
                writer.WriteLine(randomString);
                randomString = "kamran1";
                writer.WriteLine(randomString);
            }

            GameDebug.Log("File saved at: " + filePath);
        }
    }
}