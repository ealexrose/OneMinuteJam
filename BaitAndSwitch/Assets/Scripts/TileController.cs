using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    int tileColor;
    int goalColor;
    public Transform playerTarget;
    public Transform enemyTarget;
    public Vector2Int tileCoordinates;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        GameManager.LoadLevel += InitializeTile;
    }

    private void OnDisable()
    {
        GameManager.LoadLevel -= InitializeTile;
    }

    void InitializeTile() 
    {
        GameManager.instance.AddTileToGrid(this, tileCoordinates);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsOpenToPlayer() 
    {

        return true;

    }

    public bool IsOpenToMonster() 
    {

        return true;

    }

    public bool IsGoal() 
    {

        return false;
    
    }
}
