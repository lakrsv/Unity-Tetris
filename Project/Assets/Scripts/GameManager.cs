using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static float Speed;
    private AudioSource audiosource;
	// Use this for initialization
	void Start () 
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.Play();
	}
	
    public void GoToMainMenu()
    {
        Invoke("MainMenu", 1);
    }
    public void MainMenu()
    {
        Application.LoadLevel(0);
    }
}
