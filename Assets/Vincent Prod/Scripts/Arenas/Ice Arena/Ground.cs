using System;
using Unity.Mathematics;
using UnityEngine;

namespace Vincent_Prod.Scripts.Arenas.Ice_Arena
{
    public class Ground : MonoBehaviour
    {
        
        public float maxRotation = 25f;
        void Update() {
            transform.position = new Vector3(0f, -5.9f, 0f);
        }
        private void FixedUpdate()
        {
            float currentRotation = transform.rotation.z;

            if (currentRotation > maxRotation) {
                Debug.Log("upper 25");
                transform.rotation = quaternion.Euler(0,0,maxRotation - 1);
            }
            if (currentRotation < -maxRotation) {
                Debug.Log("under -25");
                transform.rotation = quaternion.Euler(0,0,-maxRotation + 1);
            }
            else transform.Rotate(new Vector3(0,0,-transform.rotation.z) * 50 * Time.deltaTime);
        }
    }
}
