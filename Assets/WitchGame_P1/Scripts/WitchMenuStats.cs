using TMPro;
using UnityEngine;

// WEEK 4: Values loaded from PlayerPrefs survive restarting the build.
public class WitchMenuStats : MonoBehaviour
{
    [SerializeField] private TMP_Text savedStatsText;

    private void Start()
    {
        int wins = PlayerPrefs.GetInt("WitchWins", 0);
        if (savedStatsText == null) return;
        if (PlayerPrefs.HasKey("WitchBestTime"))
            savedStatsText.text = "WINS: " + wins + "    BEST TIME: " +
                PlayerPrefs.GetFloat("WitchBestTime").ToString("F1") + "s";
        else
            savedStatsText.text = "WINS: " + wins + "    BEST TIME: --";
    }
}
