using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartHelper : MonoBehaviour
{
    public RestartMenu restartMenu;
    public void RestartHelpFunction() 
    {
        restartMenu.ReloadLevel();

    }
}
