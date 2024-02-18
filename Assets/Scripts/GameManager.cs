using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager {
    private static GameManager singletonInstance;
    public int enemyCount;

    public Boss boss;
    public Player player;
    public MinimapCam minimapCam;
 
    private GameManager() {
        enemyCount = 0;
        // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
        // because the game manager will be created before the objects
    }    
 
    public static GameManager instance {
        get {
            if(singletonInstance==null) {
                singletonInstance = new GameManager();
            }

            return singletonInstance;
        }
    }

    // Add your game mananger members here
    public void Pause(bool paused) {
        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void StartGame() {
        minimapCam.PlaceTowers();
        Time.timeScale = 1;
    }

    public void EndGame() {
        Time.timeScale = 0;
    }

    [MenuItem("GameManager/EnemyCount")]
    static void EnemyCount()
    {
        Debug.Log(instance.enemyCount);
    }

}
