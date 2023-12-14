using UnityEngine;

namespace Vincent_Prod.Scripts.Characters
{
    public class OutlineSprite : MonoBehaviour
    {
        public Transform parentTransform;
        void Update() {
            if (parentTransform != null)
            {
                transform.position = parentTransform.position;
                transform.rotation = parentTransform.rotation;
            }
        }
    }
}
