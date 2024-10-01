using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitGamePanel : PanelSystem.Panel
{
    [Header("References")]
    [SerializeField] Button exitBG;
    [SerializeField] Button closeBtn;
    [SerializeField] Button YesBtn;
    [SerializeField] UnityEvent StartAction;
    [SerializeField] UnityEvent CloseAction;
    public static QuitGamePanel Instance;
    public override void Init()
    {
        Instance = this;
    }

    protected override void OnCloseFinished()
    {
        gameObject.SetActive(false);
        CloseAction?.Invoke();
    }

    protected override void OnCloseStarted()
    {
        exitBG.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
    }

    protected override void OnOpenFinished()
    {
    }

    protected override void OnOpenStarted()
    {
        gameObject.SetActive(true);
        StartAction?.Invoke();
        exitBG.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        exitBG.onClick.AddListener(Close);
        closeBtn.onClick.AddListener(Close);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
}
