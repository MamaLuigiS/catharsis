using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPosition = new Vector3(0, 1, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Respawn position: " + respawnPosition);
    }

    public void RespawnPlayer()
    {
        transform.position = respawnPosition;
        Debug.Log("RespawnPlayer");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("Respawn"))
        {
            RespawnPlayer();
            Debug.Log("RespawnPlayer Trigger Activated");
        }
    }
}
