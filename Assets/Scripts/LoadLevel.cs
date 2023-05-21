using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class LoadLevel : MonoBehaviour
{
    public bool loadLevel;
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private async void LoadLevelAsync()
    {
        if (loadLevel == true)
        {
            if (SceneManager.sceneCount == 1)
            {
                var progress = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
                await progress;
            }
            else
            {
                var progress = SceneManager.UnloadSceneAsync(levelName);
                await progress;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadLevelAsync();
        }
    }
}
