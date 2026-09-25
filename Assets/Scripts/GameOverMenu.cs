using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{

    public GameObject containerGameOverMenu;

    public void Show(){
        Time.timeScale = 0f;
        containerGameOverMenu.SetActive(true);
    }

    public void Hide(){
        Time.timeScale = 1f;
        containerGameOverMenu.SetActive(false);
    }

    public void RestartGame(){
        Hide();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
