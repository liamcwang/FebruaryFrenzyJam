using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager {
    private static GameManager singletonInstance;
    public enum GameState{START_MENU, GAME, VICTORY, DEFEAT, RESTART}
    public int enemyCount;
    
    public GameState gameState {get; private set;}
    public Boss boss;
    public Player player;
    public MinimapCam minimapCam;
    public MainMenu mainMenu;

    private GameManager(GameState setGameState) {
        enemyCount = 0;
        gameState = setGameState;
        

        // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
        // because the game manager will be created before the objects
    }    
 
    public static GameManager instance {
        get {
            if(singletonInstance==null) {
                singletonInstance = new GameManager(GameState.START_MENU);
                Time.timeScale = 0;
            }

            return singletonInstance;
        }
    }


    // Add your game mananger members here
    public static void Pause(bool paused) {
        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public static void StartGame() {
        Time.timeScale = 1;
    }

    #if UNITY_EDITOR
    [MenuItem("GameManager/MainMenu")]
    #endif
    public static void returnToMaMe() {
        singletonInstance = new GameManager(GameState.START_MENU);
        SceneManager.LoadScene("testing");
        Time.timeScale = 0;
    }

    #if UNITY_EDITOR
    [MenuItem("GameManager/RestartGame")]
    #endif
    public static void RestartGame(){
        Debug.Log("Restarting...");
        singletonInstance = new GameManager(GameState.RESTART);
        SceneManager.LoadScene("testing");
        Time.timeScale = 1;
    }

    public static void Victory() {
        
    }

    public static void Defeat(){

    }

    #if UNITY_EDITOR
    [MenuItem("GameManager/EnemyCount")]
    #endif
    static void EnemyCount()
    {
        Debug.Log(instance.enemyCount);
    }

}
