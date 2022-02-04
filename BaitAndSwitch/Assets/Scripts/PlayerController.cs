using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public static event Action PlayerMoves;
    public TileController currentPosition;
    public int colorIndex;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        HandlePlayerMovement();


    }

    void HandlePlayerMovement()
    {

        if (Input.GetButtonDown("Left"))
        {
            AttemptMovement(GameManager.instance.playerCoordinates + Vector2Int.left);
        }
        if (Input.GetButtonDown("Right"))
        {
            AttemptMovement(GameManager.instance.playerCoordinates + Vector2Int.right);
        }
        if (Input.GetButtonDown("Up"))
        {
            AttemptMovement(GameManager.instance.playerCoordinates + Vector2Int.up);
        }
        if (Input.GetButtonDown("Down"))
        {
            AttemptMovement(GameManager.instance.playerCoordinates + Vector2Int.down);
        }

    }

    private void AttemptMovement(Vector2Int targetCoordinates)
    {
        if (GameManager.instance.CanPlayerMoveToTile(targetCoordinates) && GameManager.instance.isPlayerTurn)
        {
            transform.position = GameManager.instance.GetGridTile(targetCoordinates).playerTarget.position;
            GameManager.instance.UpdatePlayerPosition(targetCoordinates);
            PlayerMoves?.Invoke();
        }
    }
}
