using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{

    /*
    Tiene un gameObject que guarda un container de los elementos del menu
    */
    public GameObject containerGameOverMenu;

    /*
    - Show: Para el tiempo y muestra el menú
    - Hide: Recupera el tiempo y esconde el menú
    - Restart: Reinicia la partida

    La implementación del menú de gameOver es distinta a la del menu de pausa ya que comprobar en cada update si el jugador
    había muerto me parecía estúpido, asique he asignado el menu como gameObject de player y he llamado a estos métodos
    desde player cuando la hitbox de anve toca un asteroide. Los métodos Hide y Restart son para el botón de la interfaz
    del menú del gameOver, asignados con la opción "On click".
    */
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
