using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Transform checkPoint;
    public static CheckPoint instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("CheckPoint Instance Created");
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
            Debug.Log("CheckPoint Triggered by Player");
            RespawnManager.instance.respawnPoint = checkPoint; // Assign the checkpoint's transform
        }
    }
}