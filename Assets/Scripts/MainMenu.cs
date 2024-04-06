using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Should be renamed to some kind of manager thing, because it actually
/// has more responsibilities than just the menus.
/// </summary>
public class MainMenu : MonoBehaviour
{
    public enum ScreenName {START, DEFEAT, VICTORY, CREDITS, BOSS_VALUES}; 
    [SerializeField] private UIObject[] UIElements;
    private Dictionary<ScreenName, List<GameObject>> UIRef;
    // TODO: Document Main Menu
    void Awake() {
        GameManager.instance.mainMenu = this;
        UIRef = new Dictionary<ScreenName, List<GameObject>>();
        foreach (UIObject UIObj in UIElements) {
            if (UIRef.ContainsKey(UIObj.name)) {
                UIRef[UIObj.name].Add(UIObj.gameObject);
            } else {
                UIRef[UIObj.name] = new List<GameObject>{UIObj.gameObject};
            }
        }
        // actually makes the game smoother because
        // garbage collection happens later :)
        // but it's a bad solution, so
        // RESEARCH: Implement an object pool for all the instantiate/destroy stuff
        Application.targetFrameRate = 144;
    }

    void Start() {
        foreach (UIObject UIObj in UIElements) {
            setScreen(UIObj.name, false);
        }

        switch(GameManager.instance.gameState) {
            case GameManager.GameState.START_MENU:
                setScreen(ScreenName.START, true);
                Time.timeScale = 0;
                break;
            case GameManager.GameState.VICTORY:
                setScreen(ScreenName.VICTORY, true);
                StartCoroutine(ExitSequence());
                break;
            default:
                PlayerCam.instance.startUp();
            break;
        }
    }

    public void setScreen(ScreenName key, bool isActive) {
        foreach (GameObject gObj in UIRef[key]) {
            gObj.SetActive(isActive);
        }
    }

    public void StartGame() {
        setScreen(ScreenName.START, false);
        setScreen(ScreenName.BOSS_VALUES, true);
        GameManager.StartGame();
        PlayerCam.instance.startUp();
    }

    public void RestartGame() {
        GameManager.RestartGame();
    }

    public void returnToMaMe(){
        GameManager.returnToMaMe();
    }

    public void Victory(){
        PlayerCam.instance.VictoryTheme();
        StartCoroutine(VictorySequence());
    }

    public void Defeat() {
        StartCoroutine(DefeatSequence());
    }

    public void UpdateBossValues(Tower.Debuff debuff) {
        foreach (GameObject gObj in UIRef[ScreenName.BOSS_VALUES]) {
            UIBar bar= gObj.GetComponent<UIBar>();
            if (bar.barType == debuff) {
                bar.IncrementBar();
            }
        }
    }

    private IEnumerator DefeatSequence() {
        yield return new WaitForSeconds(2f);
        setScreen(ScreenName.DEFEAT, true);
        Time.timeScale = 0;
    }

    private IEnumerator VictorySequence() {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Victory");
        
    }

    private IEnumerator ExitSequence() {
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}

[System.Serializable]
public struct UIObject {
    public MainMenu.ScreenName name;
    public GameObject gameObject;

}