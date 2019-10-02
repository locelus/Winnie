using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats {
        public float maxHealth = 100;

        private float _curHealth;
        public float curHealth {
            get { return _curHealth; }
            set { _curHealth = Mathf.RoundToInt(Mathf.Clamp(value, 0f, maxHealth)); }
        }

        public float damage = 40;

        public void Init() {
            curHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;

    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    public void Start() {
        stats.Init();

        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        if (deathParticles == null) {
            Debug.LogError("No death particles found for enemy object");
        }
    }

    public void DamageEnemy(float damage) {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0) {
            GameMaster.KillEnemy(this);
            Debug.Log("KILL ENEMY");
        }
        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo) {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if (_player != null) {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(999999999);
        }
    }
}
