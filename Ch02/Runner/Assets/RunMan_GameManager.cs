using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPC;

public class RunMan_GameManager : BaseGameManager
{
    public float runSpeed = 1f;
    public float distanceToSpawnPlatform = 2f;
    public bool isRunning;
    public static RunMan_GameManager instance { get; private set; }
    // constructor
    public RunMan_GameManager()
    {
        if(instance != null)
        {
            Debug.Log("RunMan_GameManager constructor has been called -- There is already a RunMan_GameManager instance running, so we're not making another one");
        }
        else
        {
            instance = this;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
