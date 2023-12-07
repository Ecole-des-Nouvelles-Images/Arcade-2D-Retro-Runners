using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vincent_Prod.Scripts.Managers;
using Random = UnityEngine.Random;


namespace Vincent_Prod.Scripts.Arenas.Gravity_Arena
{
    public class GravityManager : MonoBehaviour
    {
        private float _originalGravityX = 0f;
        private float _originalGravityY = -25f;
        private int _currentGravityValue;
        private int _newGravityValue;
        private float _timer;
        public static bool GravityRight;
        public static bool GravityLeft;
        private PlayerManager _playerManager;

        private void Start()
        {
            _playerManager = FindObjectOfType<PlayerManager>();
            GravityRight = false;
            GravityLeft = false;
            Physics2D.gravity = new Vector2(_originalGravityX, _originalGravityY);
            GameObject Activeplayers = GameObject.FindGameObjectWithTag("Player");
            foreach (var player in _playerManager.Players) {
                player.transform.Rotate(0,0,0);
            }
        }
        private void Update() {
            _timer += Time.deltaTime;
            if (_timer >= 30) {
                ChooseNewGravity();
                _timer = 0;
            }
        }
        private void ChooseNewGravity() {
            _newGravityValue = Random.Range(0, 4);
            if (_newGravityValue == _currentGravityValue) {
                ChooseNewGravity();
            }
            else {
                ChangeNewGravity();
                _currentGravityValue = _newGravityValue;
                GravityLeft = false;
                GravityRight = false;
            }
        }
        private void ChangeNewGravity() {
            if (_newGravityValue == 0) StartCoroutine(GravityToDown());
            if (_newGravityValue == 1) StartCoroutine(GravityToUp());
            if (_newGravityValue == 2) StartCoroutine(GravityToRight());
            if (_newGravityValue == 3) StartCoroutine(GravityToLeft());
        }
        private IEnumerator GravityToDown() {
            yield return new WaitForSeconds(0.1f);
            foreach (var player in _playerManager.Players) {
                float playerGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
                player.GetComponent<Rigidbody2D>().AddForce(transform.up * 5, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.1f);
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.transform.localRotation = Quaternion.Euler(0, 0, 0);
                player.GetComponent<Rigidbody2D>().gravityScale = playerGravityScale;
            }
            yield return new WaitForSeconds(0.1f);
            Physics2D.gravity= new Vector2(0,-25);
        }
        private IEnumerator GravityToUp() {
            yield return new WaitForSeconds(0.1f);
            foreach (var player in _playerManager.Players) {
                float playerGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
                player.GetComponent<Rigidbody2D>().AddForce(transform.up * 5, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.1f);
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.transform.localRotation = Quaternion.Euler(0, 0, 180);
                player.GetComponent<Rigidbody2D>().gravityScale = playerGravityScale;
            }
            yield return new WaitForSeconds(0.1f);
            Physics2D.gravity= new Vector2(0,25);
        }
        private IEnumerator GravityToRight() {
            yield return new WaitForSeconds(0.1f);
            GravityRight = true;
            foreach (var player in _playerManager.Players) {
                float playerGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
                player.GetComponent<Rigidbody2D>().AddForce(transform.up * 5, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.1f);
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.transform.localRotation = Quaternion.Euler(0, 0, 90);
                player.GetComponent<Rigidbody2D>().gravityScale = playerGravityScale;
            }
            yield return new WaitForSeconds(0.1f);
            Physics2D.gravity= new Vector2(25,0);
        }
        private IEnumerator GravityToLeft() {
            yield return new WaitForSeconds(0.1f);
            GravityLeft = true;
            foreach (var player in _playerManager.Players) {
                float playerGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
                player.GetComponent<Rigidbody2D>().AddForce(transform.up * 5, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.1f);
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.transform.localRotation = Quaternion.Euler(0, 0, -90);
                player.GetComponent<Rigidbody2D>().gravityScale = playerGravityScale;
            }
            yield return new WaitForSeconds(0.1f);
            Physics2D.gravity= new Vector2(-25,0);
        }
    }
}

