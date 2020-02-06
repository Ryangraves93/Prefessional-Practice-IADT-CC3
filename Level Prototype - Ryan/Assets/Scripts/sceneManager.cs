using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneManager : MonoBehaviour
{
    int currScene;

    private void Awake()
    {
        currScene = SceneManager.GetActiveScene().buildIndex;
    }
    
    public void MenuScene()
    {
        SceneManager.LoadScene(0);
        currScene = SceneManager.GetActiveScene().buildIndex;
    }
    public void LevelSelect()
    {
        SceneManager.LoadScene(1);
        currScene = SceneManager.GetActiveScene().buildIndex;
    }
    public void Level1()
    {
        SceneManager.LoadScene(2);
        currScene = SceneManager.GetActiveScene().buildIndex;
    }
    public void Level2()
    {
        SceneManager.LoadScene(3);
        currScene = SceneManager.GetActiveScene().buildIndex;
    }
    public void nextLevel()
    {
        SceneManager.LoadScene(currScene+1);
        currScene = SceneManager.GetActiveScene().buildIndex;
    }
    public void Retry()
    {
        SceneManager.LoadScene(currScene);

    }
}
