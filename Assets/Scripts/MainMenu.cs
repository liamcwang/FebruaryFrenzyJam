using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public enum ScreenName {START, DEFEAT, VICTORY}; 
    [SerializeField] private UIObject[] UIElements;
    private Dictionary<ScreenName, GameObject> UIRef;

    void Awake() {
        UIRef = new Dictionary<ScreenName, GameObject>();
        foreach (UIObject UIObj in UIElements) {
            UIRef[UIObj.name] = UIObj.gameObject;
        }
    }

    public void setScreen(ScreenName key, bool isActive) {
        GameObject gObj = UIRef[key];
        gObj.SetActive(isActive);
    }

    public void StartGame() {
        setScreen(ScreenName.START, false);
        GameManager.instance.StartGame();
    }

    public void RestartGame() {

    }
}

[System.Serializable]
public struct UIObject {
    public MainMenu.ScreenName name;
    public GameObject gameObject;

}