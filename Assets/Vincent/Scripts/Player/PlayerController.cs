using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        private Vector2 movementInput;

        private void Update()
        {
            transform.Translate(new Vector3(movementInput.x, 0, 0) * speed * Time.deltaTime) ;
        }

        public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
        
    }
}
