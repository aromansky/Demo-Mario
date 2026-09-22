using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private SceneTransitionDataScript sceneData;
    void Awake()
    {
        sceneData.position = transform.position;
    }

}
