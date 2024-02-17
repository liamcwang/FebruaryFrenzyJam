using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {
    private static GameManager singletonInstance;
    public int enemyCount;

    public Boss boss;
    public Player player;
 
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
    }

    public void EndGame() {
        
    }

}