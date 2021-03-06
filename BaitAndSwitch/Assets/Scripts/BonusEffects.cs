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

    public void WinAnimation() 
    {
        AudioManager.instance.Play("Win");
        AudioManager.instance.Play("Bell");
        //Hide Monsters
        foreach (EnemyController monster in FindObjectsOfType<EnemyController>()) 
        {
            monster.transform.parent = monster.currentTile.enemyTarget;
            StartCoroutine(HideMonster(monster));
        }
        //Drop Tiles
        doWaveStuff = false;
        foreach (TileController tile in FindObjectsOfType<TileController>()) 
        {
            if (tile.tileCoordinates != GameManager.instance.playerCoordinates && tile.goalColor == 0) 
            {
                
                StartCoroutine(SinkTiles(tile));
            }
        }
        //Make Player Jump
        FindObjectOfType<PlayerController>().Win();


    }

    public void LoseAnimation()
    {
        //Hide Monsters
        foreach (EnemyController monster in FindObjectsOfType<EnemyController>())
        {
            monster.transform.parent = monster.currentTile.enemyTarget;
            StartCoroutine(HideMonster(monster));
        }
        //Drop Tiles
        doWaveStuff = false;
        foreach (TileController tile in FindObjectsOfType<TileController>())
        {
            if (tile.tileCoordinates != GameManager.instance.playerCoordinates)
            {
                StartCoroutine(SinkTiles(tile));
            }
        }



    }

    private IEnumerator HideMonster(EnemyController monster)
    {
        Vector3 position = monster.transform.localPosition;
        Vector3 startPos = position;
        for (float i = 0; i < 0.4f; i += Time.deltaTime) 
        {
            position.y = Mathf.Lerp(startPos.y, -9f, i / 0.4f);
            monster.transform.localPosition = position;
            yield return null;
        }
    }

    IEnumerator SinkTiles(TileController tile) 
    {
        Vector3 startPosition = tile.transform.position;
        Vector3 position = tile.transform.position;
        position = tile.transform.position;
       
        int random = Random.Range(0, 8);
        float randFloat = (float)random * 0.15f;
        yield return new WaitForSeconds(randFloat);

        for (float i = 0; i < 0.8f; i += Time.deltaTime) 
        {
            position.y = Mathf.Lerp(startPosition.y,-5f, i / 0.8f);
            tile.transform.position = position;

            yield return null;
        }
        position.y = -5f;
        tile.transform.position = position;

    }


}


