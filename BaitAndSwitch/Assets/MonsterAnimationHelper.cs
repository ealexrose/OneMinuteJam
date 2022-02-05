using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationHelper : MonoBehaviour
{
    public EnemyController enemyController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMonsterPosition() 
    {
        //enemyController.UpdateMonsterPosition();
    }

    public void ColorTile() 
    {
        enemyController.ColorTile();
    }
}
