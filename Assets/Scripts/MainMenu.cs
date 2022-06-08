using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartSinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void StartTwoPlayers() {
        SceneManager.LoadScene("TwoPlayers");
    }



}
