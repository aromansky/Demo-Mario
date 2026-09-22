using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private int targetSceneId;
    [SerializeField] private SceneTransitionDataScript sceneData;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            sceneData.wasTransition = true;
            SceneManager.LoadScene(targetSceneId);
        }
    }
}
