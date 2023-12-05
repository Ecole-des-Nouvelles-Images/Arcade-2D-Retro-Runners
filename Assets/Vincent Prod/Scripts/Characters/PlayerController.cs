using UnityEngine;
using UnityEngine.InputSystem;

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
        protected int _jumpCount;
        public bool iceArena;
        
        //Attack
        public float attackTime;
        public float attackCooldown;
        protected bool _canAttack;
        
        //Damage
        protected float _iFrame = 0.2f;
        public int health;
        public int deaths;
        protected bool _damageTake;
        protected GameObject respawnPoint;
        protected float _respawnTime = 1f;
        protected bool _respawning;

        //Pointers
        public GameObject upPointer;
        public GameObject leftPointer;
        public GameObject rightPointer;
        
        //Color;
        public Color color1 = Color.blue;
        public Color color2 = Color.red;
        public Color color3 = Color.green;
        public Color color4 = Color.yellow;
    }
}
