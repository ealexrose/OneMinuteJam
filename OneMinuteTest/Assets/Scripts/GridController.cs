using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GridController : MonoBehaviour
{
    public GameObject gridObject;
    public List<List<GameObject>> grid;
    [Range(0, 50)]
    public int width;
    [Range(0, 50)]
    public int height;

    public bool square;
    [Range(0.1f, 10f)]
    public float distance;

    public bool buildMode;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (buildMode)
        {
            if (grid == null)
            {
                grid = new List<List<GameObject>>();
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }
            if (square)
            {
                height = width;
            }
            width = width > 0 ? width : 0;
            height = height > 0 ? height : 0;
            distance = distance > 0 ? distance : 0;


            int widthDifference = width - grid.Count;

            int heightDifference = grid.Count >= 1 ? height - grid[0].Count : height;

            if (widthDifference >= 0)
            {

                for (int i = grid.Count; i < width; i++)
                {
                    List<GameObject> column = new List<GameObject>();
                    grid.Add(column);
                }
            }
            else
            {
                for (int i = grid.Count - 1; i >= width; i--)
                {
                    foreach (GameObject instantiatedGridObject in grid[i])
                    {
                        if (instantiatedGridObject != null)
                        {
                            DestroyImmediate(instantiatedGridObject);
                        }
                    }
                    grid.RemoveAt(i);
                }
            }

            if (heightDifference >= 0)
            {
                foreach (List<GameObject> column in grid)
                {
                    for (int i = column.Count; i < height; i++)
                    {
                        column.Add(PrefabUtility.InstantiatePrefab(gridObject) as GameObject);
                        column[column.Count - 1].transform.parent = transform;
                    }
                }
            }
            else
            {
                foreach (List<GameObject> column in grid)
                {
                    for (int i = column.Count - 1; i >= height; i--)
                    {
                        if (column[i] != null)
                        {
                            DestroyImmediate(column[i]);
                        }
                        column.RemoveAt(i);
                    }

                }
            }





            for (int i = 0; i < width; i++)
            {

                for (int j = 0; j < height; j++)
                {
                    float xPos = ((float)(i - width / 2)) * distance;

                    float yPos = ((float)(j - height / 2)) * distance;


                    if (width % 2 == 0)
                    {
                        xPos += (distance * .5f);
                    }
                    if (height % 2 == 0)
                    {
                        yPos += (distance * .5f);
                    }
                    grid[i][j].transform.localPosition = new Vector2(xPos, yPos);
                    grid[i][j].name = "Grid Point[" + i + "," + j + "]";
                }
                //minor change
            }
        }
    }
    public Vector2Int GetClosestGridCoordinate(Vector2 worldSpace)
    {
        float xPos = ((float)(width / 2)) * distance;

        float yPos = ((float)(height / 2)) * distance;

        Vector2 xBounds = new Vector2(-xPos, yPos);
        Vector2 yBounds = new Vector2(-yPos, xPos);

        if (width % 2 == 0)
        {
            xBounds.x = xBounds.x + (distance * .5f);
            xBounds.y = xBounds.y - (distance * .5f);
        }
        if (height % 2 == 0)
        {
            yBounds.x = yBounds.x + (distance * .5f);
            yBounds.y = yBounds.y - (distance * .5f);
        }



        xBounds += Vector2.one * transform.position.x;
        yBounds += Vector2.one * transform.position.y;

        Debug.Log(xBounds);

        worldSpace.x = Mathf.Clamp(worldSpace.x, xBounds.x, xBounds.y);
        worldSpace.y = Mathf.Clamp(worldSpace.y, yBounds.x, yBounds.y);


        worldSpace -= Vector2.one * transform.position;

        if (width % 2 == 0)
        {
            worldSpace.x -= distance/2f;
        }
        if (height % 2 == 0)
        {
            worldSpace.y -= distance/2f;
        }

        worldSpace.x = Mathf.Round((worldSpace.x / distance) + (width / 2));
        worldSpace.y = Mathf.Round((worldSpace.y / distance) + (height / 2));

        return new Vector2Int((int)worldSpace.x, (int)worldSpace.y);
    }

    public Vector2 GetClosestGridWorldPosition(Vector2 worldSpace)
    {
        Vector2Int gridCoordinate = GetClosestGridCoordinate(worldSpace);
        return GridCoordinateToWorldSpace(gridCoordinate);
    }

    public Vector2 GridCoordinateToWorldSpace(Vector2Int gridCoordinates)
    {
        float xPos = ((float)(gridCoordinates.x - width / 2)) * distance;

        float yPos = ((float)(gridCoordinates.y - height / 2)) * distance;


        if (width % 2 == 0)
        {
            xPos += (distance * .5f);
        }
        if (height % 2 == 0)
        {
            yPos += (distance * .5f);
        }

        return new Vector2(xPos, yPos) + (Vector2)transform.position;
    }

    public GameObject GetGridObjectAtCoordinates(Vector2Int coordinates) 
    {
        return gameObject.transform.GetChild(((coordinates.y )  + (coordinates.x * width))).gameObject;
    }
}
