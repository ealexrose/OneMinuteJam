using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public TileController currentTile;
    public Vector2Int currentCoordinates;
    public Animator myAnimator;
    public int colorState;
    public int index;
    MeshRenderer torusRenderer;

    public GameObject skinMaterial;
    public GameObject bottomMaterial;
    public GameObject altHead;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        skinMaterial.GetComponent<MeshRenderer>().material.SetColor("_Color", GameManager.instance.monsterColors.colorKeys[colorState].color);
        altHead.GetComponent<MeshRenderer>().material.SetColor("_Color", GameManager.instance.monsterColors.colorKeys[colorState].color);
        bottomMaterial.GetComponent<MeshRenderer>().material.color = GameManager.instance.monsterColors.colorKeys[colorState].color;
        skinMaterial.SetActive(false);
        bottomMaterial.SetActive(false);
        altHead.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {

    }
    internal void Attack()
    {
        if (currentCoordinates == GameManager.instance.playerCoordinates && colorState == GameManager.instance.GetPlayerState())
        {
            GameManager.instance.player.Kill();
            skinMaterial.SetActive(false);
            bottomMaterial.SetActive(false);
            altHead.SetActive(false);
        }
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
                StartCoroutine(MonsterMove(target));
            }
            else
            {
                StartCoroutine(DelayPlayer());
            }
        }

    }

    private IEnumerator DelayPlayer()
    {

        yield return new WaitForSeconds(1f);
        GameManager.instance.isPlayerTurn = true;

    }
    IEnumerator MonsterMove(Vector2Int targetCoordinates)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, .4f));
        float targetRotation = 0f;
        if (targetCoordinates.x > currentCoordinates.x)
        {
            targetRotation = -90f;
        }
        if (targetCoordinates.y > currentCoordinates.y)
        {
            targetRotation = 180f;
        }
        if (targetCoordinates.x < currentCoordinates.x)
        {
            targetRotation = 90f;
        }
        //if (targetCoordinates.y < currentCoordinates.x) 
        //{
        //    targetRotation = 0f;
        //}
        Vector3 angles = transform.localRotation.eulerAngles;
        angles.y = targetRotation;
        transform.localRotation = Quaternion.Euler(angles);


        GameManager.instance.UpdateMonsterPosition(index, targetCoordinates);
        AudioManager.instance.Play("Woosh");
        myAnimator.SetTrigger("Move");
        yield return new WaitForSeconds(1.1f);
        skinMaterial.SetActive(false);
        altHead.SetActive(true);
        myAnimator.SetTrigger("Stop");
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        UpdateMonsterPosition();
        GameManager.instance.isPlayerTurn = true;
        altHead.SetActive(false);
        yield return new WaitForEndOfFrame();
        skinMaterial.SetActive(true);
        yield return null;

    }

    public void UpdateMonsterPosition()
    {
        transform.position = GameManager.instance.grid[currentCoordinates.x, currentCoordinates.y].enemyTarget.position;
    }

    public void ColorTile()
    {

        currentTile.RecolorTile(colorState);
        if (currentTile.goalColor == colorState) 
        {
            AudioManager.instance.Play("Correct");
        }

    }

    public void RiseUp()
    {
        StartCoroutine(RiseUpEnumerator());

    }

    IEnumerator RiseUpEnumerator()
    {
        transform.parent = currentTile.enemyTarget;
        transform.localPosition = Vector3.down * 8f;

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(UnityEngine.Random.Range(4, 9) * 0.15f);
        currentTile.RecolorTile(colorState);
        skinMaterial.SetActive(true);
        bottomMaterial.SetActive(true);
        for (float i = 0; i < 1.2; i += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(Vector3.down * 8f, Vector3.zero, i / 1.2f);
            yield return null;
        }
        transform.localPosition = Vector3.zero;
        GameManager.instance.isPlayerTurn = true;
        transform.parent = null;


    }

}

