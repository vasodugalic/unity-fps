
using UnityEngine;
using UnityEngine.UI;

public class Kills : MonoBehaviour
{
    int killCount = 0;
    public Text killsText;

    public void AddKillCount(int amount)
    {
        killCount += amount;
        killsText.text = $"Kills: {killCount}";
    }
}
