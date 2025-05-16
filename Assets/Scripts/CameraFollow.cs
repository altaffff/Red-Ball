using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 5f; // Speed of camera movement
    [SerializeField] private Transform playerPos; // Player's position
    [SerializeField] private float camHeight = 2f; // Camera height above the player
    [SerializeField] private float camPosOffset = 0f; // Horizontal offset from the player
    [SerializeField] private float horizontalPadding = 2f; // Padding on the left and right
    [SerializeField] private float verticalPadding = 2f; // Padding on the top and bottom
    private Transform player;
    private Camera cam;

    private void Start()
    {
        // Ensure the player and camera are correctly assigned
        if (PlayerController.instance != null)
        {
            player = PlayerController.instance.transform;
        }
        else
        {
            Debug.LogError("PlayerController instance is null. Make sure PlayerController is correctly initialized.");
        }

        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main Camera is not found. Make sure there is a camera tagged as MainCamera.");
        }
    }

    private void LateUpdate()
    {
        if (CamDontFollow.instance == null || !CamDontFollow.instance.stopCam)
        {
            FollowPlayerWithPadding();
        }
    }

    private void FollowPlayerWithPadding()
    {
        if (player != null)
        {
            // Get the half width and height of the camera view in world units
            float screenHalfWidth = cam.orthographicSize * cam.aspect;
            float screenHalfHeight = cam.orthographicSize;

            // Calculate minimum and maximum X values for the camera position
            float minX = player.position.x - screenHalfWidth + horizontalPadding;
            float maxX = player.position.x + screenHalfWidth - horizontalPadding;

            // Calculate minimum and maximum Y values for the camera position
            float minY = player.position.y - screenHalfHeight + verticalPadding;
            float maxY = player.position.y + screenHalfHeight - verticalPadding;

            // Clamp the target X and Y positions within the min and max bounds
            float targetX = Mathf.Clamp(player.position.x + camPosOffset, minX, maxX);
            float targetY = Mathf.Clamp(player.position.y + camHeight, minY, maxY);

            Vector3 targetPosition = new Vector3(
                targetX,
                targetY,
                -10f
            );

            // Smoothly move the camera towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogError("Player position is null. Ensure the player is assigned correctly.");
        }
    }

    // Method to update the camera target to a new player
    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
        playerPos = newPlayer; // Ensure playerPos is also updated
    }
}
