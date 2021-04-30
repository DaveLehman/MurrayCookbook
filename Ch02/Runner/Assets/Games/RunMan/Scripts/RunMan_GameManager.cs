using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPC;
using UnityEngine.SceneManagement;

public class RunMan_GameManager : BaseGameManager
{
    /* Main Game Loop located here */
    public float runSpeed = 1f;
    public float distanceToSpawnPlatform = 20f;
    public bool isRunning;

    public Transform _platformPrefab;
    public RunMan_CharacterController _RunManCharacter;
    private float distanceCounter;
    public RunMan_UIManager _uiManager;

    public float playAreaTopY = -0.4f;
    public float playAreaBottomY = 0.2f;
    public float platformStartX = 3f;
    public static RunMan_GameManager instance { get; private set; }

    /*
 * Note that there is nothing in the Game Manager to deal with scoring -- that's the BaseUserManager component 
 */
    /*
     * The BaseGameManager includes 17 different states for larger more complicated games. This game will use 5
     * 
     * loaded           This state is called when the main game Scene first loads
     * gameStarting     After intialization and setup of anything we need in teh main Game Scene, gameStarting will be set and we
     *                  use this to display amessage that will notify the player to get ready to play
     * gamePlaying      Once the game is running, the state stays in gamePlaing
     * gameEnding       If the player falls off a platform and hits a hidden trigger, the game ends. The gameEnding state takes care of stopping movement
     *                  and preventing any more score being earned. We also show the final 'game over' user interface as this state begins
     * gameEnded        When this state occurs, we leave the main game Scene and load the main menu
     */
    /*
     * We deal with game states through three functions, SetTargetState(), UpdateTargetState(), UpdateCurrentState(). When a call to SetTargetState() is made,
     * BaseGameManager knows that the asked for state is different from the current state and calls UpdateTargetState(). The current state is then updated.
     * Meanwhile every frame' Update is checking UpdateCurrentState(). This is where we run any code that needs to update every frame.
     */

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
    public void Start()
    {
        SetTargetState(Game.State.loaded);
    }

    public override void UpdateTargetState()
    {
        Debug.Log("targetGameState=" + targetGameState);

        if (targetGameState == currentGameState)
            return;
        switch(targetGameState)
        {
            // main game loop
            case Game.State.loaded:
                Loaded();
                break;
            case Game.State.gameStarting:
                GameStarting();     // inherited, contains calls to invoke Unity events that we will be using to hide/show the user interface
                StartGame();
                break;
            case Game.State.gameStarted:
                // fire the game started event
                GameStarted();      // inherited, contains calls to invoke Unity events that we will be using to hide/show the user interface
                OnGameStarted.Invoke();
                SetTargetState(Game.State.gamePlaying);
                break;
            case Game.State.gamePlaying:
                break;
            case Game.State.gameEnding:
                GameEnding();
                EndGame();
                break;
            case Game.State.gameEnded:
                GameEnded();
                break;
        }
    }

    public override void Loaded()
    {
        base.Loaded();
        // set high score
       // _uiManager.SetHighScore(GetComponent<BaseProfileManager>().GetHighScore());
        //reset score
        //_RunManCharacter.SetScore(0);

        SetTargetState(Game.State.gameStarting);
    }

    void StartGame()
    {
        runSpeed = 0;

        // add a little delay to the start of the game unsing Unity's buult-in scheduling function, Invokde
        Invoke("StartRunning", 2f); 
    }

    void StartRunning()
    {
        isRunning = true;
        runSpeed = 1f;
        distanceCounter = 1f;

        SetTargetState(Game.State.gameStarted);

        //start animation
        _RunManCharacter.StartRunAnimation();

        InvokeRepeating("AddScore", 0.5f, 0.5f);
        
    }

    void EndGame()
    {
        isRunning = false;
        runSpeed = 0;

        // schedule return to main menu in 4 seconds
        Invoke("ReturnToMenu", 4f);

        // stop the repeating invoke that awards score
        CancelInvoke("AddScore");

        //if (GetComponent <BaseProfileManager>().SetHighScore(_RunManCharacter.GetScore()) == true)
        //    _uiManager.ShowGotHighScore();
        //_uiManager.SetFinalScore(_RunManCharacter.GetScore());

        SetTargetState(Game.State.gameEnded);
       
    }

    void AddScore()
    {
        // _RunManCharacter.AddScore(1);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("runMan_menu");
    }

    public void PlayerFell()
    {
        SetTargetState(Game.State.gameEnding);
    }

    void SpawnPlatform()
    {
        // increase difficulty level
        runSpeed += 0.02f;
        distanceCounter = 0;

        //float randomY = Random.Range(-0.4f, 0.2f);
        float randomY = Random.Range(playAreaTopY, playAreaBottomY);
        Vector2 start_pos = new Vector2(platformStartX, randomY);
        Instantiate(_platformPrefab, start_pos, Quaternion.identity);
        distanceCounter = Random.Range(-0.5f, 0.25f);

    }

    // Update is called once per frame
    void Update()
    {
     if(isRunning)
        {
            distanceCounter += runSpeed * Time.deltaTime;
            if(distanceCounter >= distanceToSpawnPlatform)
            {
                SpawnPlatform();
            }
            //_uiManager.SetScore(_RunManCharacter.GetScore());
        }
    }
}
