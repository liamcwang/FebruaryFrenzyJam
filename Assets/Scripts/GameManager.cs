using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


/// <summary>
/// Responsible for maintaining gamestate across scenes and during the session.
/// Also makes objects globally available to other objects.
/// And holds responsiblity for hooking all relevant objects together for important functions.
/// </summary>
public class GameManager {
    // REMINDER: Working static instances :D
    private static GameManager singletonInstance;
    public enum GameState{START_MENU, GAME, VICTORY, DEFEAT, RESTART}
    public int enemyCount;
    
    public GameState gameState {get; set;}
    public Boss boss;
    public Player player;
    public PlayerCam playerCam;
    public MinimapCam minimapCam;
    public MainMenu mainMenu;

    /// <summary>
    /// Think of this as the same as Start() in Unity
    /// Except it will still run on the first frame.
    /// </summary>
    /// <param name="setGameState"></param>
    private GameManager(GameState setGameState) {
        enemyCount = 0;
        gameState = setGameState;
        

        // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
        // because the game manager will be created before the objects
    }    
 
    /// <summary>
    /// The main reference point for GameManager.
    /// During runtime, should always refer to GameManager.instance for specific variables
    /// </summary>
    /// <value></value>
    public static GameManager instance {
        get {
            if(singletonInstance==null) {
                singletonInstance = new GameManager(GameState.START_MENU);

            }

            return singletonInstance;
        }
    }


    // Add your game mananger members here
    public static void Pause(bool paused) {
        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    /// <summary>
    /// Start of game functions go here.
    /// </summary>
    public static void StartGame() {
        Time.timeScale = 1;
        GameManager.instance.gameState = GameState.GAME;
    }

    /// <summary>
    /// Return to Main Menu functions go here.
    /// When scene reloads many parts of the program will ask
    /// What state the game is in to determine Start behavior.
    /// </summary>
    #if UNITY_EDITOR
    [MenuItem("GameManager/MainMenu")]
    #endif
    public static void returnToMaMe() {
        singletonInstance = new GameManager(GameState.START_MENU);
        SceneManager.LoadScene(0);
    }


    /// <summary>
    /// Restart game functions go here.
    /// When scene reloads many parts of the program will ask
    /// What state the game is in to determine Start behavior.
    /// </summary>
    #if UNITY_EDITOR
    [MenuItem("GameManager/RestartGame")]
    #endif
    public static void RestartGame(){
        Debug.Log("Restarting...");
        singletonInstance = new GameManager(GameState.RESTART);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Victory functions go here.
    /// </summary>
    public static void Victory() {
        instance.gameState = GameState.VICTORY;
        instance.mainMenu.Victory();
        
    }

    /// <summary>
    /// Defeat functions go here.
    /// </summary>
    public static void Defeat(){
        instance.gameState = GameState.DEFEAT;
        instance.mainMenu.Defeat();
    }


    /// <summary>
    /// Thought this might be relevant at some point.
    /// </summary>
    #if UNITY_EDITOR
    [MenuItem("GameManager/EnemyCount")]
    #endif
    static void EnemyCount()
    {
        Debug.Log(instance.enemyCount);
    }

}
