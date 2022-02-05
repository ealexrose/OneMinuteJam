using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public Animator menuAnimator;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit() 
    {
        AudioManager.instance.Play("Bell");
        AudioManager.instance.Play("Rumble");
        Application.Quit();

    }

    public void StartGame() 
    {
        AudioManager.instance.Play("Bell");
        AudioManager.instance.Play("Rumble");
        GameManager.instance.StartGame();
        menuAnimator.SetTrigger("FadeMenuOut");
    }
}
