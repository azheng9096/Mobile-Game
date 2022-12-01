using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathUI : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDeath()
    {
        Time.timeScale = 1f;
        GetComponent<Animation>().Play();
    }
    public void restartGame()
    {
        Time.timeScale = 1f;

        //maybe add a sort of transition animation
        GameStateManager.Instance.SetState(GameState.Cutscene);
        SceneManager.LoadScene("anna_dev");
    }
    public void quitGame()
    {
        Time.timeScale = 1f;
        //maybe add a sort of transition animation
        SceneManager.LoadScene("Start");

    }
}
