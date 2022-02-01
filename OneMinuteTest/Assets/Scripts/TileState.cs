using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class TileState : MonoBehaviour
{
    public Gradient gradient;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer goalOutline;
    public enum TileColor 
    {
        Blank,
        Red,
        Green
    }

    public TileColor currentColor;
    public TileColor goalColor;
    private TileColor setGoalColor;

    public void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (setGoalColor != goalColor) 
        {
            setGoalColor = goalColor;
            goalOutline.color = gradient.colorKeys[(int)goalColor].color;
        }
    }
    public void SetTileColor(TileColor newTileColor) 
    {
        currentColor = newTileColor;
        spriteRenderer.color = gradient.colorKeys[(int)newTileColor].color;   
    }



}
