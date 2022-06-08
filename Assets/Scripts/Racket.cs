using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player {
    PLAYER_1 = 1,
    PLAYER_2 = 2,
}

public class Racket : MonoBehaviour
{

    public Player player;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float courtWidth = 10f;
    private float yLimit;
    private Vector2 startPosition;

    private Vector3 movement = Vector3.zero;

    private void Start()
    {
        startPosition = transform.position;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        yLimit = courtWidth / 2 - collider.size.y / 2;
    }

    public void Move(float amplitude)
    {
        movement = Vector3.up * amplitude * moveSpeed;
    }

    private void Update()
    {
        transform.Translate(movement * Time.deltaTime);
        transform.localPosition = new Vector2(
            transform.localPosition.x,
            Mathf.Clamp(transform.localPosition.y, -yLimit, yLimit)
            );
    }

    public void ResetRacket() {
        transform.position = startPosition;
    }
}
