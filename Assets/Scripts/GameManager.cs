using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Ball ball;
    [SerializeField] private Racket player1;
    [SerializeField] private Racket player2;
    [SerializeField] private ScoreHandler scoreHandler;
    [SerializeField] private bool isTraining = false;
    // Start is called before the first frame update
    void Start()
    {
        ball.onPlayer1GoalEnter += Player2Scores;
        ball.onPlayer2GoalEnter += Player1Scores;

        // Process game state evry time someone scores. 
        ball.onPlayer1GoalEnter += ProcessGameState;
        ball.onPlayer2GoalEnter += ProcessGameState;

    }

    private void ProcessGameState()
    {
        // Check if is traingin and then simply start a new round
        if (isTraining)
        {
            StartCoroutine(StartNewRound());
            return;
        }

        if (scoreHandler != null && scoreHandler.Result != ResultState.GAME_ONGOING) {
            // TODO: Show text of who won the game before closing
            if (scoreHandler.Result == ResultState.PLAYER_ONE_WON) Debug.Log("Player One Wins");
            if (scoreHandler.Result == ResultState.PLAYER_TWO_WON) Debug.Log("Player Two Wins");
        
            StartCoroutine(EndGame(2));
        }
    }

    private IEnumerator EndGame(int delayInSeconds) {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("MainMenu");
    }

    private void Player1Scores()
    {
        Debug.Log("Player One Scores!");
        if (scoreHandler != null)
        {
            scoreHandler.Player1Scores();
        }
    }

    private void Player2Scores()
    {
        Debug.Log("Player Two Scores!!");
        if (scoreHandler != null)
        {
            scoreHandler.Player2Scores();
        }
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
