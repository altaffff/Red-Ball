using System;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;
    public GameObject player;
    public Transform respawnPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("RespawnManager Instance Created");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (respawnPoint != null)
            {
                Debug.Log("Respawning Player at: " + respawnPoint.position);
                player.transform.position = respawnPoint.position; // Move player to respawn point's position
            }
            else
            {
                Debug.LogError("Respawn Point is null");
            }
        }
    }
}