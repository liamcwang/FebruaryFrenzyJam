using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public enum ScreenName {START, DEFEAT, VICTORY, CREDITS}; 
    [SerializeField] private UIObject[] UIElements;
    private Dictionary<ScreenName, GameObject> UIRef;

    void Awake() {
        GameManager.instance.mainMenu = this;
        UIRef = new Dictionary<ScreenName, GameObject>();
        foreach (UIObject UIObj in UIElements) {
            UIRef[UIObj.name] = UIObj.gameObject;
        }
        
    }

    void Start() {
        switch(GameManager.instance.gameState) {
            case GameManager.GameState.START_MENU:
                setScreen(ScreenName.START, true);
            break;
            default:
            break;
        }
    }

    public void setScreen(ScreenName key, bool isActive) {
        GameObject gObj = UIRef[key];
        gObj.SetActive(isActive);
    }

    public void StartGame() {
        setScreen(ScreenName.START, false);
        GameManager.StartGame();
    }

    public void RestartGame() {
        GameManager.RestartGame();
    }

    public void returnToMaMe(){
        GameManager.MainMenu();
    }
}

[System.Serializable]
public struct UIObject {
    public MainMenu.ScreenName name;
    public GameObject gameObject;

}