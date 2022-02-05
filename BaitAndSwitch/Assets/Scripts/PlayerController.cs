using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public int minColorIndex;
    public int maxColorIndex;
    public static event Action PlayerMoves;
    public TileController currentPosition;
    public int colorIndex;
    public Animator animator;
    public SkinnedMeshRenderer playerSkin;
    // Start is called before the first frame update
    void Start()
    {
        playerSkin.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        HandlePlayerMovement();
        HandleColorSwitching();

    }

    private void HandleColorSwitching()
    {
        if (Input.GetButtonDown("Jump") && GameManager.instance.isPlayerTurn)
        {

            SwitchColors();
        }
    }

    private void SwitchColors()
    {
        AudioManager.instance.Play("Switch");
        colorIndex++;
        if (colorIndex > maxColorIndex)
        {
            colorIndex = minColorIndex;
        }
        animator.SetTrigger("Spin");
        playerSkin.material.color = GameManager.instance.playerColors.colorKeys[colorIndex].color;
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
            StartCoroutine(DoPlayerMoveAnimations(targetCoordinates));

            GameManager.instance.UpdatePlayerPosition(targetCoordinates);
            PlayerMoves?.Invoke();
        }
    }
    IEnumerator DoPlayerMoveAnimations(Vector2Int targetCoordinates)
    {
        Vector2Int prevCoordinates = GameManager.instance.playerCoordinates;
        float currentRotation = transform.localRotation.eulerAngles.y;
        bool willDie = false;
        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
        {
            if (enemy.currentCoordinates == targetCoordinates && enemy.colorState == GameManager.instance.GetPlayerState())
            {
                willDie = true;
            }
        }

        Vector3 startPos = transform.position;
        Vector3 targetPos = GameManager.instance.GetGridTile(targetCoordinates).playerTarget.position;
        if (Mathf.Approximately(currentRotation, 270f))
        {
            currentRotation = -90f;
        }

        float targetRotation = 0f;
        Vector2Int currentCoordinates = GameManager.instance.playerCoordinates;
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
        if (targetCoordinates.y < currentCoordinates.y)
        {
            targetRotation = 0f;
        }

        Vector3 angles = transform.localRotation.eulerAngles;
        AudioManager.instance.Play("Step");
        animator.SetTrigger("Hop");
        new WaitForSeconds(.23f);
        for (float i = 0; i < .23f; i += Time.deltaTime)
        {
            angles.y = Mathf.Lerp(currentRotation, targetRotation, i / .23f);
            transform.localRotation = Quaternion.Euler(angles);
            yield return null;
        }
        angles.y = targetRotation;
        transform.localRotation = Quaternion.Euler(angles);
        new WaitForSeconds(.23f);
        for (float i = 0; i < .23f; i += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, i / .23f);
            if (willDie && i > (.23f / 2f))
            {
                transform.position = Vector3.Lerp(targetPos, startPos, i / .23f);

            }

            yield return null;
        }
        if (!willDie)
        {
            animator.SetTrigger("Idle");
            transform.position = GameManager.instance.GetGridTile(targetCoordinates).playerTarget.position;
            transform.parent = GameManager.instance.GetGridTile(targetCoordinates).playerTarget;
        }
        else
        {
            GameManager.instance.playerCoordinates = prevCoordinates;
            Kill();
        }

        yield return null;
    }

    public void PlayerDrop()
    {
        StartCoroutine(PlayerDropEnumerator());

    }

    IEnumerator PlayerDropEnumerator()
    {
        animator.SetTrigger("Drop");
        yield return new WaitForEndOfFrame();
        playerSkin.gameObject.SetActive(true);
    }
    public void Kill()
    {
        AudioManager.instance.Play("Death");
        animator.SetTrigger("Death");
        playerSkin.material.color = GameManager.instance.playerColors.colorKeys[3].color;
        FindObjectOfType<BonusEffects>().LoseAnimation();
        FindObjectOfType<RestartMenu>().LoadRestart();
    }

    public void Win()
    {
        animator.SetTrigger("Win");
    }
}
