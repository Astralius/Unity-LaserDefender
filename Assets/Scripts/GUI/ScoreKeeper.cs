using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public static int ScorePoints { get; private set; }

    public void Score(int points)
    {
        ScorePoints += points;
        ScoreText.text = ScorePoints.ToString();
    }

    public void Reset()
    {
        ScorePoints = 0;
        ScoreText.text = ScorePoints.ToString();
    }

    private void Start()
    {
        if (ScorePoints > 0)
        {
            ScoreText.text = ScorePoints.ToString();
        }
    }
}
