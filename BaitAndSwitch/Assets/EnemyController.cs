using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public TileController currentTile;
    public Vector2Int currentCoordinates;
    public int colorState;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddEnemyToList(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        PlayerController.PlayerMoves += MonsterTurn;
    }

    private void OnDisable()
    {
        PlayerController.PlayerMoves -= MonsterTurn;
    }

    void MonsterTurn()
    {
        if (GameManager.instance.GetPlayerState() == colorState)
        {
            //Get Pathfinding point
            Vector2Int target = GameManager.instance.GetNextSpotTowardsPoint(currentCoordinates, GameManager.instance.playerCoordinates);
            
            //Move to point if there is one to move to
            if (target != currentCoordinates) 
            {
                MonsterMove(target);
            }
        }

    }

    private void MonsterMove(Vector2Int targetCoordinates)
    {
        GameManager.instance.UpdateMonsterPosition(index, targetCoordinates);
    }
}

