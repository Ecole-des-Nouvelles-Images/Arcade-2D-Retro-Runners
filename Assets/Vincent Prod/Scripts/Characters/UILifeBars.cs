using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Vincent_Prod.Scripts.Managers;

namespace Vincent_Prod.Scripts.Characters {
    public class UILifeBars : MonoBehaviour {
        private UILifeBarsManager _uiLifeBarsManager;
        private int _listID;
        private PlayerController _myPlayer;
        public TextMeshProUGUI lifeText;
        public TextMeshProUGUI killsText;
        public Image panelImage;
        public Image portraitImage;
        private void Awake() {
            _uiLifeBarsManager = GetComponentInParent<UILifeBarsManager>();
            _uiLifeBarsManager.LifeBars.Add(this);
            _listID = _uiLifeBarsManager.LifeBars.Count;
            foreach (GameObject player in _uiLifeBarsManager.playerManager.Players) {
                if (player.GetComponent<Knight>()) { _myPlayer = player.GetComponent<Knight>(); }
                else if (player.GetComponent<Archer>()) { _myPlayer = player.GetComponent<Archer>(); }
                else if (player.GetComponent<Paladin>()) { _myPlayer = player.GetComponent<Paladin>(); }
                else if (player.GetComponent<Mage>()) { _myPlayer = player.GetComponent<Mage>(); }
                Color tempColor = _myPlayer.GetComponentInChildren<Light2D>().color;
                panelImage.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.2f);
            }
        }

        private void Start() {
            
            if (_myPlayer.GetComponent<Knight>()) { portraitImage.sprite = _myPlayer.GetComponent<Knight>().portrait; } 
            if (_myPlayer.GetComponent<Archer>()) { portraitImage.sprite = _myPlayer.GetComponent<Archer>().portrait; }
            if (_myPlayer.GetComponent<Paladin>()) { portraitImage.sprite = _myPlayer.GetComponent<Paladin>().portrait; }
            if (_myPlayer.GetComponent<Mage>()) { portraitImage.sprite = _myPlayer.GetComponent<Mage>().portrait; }
        }
        private void FixedUpdate() {
            if (_myPlayer.GetComponent<Knight>()) {
                lifeText.text = _myPlayer.GetComponent<Knight>().health.ToString();
                killsText.text = _myPlayer.GetComponent<Knight>().kills.ToString();
            }
            else if (_myPlayer.GetComponent<Archer>()) {
                lifeText.text = _myPlayer.GetComponent<Archer>().health.ToString();
                killsText.text = _myPlayer.GetComponent<Archer>().kills.ToString();
            }
            else if (_myPlayer.GetComponent<Paladin>()) {
                lifeText.text = _myPlayer.GetComponent<Paladin>().health.ToString();
                killsText.text = _myPlayer.GetComponent<Paladin>().kills.ToString();
            }
            else if (_myPlayer.GetComponent<Mage>()) {
                lifeText.text = _myPlayer.GetComponent<Mage>().health.ToString();
                killsText.text = _myPlayer.GetComponent<Mage>().kills.ToString();
            }
        }
    }
}

