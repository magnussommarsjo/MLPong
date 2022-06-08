using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Racket))]
public class SimpleAI : MonoBehaviour
{

    [SerializeField] private GameObject ball;
    [SerializeField] private float moveDelayDistance = 0.5f;
    private Racket racket;
    // Start is called before the first frame update
    void Start()
    {
        racket = GetComponent<Racket>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float currentPos = transform.position.y;
        float ballPos = ball.transform.position.y;

        if (currentPos + moveDelayDistance < ballPos)
        {
            // Racket position lower than ball, move up to match ball position. 
            racket.Move(1);
        }
        else if (currentPos - moveDelayDistance > ballPos)
        {
            // Racket position higher than  ball, move down to match position.
            racket.Move(-1);
        }
        else
        {
            // In position, No need to move. 
            racket.Move(0);
        }

    }
}
