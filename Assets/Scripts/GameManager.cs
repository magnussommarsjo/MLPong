using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Ball ball;
    [SerializeField] private Racket player1;
    [SerializeField] private Racket player2;
    [SerializeField] private ScoreHandler scoreHandler;
    // Start is called before the first frame update
    void Start()
    {
        ball.onPlayer1GoalEnter += Player2Scores;
        ball.onPlayer2GoalEnter += Player1Scores;

    }

    private void Player1Scores()
    {
        Debug.Log("Player One Scores!");
        if (scoreHandler != null)
        {
            scoreHandler.Player1Scores();
        }
        StartCoroutine(StartNewRound());
    }

    private void Player2Scores()
    {
        Debug.Log("Player Two Scores!!");
        if (scoreHandler != null)
        {
            scoreHandler.Player2Scores();
        }
        StartCoroutine(StartNewRound());
    }

    private IEnumerator StartNewRound()
    {
        Debug.Log("Starting new round");
        ball.ResetBall();
        player1.ResetRacket();
        player2.ResetRacket();
        yield return new WaitForSeconds(2);
        // Add UI updates here. 
        ball.GenerateRandomVelocity();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
