using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    private void Start()
    {
        Screen.SetResolution(1280, 720, false);

    }
    public void MoveToScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }
}
