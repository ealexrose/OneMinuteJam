using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int tileColor;
    public int goalColor;
    public Transform playerTarget;
    public Transform enemyTarget;
    public Vector2Int tileCoordinates;
    public GameObject edge;
    public GameObject mainTile;
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
        RecolorTile(tileColor);
        RecolorEdge(goalColor);
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

        if (tileColor == goalColor || goalColor == 0)
        {
            return true;
        }

        return false;

    }

    public void RecolorTile(int index)
    {
        mainTile.GetComponent<MeshRenderer>().material.SetColor("_Color", GameManager.instance.tileColors.colorKeys[index].color);
        tileColor = index;
    }

    public void RecolorEdge(int index)
    {
        edge.GetComponent<MeshRenderer>().material.color = GameManager.instance.tileColors.colorKeys[index].color;
        goalColor = index;
    }
}
