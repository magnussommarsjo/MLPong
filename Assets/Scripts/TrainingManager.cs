using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : GameManager
{
    private RacketAgent racketAgentPlayer1;
    private RacketAgent racketAgentPlayer2;

    protected override void Start()
    {
        base.Start();

        racketAgentPlayer1 = player1.GetComponent<RacketAgent>();
        racketAgentPlayer2 = player2.GetComponent<RacketAgent>();
    }

    protected override IEnumerator StartNewRound(int timeDelay)
    {
        // Calling EndEpisode on agents
        racketAgentPlayer1.EndEpisode();
        racketAgentPlayer2.EndEpisode();

        return base.StartNewRound(timeDelay);
    }

    protected override void ProcessGameState()
    {
        StartCoroutine(StartNewRound(0));
    }


}
