using System;
using System.Collections;
using UnityEngine;

public class WaterDrowning : MonoBehaviour
{
    private Rigidbody2D playerrb;
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerrb = player.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Mass is getting effected ");
            playerrb.mass = 100f;
            StartCoroutine(Drowning());

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerrb.mass = 1f;
    }
    
    IEnumerator Drowning()
    {
        yield return new WaitForSeconds(3f);
        UIManager.instance.GameOver();
    }
}
