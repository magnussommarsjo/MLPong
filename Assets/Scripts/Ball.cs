using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Vector2 startPosition;
    private new Collider2D collider;
    private new Rigidbody2D rigidbody;

    [SerializeField] private float ballSpeed = 5f;
    [SerializeField] private float ballSpeedIncrease = 1.1f;
    [SerializeField] private float maxSpeed = 50f;
    private float startSpeed;

    [SerializeField] private bool isDirectionContactDependent = true;
    [SerializeField] private float relaxation = 1f;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        GenerateRandomVelocity();
    }

    public void GenerateRandomVelocity()
    {
        float angleLimit = 60f;
        float angle = Random.Range(-angleLimit, angleLimit);
        rigidbody.velocity = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ).normalized * ballSpeed;
    }

    public void ResetBall()
    {
        // Back to start position with zero speed. 
        transform.position = startPosition;
        rigidbody.velocity = Vector2.zero;
    }

    public event System.Action onPlayer1GoalEnter;
    public event System.Action onPlayer2GoalEnter;
    public event System.Action onPlayer1RacketCollision;
    public event System.Action onPlayer2RacketCollision;

    private void OnGoalEnter(Collider2D other)
    {
        if (other.CompareTag("Player1Goal") && onPlayer1GoalEnter != null)
        {
            onPlayer1GoalEnter();
        }
        else if (other.CompareTag("Player2Goal") && onPlayer2GoalEnter != null)
        {
            onPlayer2GoalEnter();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnGoalEnter(other);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.collider.CompareTag("Racket"))
        {
            Racket racket = other.gameObject.GetComponent<Racket>();
            if (racket.player == Player.PLAYER_1 && onPlayer1RacketCollision != null)
            {
                onPlayer1RacketCollision();
            }
            else if (racket.player == Player.PLAYER_2 && onPlayer2RacketCollision != null)
            {
                onPlayer2RacketCollision();
            }


            // If we collide with a racket, increases ball speed.
            if (rigidbody.velocity.magnitude < maxSpeed) {
                rigidbody.velocity *= ballSpeedIncrease;
            }

            if (isDirectionContactDependent)
            {
                rigidbody.velocity = GetNewVelocityFromRacketCollision(other);
            }
        }
    }

    private Vector2 GetNewVelocityFromRacketCollision(Collision2D racketCollision)
    {
        // Calculate new direction based on contact point and racket position. 
        ContactPoint2D contact = racketCollision.GetContact(0); // Assume first contact always is racket. 

        // If we hit the racket at the ends, then return the present velocity. 
        if (contact.normal.y != 0)
        {
            return rigidbody.velocity;
        }
        //Vector2 direction = (contact.point - (Vector2) racketCollision.collider.transform.position).normalized;
        Vector2 racketPosition = (Vector2)racketCollision.collider.transform.position;
        BoxCollider2D racketCollider = (BoxCollider2D)contact.collider;
        float racketSize = racketCollider.size.y;

        // Get relative collision point with value between -1 to 1. 
        float relativeCollisionPoint = (contact.point.y - racketPosition.y) / racketSize * 2;
        float angleToRotate = relativeCollisionPoint * 90 * relaxation;

        Vector2 direction = new Vector2(
                Mathf.Cos(angleToRotate * Mathf.Deg2Rad) * contact.normal.x,
                Mathf.Sin(angleToRotate * Mathf.Deg2Rad)
            ).normalized;

        // Apply new direction to ball. 
        return rigidbody.velocity.magnitude * direction;
    }

    private void OnTriggerExit2D(Collider2D other) {

        // If somehow the ball exits of court, then reset ball. 
        if (other.CompareTag("Boundary")) {
            ResetBall();
            GenerateRandomVelocity();
            Debug.Log("Hard Reset Ball");
        }
    }



}
