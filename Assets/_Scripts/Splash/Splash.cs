using System.Collections;
using System.Collections.Generic;
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
        }
    }
}