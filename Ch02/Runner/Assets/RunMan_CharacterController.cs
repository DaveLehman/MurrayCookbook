using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPC;


public class RunMan_CharacterController : BasePlayer2DPlatformCharacter
{
    // typo? book says class is BaseSoundController
    public BaseSoundManager _soundControl;
    public Animator _RunManAnimator;
    public bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        isRunning = false;
        allow_jump = true;
        allow_right = true;
        allow_left = true;
        _soundControl = GetComponent<BaseSoundManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        if(isOnGround && isRunning && _RunManAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunMan_Run"))
        {
            StartRunAnimation();
        }
    }

    public void StartRunAnimation()
    {
        isRunning = true;
        _RunManAnimator.SetTrigger("Run");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _soundControl.PlaySoundByIndex(1);
        // not found -- not written yet??
        // RunMan_GameManager.instance.PlayerFell();
    }

    public override void Jump()
    {
        base.Jump();
        _soundControl.PlaySoundByIndex(0);
        _RunManAnimator.SetTrigger("Jump");
    }
}
