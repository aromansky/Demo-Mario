using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "Scene Data", order = 51)]
public class SceneTransitionDataScript : ScriptableObject
{
    public Vector3 position = new Vector3(0.54f, -2.46f, 0f);
    public int playerScore = 0;
    public float bonusDuration = 0f;
    public bool wasTransition = false;

    public void Reset()
    {
        position = new Vector3(0.54f, -2.46f, 0f);
        playerScore = 0;
        wasTransition = false;
        bonusDuration = 0f;
    }
}
