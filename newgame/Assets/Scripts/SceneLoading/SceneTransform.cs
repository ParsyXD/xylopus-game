using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransform : MonoBehaviour
{
    public string sceneName;
    public bool isLoaded;
    public int buildIndex;

    
    [ContextMenu("GetBuildIndex")]
    void GetBuildIndex()
    {
        int bi = SceneManager.GetSceneByName(sceneName).buildIndex;
        buildIndex = bi;
    }
}
