using RingMaester;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardProfile : MonoBehaviour
{
    [Header("References")]
    public RectTransform Rect;
    [SerializeField] Image BG;
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI scoreTxt;
    bool isPlayer;
    public void Repaint(int number,string name)
    {
        nameTxt.text = name;
        scoreTxt.text = number.ToString();
        isPlayer = LeaderBoardPanel.Instance.PlayerName.Equals(name);
        if (isPlayer)
        {
            BG.color = MainMenuResourceHolder.Instance.LeaderboardSelfColor;
        }
        else
        {
            BG.color = MainMenuResourceHolder.Instance.LeaderboardOthersColor;
        }
    }
}
public class RandomStringGenerator
{
    private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomString(int length)
    {
        StringBuilder result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            var random = Random.Range(0, chars.Length);
            result.Append(chars[random]);
        }

        return result.ToString();
    }
}
