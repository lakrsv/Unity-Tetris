using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour 
{
    float Speed = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartGame()
    {
        GameManager.Speed = Speed;
        Application.LoadLevel(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SetSpeed(float amount)
    {
        Speed = amount;
    }
}
