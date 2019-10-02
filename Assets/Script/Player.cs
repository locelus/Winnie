using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    
    [System.Serializable]          
    public class PlayerStats {     
        public float maxHealth = 100f;
                                   
                                   
        private float _curHealth;  
        public float curHealth {   
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }                          
                                   
        public void Init() {       
            curHealth = maxHealth; 
        }                          
                                   
    }                              
                                   
    public PlayerStats stats = new PlayerStats();
                                   
    public int fallBoundary = -20;
                                   
    [SerializeField]               
    private StatusIndicator statusIndicator;
    

    private void Start() {         
        stats.Init();
        if (statusIndicator == null) {
            Debug.LogError("No status indicator found on Player object");
        } else {                   
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
        GameMaster.playerObject = this.gameObject;
    }

    void Update() {
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(9999999);
        }
    }

    public void DamagePlayer (float damage) {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0) {
            GameMaster.KillPlayer(this);
            Debug.Log("KILL PLAYER");
        }

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

}
