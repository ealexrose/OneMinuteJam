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

    public bool RiseOnStart;
    public bool doWaveStuff = false;
    public float startHeight;
    public float endHeight;
    public float riseTime;
    // Start is called before the first frame update
    void Start()
    {
        HideTiles();
    }

    // Update is called once per frame
    void Update()
    {

        if (doWaveStuff)
        {
            timePassed += Time.deltaTime;
            WaveEffect();
        }
    }


    void WaveEffect()
    {
        for (int i = 0; i < GameManager.instance.grid.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.instance.grid.GetLength(1); j++)
            {

                if (GameManager.instance.grid[i, j])
                {
                    Vector3 position = GameManager.instance.grid[i, j].transform.position;
                    position.y = baseHeight + (Mathf.Clamp(Mathf.PerlinNoise((((float)i) * waveSpacer + (timePassed * waveSpeed)), (((float)j) * waveSpacer)), minClamp, 1f) -minClamp) * waveMagnitude;
                    GameManager.instance.grid[i, j].transform.position = position;
                }


            }
        }
    }

    public void RiseUp()
    {
        
        for (int j = 0; j < GameManager.instance.grid.GetLength(0); j++)
        {
            for (int k = 0; k < GameManager.instance.grid.GetLength(1); k++)
            {
                int random = Random.Range(0, 8);
                float randFloat = (float)random * 0.15f;
                StartCoroutine(RiseEnumerator(GameManager.instance.grid[j, k], randFloat));
            }
        }
        StartCoroutine(DelayWave(8f * 0.15f));
    }

    IEnumerator RiseEnumerator(TileController tile, float delay) 
    {

        Vector3 position = tile.transform.position;
        position = tile.transform.position;
        position.y = startHeight;
        tile.transform.position = position;

        yield return new WaitForSeconds(delay);
        for (float i = 0f; i < riseTime + (riseTime * 0.2f); i += Time.deltaTime)
        {
            position = tile.transform.position;
            position.y = Mathf.Lerp(startHeight, baseHeight * Mathf.Sin((Mathf.PI / 2) * 0.8f), Mathf.Sin((Mathf.PI / 2) * i / riseTime * 0.8f));
            tile.transform.position = position;
            yield return null;
        }

        position = tile.transform.position;
        position.y = baseHeight;
        tile.transform.position = position;
    }
    IEnumerator DelayWave(float delay) 
    {
        yield return new WaitForSeconds(delay);
        doWaveStuff = true;
        PlayerLanding();
        yield return new WaitForSeconds(1.5f);
        foreach (EnemyController monster in FindObjectsOfType<EnemyController>()) 
        {
            monster.RiseUp();
        }
    }
    void PlayerLanding() 
    {
        FindObjectOfType<PlayerController>().PlayerDrop();
    
    }

    void HideTiles() 
    {

        foreach (TileController tile in FindObjectsOfType<TileController>()) 
        {
            Vector3 position = tile.transform.position;
            position = tile.transform.position;
            position.y = startHeight;
            tile.transform.position = position;

        }

    }
}


