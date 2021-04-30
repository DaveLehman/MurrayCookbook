using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPC;

public class AutoTransformMove : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 moveVector;
    
    private Transform _tr;  // this object's transform

    // Start is called before the first frame update
    void Start()
    {
        _tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // keep move speed updated from game manager
        moveSpeed = RunMan_GameManager.instance.runSpeed;

        //move the transform
        _tr.Translate((moveVector * moveSpeed) * Time.deltaTime);
    }

    public void SetMoveSpeed(float aSpeed)
    {
        // amethod to adjust speed from other scripts
        moveSpeed = aSpeed;
    }
}
