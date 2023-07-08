using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM { get; private set; }
    // [SerializeField] List<string> algorithmSceneNames;
    // [SerializeField] AlgorithmNames selectedAlgo;

    // AsyncOperation loadingAlgoScene;
    void Awake()
    {
        if (GM != null && GM != this)
        {
            Destroy(this);
            return;
        }
        GM = this;
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator LoadingAlgoScene()
    {
        yield return null;
    }
}
