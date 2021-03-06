using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class HighscoreText : MonoBehaviour
{
    Text highscore;

    private void OnEnable()
    {
        highscore = GetComponent<Text>();
        highscore.text = "High Score: " +PlayerPrefs.GetInt("HighScore").ToString();
    }
}
