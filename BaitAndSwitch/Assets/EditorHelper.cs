using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class EditorHelper : MonoBehaviour
{
    public bool UpdateAllVisuals;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateAllVisuals)
        {
            //    TileController[] tiles = FindObjectsOfType<TileController>();
            //    foreach (TileController tile in tiles) 
            //    {
            //        tile.RecolorTile(tile.tileColor);

            //    }
            UpdateAllVisuals = false;
        }
    }
}
