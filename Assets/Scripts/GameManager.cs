using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {
    private static GameManager instance;
    public int enemyCount;
 
    private GameManager() {
        enemyCount = 0;
        // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
        // because the game manager will be created before the objects
    }    
 
    public static GameManager Instance {
        get {
            if(instance==null) {
                instance = new GameManager();
            }

 
            return instance;
        }
    }

    // Add your game mananger members here
    public void Pause(bool paused) {
    }

}