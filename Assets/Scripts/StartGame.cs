using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private SceneTransitionDataScript sceneData;
    private void Start()
    {
    }

    private void Update()
    {
    }

    public void StartTheGame()
    {
        sceneData.Reset();
        SceneManager.LoadScene(1);
    }
}
