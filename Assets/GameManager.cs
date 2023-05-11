using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Collect(int value)
    {
        score += value;
        ScoreText();
    }


    public void ScoreText()
    {
        scoreText.SetText("Gold: " + score.ToString());
    }
}
