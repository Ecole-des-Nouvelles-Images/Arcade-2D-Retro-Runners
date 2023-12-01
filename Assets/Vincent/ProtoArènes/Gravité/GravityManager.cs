using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Vincent.Scripts.Player;

public class GravityManager : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    private void Update()
    {
        
    }

    [ContextMenu("Gravity Up")]
    public void GravityToUp()
    {
        Physics2D.gravity = -Physics2D.gravity;
        players.Clear();
        GameObject Activeplayers = GameObject.FindGameObjectWithTag("Player");
        players.Add(Activeplayers);
        foreach (var player in players)
        {
            player.transform.Rotate(0,0,180);
        }
    }
}
