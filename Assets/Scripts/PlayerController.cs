using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Racket))]
public class PlayerController : MonoBehaviour
{
    private Racket racket;
    [SerializeField] private KeyCode moveUpKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode moveDownKey = KeyCode.DownArrow;


    // Start is called before the first frame update
    void Start()
    {
        racket = GetComponent<Racket>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
    }

    private void HandlePlayerMovement()
    {
        if (Input.GetKey(moveUpKey)){
            // Move up
            racket.Move(1);
        } else if (Input.GetKey(moveDownKey)){
            // Move Down
            racket.Move(-1);
        } else {
            // No movement
            racket.Move(0);
        }
    }
}
