using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public void StartGame(string sceneToLoad) {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToLoad);
    }
    
    public void QuitGame() {
        Debug.Log("we quit game");
        Application.Quit();
    }

}
