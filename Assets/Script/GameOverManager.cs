using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

    // Use this for initialization
    public void Quit() {
        Debug.Log("Application.quit");
        Application.Quit();
    }
    public void Retry() {
        Debug.Log("Restarting");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}