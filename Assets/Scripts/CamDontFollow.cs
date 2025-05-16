using System;
using UnityEngine;

public class CamDontFollow : MonoBehaviour
{
    public static CamDontFollow instance;
    public bool stopCam = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Trigger to stop the camera
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stopCam = true;
        }
    }

    // Trigger to resume the camera
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stopCam = false;
        }
    }
}