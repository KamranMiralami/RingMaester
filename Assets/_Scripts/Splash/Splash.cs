using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RingMaester
{
    public class Splash : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Button GoNext;
        private void Start()
        {
            Application.targetFrameRate = 120;
            GoNext.onClick.RemoveAllListeners();
            GoNext.onClick.AddListener(() =>
            {
                SceneManager.LoadSceneAsync(SceneNames.Instance.MainMenuSceneName);
            });
            Settings.Instance.SFXMult = PlayerPrefs.GetFloat("SFXMult", 1);
            Settings.Instance.MusicMult = PlayerPrefs.GetFloat("MusicMult", 1);
            Settings.Instance.VibrationMult = PlayerPrefs.GetFloat("VibrationMult", 1);
            if (!PlayerPrefs.HasKey("FirstTime"))
            {
                GenerateRandomNumberFile();
                GenerateRandomStringFile();
                PlayerPrefs.SetInt("FirstTime", 1);
            }
        }
        void GenerateRandomNumberFile()
        {
            string filePath = Path.Combine(Application.persistentDataPath, LeaderboardSettings.Instance.FileNameN);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                int randomNumber = 0;
                for (int i = 1; i <= LeaderboardSettings.Instance.NumberOfRandomNumbers-5; i++)
                {
                    randomNumber =Mathf.FloorToInt(i / 1000);
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