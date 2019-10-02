using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

    [SerializeField]
    private WaveSpawner spawner;

    [SerializeField]
    private Animator waveAnimator;

    [SerializeField]
    private Text waveCountdownText;

    [SerializeField]
    private Text waveCountText;

    private WaveSpawner.SpawnState previousState;

	// Use this for initialization
	void Start () {
		if (spawner == null) {
            Debug.LogError("Spawner null");
        }
        if (waveAnimator == null) {
            Debug.LogError("Wave animator null");
        }
        if (waveCountdownText == null) {
            Debug.LogError("Wave countdown text null");
        }
        if (waveCountText == null) {
            Debug.LogError("Wave count text null");
        }
    }
	
	// Update is called once per frame
	void Update () {
		switch(spawner.State) {
            case WaveSpawner.SpawnState.counting:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.spawning:
                UpdateSpawningUI();
                break;
        }
        previousState = spawner.State;
    }



    void UpdateCountingUI() {
        if (previousState != WaveSpawner.SpawnState.counting) {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
            //Debug.Log("Counting");
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown + 1).ToString();
    }

    void UpdateSpawningUI() {
        if (previousState != WaveSpawner.SpawnState.spawning) { 
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = (spawner.NextWave + 1).ToString();

            //Debug.Log("Spawning");
        }
    }
}
