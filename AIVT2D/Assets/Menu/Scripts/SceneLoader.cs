using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Button - Search Algorithms
    public void SearchAlgorithms()
    {
        SceneManager.LoadScene(1);
    }

    //Button - Fractal Generation
    public void FractalGeneration()
    {

    }

    //Button - How it Works
    public void HowItWorks()
    {

    }

    //Button - Tool Settings
    public void Settings()
    {

    }

    //Button - Close Program
    public void Quit()
    {
        Application.Quit();
    }
}
