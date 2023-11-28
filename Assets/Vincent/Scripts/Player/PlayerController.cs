using System;
using System.Collections;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vincent.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        public Color color1 = Color.blue;
        public Color color2 = Color.red;
        public Color color3 = Color.green;
        public Color color4 = Color.yellow;
        private PlayerManager _playerManager;
    }
    
    
}
