using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    public int monsterType;
    public Color paintColor;
    public PlayerController player;
    public GridController gridController;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        gridController = FindObjectOfType<GridController>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        PlayerController.PlayerMoves -= MonsterMove;
    }
    private void OnEnable()
    {
        PlayerController.PlayerMoves += MonsterMove;
    }


    // Update is called once per frame
    void Update()
    {

    }

    void MonsterMove()
    {
        StartCoroutine(MoveMonsterAnimated());
        //if (player.colorState == monsterType)
        //{
        //    bool moved = false;

        //    if (Mathf.Abs(GameManager.instance.playerCoordinates.x - transform.position.x) > Mathf.Abs(GameManager.instance.playerCoordinates.y - transform.position.y))
        //    {
        //        if (GameManager.instance.playerCoordinates.x > transform.position.x && !moved)
        //        {
        //            transform.position = transform.position + Vector3.right;
        //            moved = true;
        //        }
        //        else if (GameManager.instance.playerCoordinates.x < transform.position.x && !moved)
        //        {
        //            transform.position = transform.position - Vector3.right;
        //            moved = true;
        //        }
        //    }
        //    else 
        //    {

        //        if (GameManager.instance.playerCoordinates.y > transform.position.y && !moved)
        //        {
        //            transform.position = transform.position + Vector3.up;
        //            moved = true;
        //        }
        //        else if (GameManager.instance.playerCoordinates.y < transform.position.y && !moved)
        //        {
        //            transform.position = transform.position - Vector3.up;
        //            moved = true;
        //        }
        //    }


        //    if (moved) 
        //    {

        //        GameObject tile = gridController.GetGridObjectAtCoordinates(new Vector2Int((int)transform.position.x + 4, (int) transform.position.y + 4));
        //        tile.GetComponent<SpriteRenderer>().color = paintColor;
        //    }
        //}

    }

    IEnumerator MoveMonsterAnimated() 
    {
        if (player.colorState == monsterType)
        {
            bool moved = false;
            Vector3 targetPosition = new Vector3();
            if (Mathf.Abs(GameManager.instance.playerCoordinates.x - transform.position.x) > Mathf.Abs(GameManager.instance.playerCoordinates.y - transform.position.y))
            {
                if (GameManager.instance.playerCoordinates.x > transform.position.x && !moved)
                {
                   targetPosition = transform.position + Vector3.right;
                    moved = true;
                }
                else if (GameManager.instance.playerCoordinates.x < transform.position.x && !moved)
                {
                    targetPosition = transform.position - Vector3.right;
                    moved = true;
                }
            }
            else
            {

                if (GameManager.instance.playerCoordinates.y > transform.position.y && !moved)
                {
                    targetPosition = transform.position + Vector3.up;
                    moved = true;
                }
                else if (GameManager.instance.playerCoordinates.y < transform.position.y && !moved)
                {
                    targetPosition = transform.position - Vector3.up;
                    moved = true;
                }
            }


            if (moved)
            {
                Vector3 originalPosition = transform.position;
                for (float i = 0f; i < .4f; i += Time.deltaTime)
                {
                    transform.position = Vector3.Lerp(originalPosition, targetPosition, i / .4f);
                    yield return null;
                }
                transform.position = targetPosition;

                GameObject tile = gridController.GetGridObjectAtCoordinates(new Vector2Int((int)transform.position.x + 4, (int)transform.position.y + 4));
                tile.GetComponent<SpriteRenderer>().color = paintColor;
                GameManager.instance.playerTurn = true;
            }
        }




    }


}
