using UnityEngine;

namespace Vincent_Prod.Scripts.Characters
{
    public class PaladinShieldSwap : MonoBehaviour
    {
        public Sprite _shieldTroisQuarts;
        public Sprite _shieldFace;
        public SpriteRenderer _shieldSprite;
        public void SpriteShieldFace() {
            _shieldSprite.sprite = _shieldFace;
        }
        public void SpriteShieldTroisQuarts() {
            _shieldSprite.sprite = _shieldTroisQuarts;
        }
    }
}
