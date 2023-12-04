using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vincent.Scripts.Archer;
using Vincent.Scripts.Knight;
using Vincent.Scripts.Player;
using PlayerController = Vincent.Scripts.Player.PlayerController;


public class UILifeBars : MonoBehaviour
    {
        private UILifeBarsManager _uiLifeBarsManager;
        private int _listID;
        private PlayerController _myPlayer;
        private KnightController _myKnight;
        private ArcherController _myArcher;

        public TextMeshProUGUI lifeText;
        public TextMeshProUGUI diesText;
        public Image panelImage;
        public Image portraitImage;
        private void Awake() {
            _uiLifeBarsManager = GetComponentInParent<UILifeBarsManager>();
            _uiLifeBarsManager.LifeBars.Add(this);
            _listID = _uiLifeBarsManager.LifeBars.Count;
            foreach (GameObject player in _uiLifeBarsManager.playerManager.Players)
            {
                if (player.GetComponent<PlayerController>())
                {
                    _myPlayer = player.GetComponent<PlayerController>();
                }
                if (player.GetComponent<KnightController>())
                {
                    _myPlayer = player.GetComponent<KnightController>();
                }
                if (player.GetComponent<ArcherController>())
                {
                    _myPlayer = player.GetComponent<ArcherController>();
                }
            }
            Color tempColor = _myPlayer.GetComponent<SpriteRenderer>().color;
            panelImage.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.7f);
        }

        private void Start()
        {
            if (_myPlayer.GetComponent<KnightController>())
            {
                portraitImage.sprite = _myPlayer.GetComponent<KnightController>().portrait;
            }
            if (_myPlayer.GetComponent<ArcherController>())
            {
                portraitImage.sprite = _myPlayer.GetComponent<ArcherController>().portrait;
            }
        }

        private void FixedUpdate()
        {
            if (_myPlayer.GetComponent<KnightController>()) {
                lifeText.text = _myPlayer.GetComponent<KnightController>().health.ToString();
                diesText.text = _myPlayer.GetComponent<KnightController>().Dies.ToString();
            }
            if (_myPlayer.GetComponent<ArcherController>()) {
                lifeText.text = _myPlayer.GetComponent<ArcherController>().health.ToString();
                diesText.text = _myPlayer.GetComponent<ArcherController>().Dies.ToString();
            }
            
        }
    }

