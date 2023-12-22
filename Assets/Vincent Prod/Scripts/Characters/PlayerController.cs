using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Vincent_Prod.Scripts.Managers;

namespace Vincent_Prod.Scripts.Characters
{
    public class PlayerController : MonoBehaviour
    {
        //Movements
        public PlayerInput playerInput;
        public float speed;
        public float rbSpeed;
        public float jumpPower;
        public Vector2 movementInput;
        protected Rigidbody2D _rigidbody2D;
        public BoxCollider2D groundCollider;
        protected bool _isGrounded;
        public int _jumpCount;
        public bool iceArena;
        protected float _baseSpeed;
        
        //Attack
        public float attackTime;
        public float attackCooldown;
        protected bool _canAttack;
        
        //Damage
        protected float _iFrame = 0.2f;
        public int health;
        public int deaths;
        public int kills;
        protected PlayerController _lastPlayerHitMe;
        protected bool _damageTake;
        protected GameObject respawnPoint;
        protected float _respawnTime = 2f;
        protected bool _respawning;
        protected float _attackExplusionForce = 5f;
        protected float _bigAttackExplusionForce = 8f;
        
        //Visuel
        public GameObject RespawnVFX;
        public ParticleSystem ImpactVFX;
        public GameObject DeathParticle;

        //Pointers
        public GameObject upPointer;
        public GameObject leftPointer;
        public GameObject rightPointer;
        
        //Color;
        public Light2D light2D;
        public Color color1 = Color.blue;
        public Color color2 = Color.red;
        public Color color3 = Color.green;
        public Color color4 = Color.yellow;
    }
}
