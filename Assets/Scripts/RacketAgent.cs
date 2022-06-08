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
        SetReward(-1.0f);
    }

    private void HitOwnRacket()
    {
        SetReward(0.1f); // Do we need this?
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe itself and the oponents position
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);
        sensor.AddObservation(oponent.transform.localPosition.x);
        sensor.AddObservation(oponent.transform.localPosition.y);

        // Observe ball position and velocity
        sensor.AddObservation(ball.transform.localPosition.x);
        sensor.AddObservation(ball.transform.localPosition.y);
        sensor.AddObservation(ballRigidbody.velocity.x);
        sensor.AddObservation(ballRigidbody.velocity.y);

        // Note: Do we need to observe top and bottom wall as well?
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
