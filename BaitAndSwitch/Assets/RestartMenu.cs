using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{

    public Animator animator;

    public void Restart() 
    {
        animator.SetTrigger("Restart");
    
    }
    public void LoadRestart() 
    {
        animator.SetTrigger("OpenMenu");
    }
    public void ReloadLevel() 
    {
        SceneManager.LoadScene(0);
    }
}
