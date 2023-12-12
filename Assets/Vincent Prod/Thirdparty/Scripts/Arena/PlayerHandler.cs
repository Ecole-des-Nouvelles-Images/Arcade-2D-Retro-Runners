using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    public float speed;
    Vector2 moveDirection;
    Vector2 aimDirection;

    private void Update()
    {
        this.transform.position += new Vector3(moveDirection.x,0, moveDirection.y) * speed *  Time.deltaTime;
    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    private void OnAim(InputValue value)
    {
        if (value.Get<Vector2>().magnitude > 0.75)
        {
            aimDirection = value.Get<Vector2>();
            this.transform.rotation = Quaternion.LookRotation(new Vector3(aimDirection.x, 0, aimDirection.y));
        }
    }

    private void OnFire()
    {
        Debug.Log("Fire");
    }

    public void OnCancel()
    {
        Debug.Log("Test");
    }
}
