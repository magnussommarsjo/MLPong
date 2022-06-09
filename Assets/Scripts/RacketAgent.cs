using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(Racket))]
public class RacketAgent : Agent
{
    [SerializeField] private GameObject oponent;
    [SerializeField] private GameObject ball;
    private Rigidbody2D ballRigidbody;
    private Ball ballScript;
    private Racket racket;

    private void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballScript = ball.GetComponent<Ball>();

        racket = GetComponent<Racket>();

        // Set up rewards 
        if (racket.player == Player.PLAYER_1)
        {
            ballScript.onPlayer2GoalEnter += Wins;
            ballScript.onPlayer1GoalEnter += Loses;
            ballScript.onPlayer1RacketCollision += HitOwnRacket;

        }
        else if (racket.player == Player.PLAYER_2)
        {
            ballScript.onPlayer1GoalEnter += Wins;
            ballScript.onPlayer2GoalEnter += Loses;
        }

    }

    private void Wins()
    {
        SetReward(1.0f);
    }

    private void Loses()
    {
        SetReward(0); // No penalty
    }

    private void HitOwnRacket()
    {
        SetReward(0.1f);
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Calculate realtive positions in X so that it would be same independent of
        // left or right side playing. 
        float oponentRelativeX = Mathf.Abs(oponent.transform.localPosition.x - transform.localPosition.x);
        float ballRelativeX = Mathf.Abs(ball.transform.localPosition.x - transform.localPosition.x);
        
        // Observe itself and the relative position to others
        sensor.AddObservation(transform.localPosition.y);

        sensor.AddObservation(oponentRelativeX);
        sensor.AddObservation(oponent.transform.localPosition.y);

        sensor.AddObservation(ball.transform.localPosition.y);
        sensor.AddObservation(ballRelativeX);

        // Relative speed towards agent 
        // (+1: moving towards agent)
        // (-1: moving away from agent)
        Vector2 ballDirection = ballRigidbody.velocity.normalized;
        sensor.AddObservation(ballDirection.y);
        sensor.AddObservation(ballDirection.x * Mathf.Sign(transform.localPosition.x));

        // Observe ball velocity
        sensor.AddObservation(ballRigidbody.velocity.magnitude);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float upAmount = actions.ContinuousActions[0];
        racket.Move(upAmount); // Move up

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            continousActionsOut[0] = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            continousActionsOut[0] = -1f;
        }
        else
        {
            continousActionsOut[0] = 0f;
        }
    }


}
