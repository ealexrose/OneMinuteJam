using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TileController[,] grid = new TileController[8, 8];
    public Vector2Int playerCoordinates;
    public bool isPlayerTurn;
    public PlayerController player;
    public List<EnemyController> enemies = new List<EnemyController>();
    public static event Action LoadLevel;



    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            LoadLevel?.Invoke();
        }
        else
        {
            Destroy(this);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        player.transform.position = grid[playerCoordinates.x, playerCoordinates.y].playerTarget.position;
        player.currentPosition = grid[playerCoordinates.x, playerCoordinates.y];

    }



    // Update is called once per frame
    void Update()
    {

    }

    public bool CanPlayerMoveToTile(Vector2Int tilePosition)
    {
        if (tilePosition.x < 0 || tilePosition.y < 0 || tilePosition.x >= grid.GetLength(0) || tilePosition.y >= grid.GetLength(0))
        {
            return false;
        }

        if (grid[tilePosition.x, tilePosition.y] && grid[tilePosition.x, tilePosition.y].IsOpenToPlayer())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public TileController GetGridTile(Vector2Int tilePosition)
    {
        if (tilePosition.x < 0 || tilePosition.y < 0 || tilePosition.x >= grid.GetLength(0) || tilePosition.y >= grid.GetLength(0))
        {
            return null;
        }

        if (grid[tilePosition.x, tilePosition.y])
        {
            return grid[tilePosition.x, tilePosition.y];
        }
        return null;
    }

    internal void UpdatePlayerPosition(Vector2Int targetCoordinates)
    {
        playerCoordinates = targetCoordinates;
        player.currentPosition = grid[targetCoordinates.x, targetCoordinates.y];
    }

    internal void UpdateMonsterPosition(int index, Vector2Int targetCoordinates)
    {
        enemies[index].transform.position = grid[targetCoordinates.x, targetCoordinates.y].enemyTarget.position;
        enemies[index].currentCoordinates = targetCoordinates;
    }

    internal void AddTileToGrid(TileController tileController, Vector2Int tileCoordinates)
    {
        if (!(tileCoordinates.x < 0 || tileCoordinates.y < 0 || tileCoordinates.x >= grid.GetLength(0) || tileCoordinates.y >= grid.GetLength(0)))
        {
            if (grid[tileCoordinates.x, tileCoordinates.y] != null)
            {
                Debug.LogWarning("Tile being overwritten at " + tileCoordinates + "Information may be lost");
            }
            grid[tileCoordinates.x, tileCoordinates.y] = tileController;
        }
        else
        {
            Debug.LogError("Attempted to add tile out of bounds of declared Array: failed to add at " + tileCoordinates);
        }
    }

    public void AddEnemyToList(EnemyController enemy)
    {
        enemies.Add(enemy);
        enemy.currentTile = GetGridTile(enemy.currentCoordinates);
        enemy.transform.position = enemy.currentTile.enemyTarget.position;
        enemy.index = enemies.Count - 1;
    }


    public int GetPlayerState()
    {
        return player.colorIndex;
    }

    internal Vector2Int GetNextSpotTowardsPoint(Vector2Int startCoordinates, Vector2Int targetCoordinates)
    {
        if (Math.Abs(targetCoordinates.x - startCoordinates.x) > Math.Abs(targetCoordinates.y - startCoordinates.y))
        {
            if (targetCoordinates.x - startCoordinates.x < 0)
            {
                if (MonsterCanEnter(startCoordinates + Vector2Int.left))
                {
                    return startCoordinates + Vector2Int.left;
                }
            }
            else if (targetCoordinates.x - startCoordinates.x > 0)
            {
                if (MonsterCanEnter(startCoordinates + Vector2Int.right))
                {
                    return startCoordinates + Vector2Int.right;
                }

            }
        }
        else 
        {
            if (targetCoordinates.y - startCoordinates.y > 0)
            {
                if (MonsterCanEnter(startCoordinates + Vector2Int.up))
                {
                    return startCoordinates + Vector2Int.up;
                }
            }
            else if (targetCoordinates.y - startCoordinates.y < 0)
            {
                if (MonsterCanEnter(startCoordinates + Vector2Int.down))
                {
                    return startCoordinates + Vector2Int.down;
                }

            }
        }

        return startCoordinates;
    }

    private bool MonsterCanEnter(Vector2Int tileCoordinates)
    {
        if ((tileCoordinates.x < 0 || tileCoordinates.y < 0 || tileCoordinates.x >= grid.GetLength(0) || tileCoordinates.y >= grid.GetLength(0)))
        {
            return false;
        }

        return grid[tileCoordinates.x, tileCoordinates.y].IsOpenToMonster();
    }
}
