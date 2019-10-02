using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    public static GameObject playerObject;

    
    private static int _remainingLives = 3;
    public static int remainingLives {
        get { return _remainingLives; }
    }

    void Awake() {
        _remainingLives = 3;
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }
        if (playerObject == null) {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public AudioSource respawnTimer;

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject GameOverUI;

    public void Start() {
        if (cameraShake == null) {
            Debug.LogError("No camera shake referenced in Game Master");
        }
    }

    public void EndGame() {
        Debug.Log("Game Over man");
        GameOverUI.SetActive(true);
    }

    public void LoadScene(string sceneToLoad) {
        SceneManager.LoadScene(sceneToLoad);
    }

    public IEnumerator _RespawnPlayer() {
        respawnTimer.Play();
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(clone, 3f);
    }
    
    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        _remainingLives--;
        if (_remainingLives <= 0) {
            gm.EndGame();
        }
        else {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy) {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy) {
        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
        Destroy(_clone.gameObject, 5f);
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
