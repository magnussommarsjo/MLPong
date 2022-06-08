using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    private int player1Score = 0;
    private int player2Score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int maxScore = 11;

    public ResultState Result { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScores();
    }
    public void Player1Scores()
    {
        player1Score += 1;
        UpdateScores();
    }

    public void Player2Scores()
    {
        player2Score += 1;
        UpdateScores();
    }

    private void UpdateScores()
    {
        UpdateResultState();
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        scoreText.text = $"{player1Score} : {player2Score}";
    }

    private void UpdateResultState()
    {
        // Update Result State
        if (player1Score == maxScore)
        {
            Result = ResultState.PLAYER_ONE_WON;
        }
        else if (player2Score == maxScore)
        {
            Result = ResultState.PLAYER_TWO_WON;
        }
        else
        {
            Result = ResultState.GAME_ONGOING;
        }

    }

}

public enum ResultState
{
    GAME_ONGOING,
    PLAYER_ONE_WON,
    PLAYER_TWO_WON,
}
