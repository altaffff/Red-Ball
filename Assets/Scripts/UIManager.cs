using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public List<Image> heartList = new List<Image>(); // List to store the health images
    public int numberOfHearts = 3; // Starts from 3 and ends at 0 which is no health
    public GameObject gameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("UIManager Instance Created");
        }
        else
        {
            Destroy(gameObject);
        }

        if (gameOver == null)
        {
            Debug.LogError("GameOver GameObject is missing!");
        }

        gameOver.SetActive(false); // Ensure game over is hidden initially
    }

    private void Start()
    {
        // Set number of hearts to 3 at the start of the game
        numberOfHearts = 3;

        // Initialize the level
        LoadCurrentLevel();
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    // Method to load the current level
    private void LoadCurrentLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        string prefabPath = $"Levels/Level {currentLevel}";
        Debug.Log($"Loading Level {currentLevel} from path: {prefabPath}");

        GameObject levelPrefab = Resources.Load<GameObject>(prefabPath);
        if (levelPrefab == null)
        {
            Debug.LogError($"Level prefab not found at path: {prefabPath}. Ensure the prefab exists and the path is correct.");
            return;
        }
        GameObject child = Instantiate(levelPrefab);
    }

    // Health system
    public void UpdateHealthUI()
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            if (numberOfHearts > i)
            {
                heartList[i].gameObject.SetActive(true);
            }
            else
            {
                heartList[i].gameObject.SetActive(false);
            }
        }
    }

    // GameOver System
    public void GameOver()
    {
        Debug.Log("Game Over Triggered");
        gameOver.SetActive(true);
    }
    
    // Restart Button
    public void RestartButton()
    {
        Debug.Log("Restart Button Clicked");
        gameOver.SetActive(false);
        LevelRestart();
    }
    
    // Level Restart
    public void LevelRestart()
    {
        Debug.Log("Level Restart Triggered");
        PlayerPrefs.SetInt("Health", 3); // Reset health to 3
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    // Save current health to PlayerPrefs
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("Health", numberOfHearts);
    }

    // Level Manager
    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        currentLevel++;

        // Limit the total number of levels if needed
        int totalLevels = 3; // For example, 5 levels. You can change this based on your game.
        if (currentLevel > totalLevels)
        {
            currentLevel = 1; // Loop back to level 1 or show a win screen.
        }

        // Save progress before moving to the next level
        SaveProgress();

        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        SceneManager.LoadScene("RedBallGame"); // Change this to your scene name
    }
}
