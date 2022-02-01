using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Color colorOne;
    public Color colorTwo;
    public int colorState = 1;

    private SpriteRenderer spriteRenderer;
    private GridController gridController;

    public static event Action PlayerMoves;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playerTurn) 
        {
            HandleMovement();
        }

        HandleColorSwap();
    }

    private void HandleMovement()
    {
        bool moved = false;

        if (Input.GetButtonDown("Left")) 
        {
            transform.position += (Vector3)Vector2.left;
            GameManager.instance.playerCoordinates.x--;
            moved = true;
        }
        if (Input.GetButtonDown("Right")) 
        {
            transform.position += (Vector3)Vector2.right;
            GameManager.instance.playerCoordinates.x++;
            moved = true;
        }

        if (Input.GetButtonDown("Up")) 
        {
            transform.position += (Vector3)Vector2.up;
            GameManager.instance.playerCoordinates.y++;
            moved = true;
        }

        if (Input.GetButtonDown("Down")) 
        {
            transform.position += (Vector3)Vector2.down;
            GameManager.instance.playerCoordinates.y--;
            moved = true;
        }

        if (moved) 
        {
            GameManager.instance.playerTurn = false;
            PlayerMoves?.Invoke();
        }


    }

    private void HandleColorSwap()
    {

        if (Input.GetButtonDown("Jump")) 
        {
            if (colorState == 1)
            {
                colorState = 2;
                spriteRenderer.color = colorTwo;
            }
            else 
            {
                colorState = 1;
                spriteRenderer.color = colorOne;
            }        
        }
    }
}
