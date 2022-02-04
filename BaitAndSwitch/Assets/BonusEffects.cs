using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEffects : MonoBehaviour
{
    [Range(0.001f, 1f)]
    public float waveSpeed;
    [Range(0.1f, 5f)]
    public float waveMagnitude;
    [Range(0.1f, 5f)]
    public float waveSpacer;
    public float timePassed;
    public float baseHeight = 5f;
    [Range(0.01f, 1f)]
    public float minClamp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        WaveEffect();
    }


    void WaveEffect()
    {
        for (int i = 0; i < GameManager.instance.grid.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.instance.grid.GetLength(1); j++)
            {
                Vector3 position = GameManager.instance.grid[i, j].transform.position;
                position.y = baseHeight + Mathf.Clamp(Mathf.PerlinNoise((((float)i) * waveSpacer + (timePassed * waveSpeed)), (((float)j) * waveSpacer)), minClamp, 1f) * waveMagnitude;
                GameManager.instance.grid[i, j].transform.position = position;


            }
        }
    }
}
