using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; protected set; }

    public bool isPaused;

	void Start ()
    {
		if (Instance != null)
        {
            Debug.LogError("There should be no more than one GameController in the scene. There are atleast two.");
        }

        Instance = this;
	}
	
	void Update ()
    {
		
	}
}
