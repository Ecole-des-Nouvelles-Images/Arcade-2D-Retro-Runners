using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vincent_Prod.Scripts.Characters {
    public class UiResultPanel : MonoBehaviour {
        public TextMeshProUGUI rankNumber;
        public Image portraitImage;
        public TextMeshProUGUI killsNumber;
        public TextMeshProUGUI deathNumber;
        public List<Image> panelImages = new List<Image>();
    }
}
