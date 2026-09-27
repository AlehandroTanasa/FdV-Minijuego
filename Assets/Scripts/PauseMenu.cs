using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour{

    /*
    Tiene un gameObject que guarda un container de los elementos del menu
    */
    public GameObject containerMenuPausa;

    /* Update is called once per frame

    Comprueba si ha pulsado el escape, para el tiempo y activa el menu en pantalla
    */
    void Update(){
        if(Keyboard.current.escapeKey.wasPressedThisFrame){
            Time.timeScale = 0f;
            containerMenuPausa.SetActive(true);
        }
    }

    /*
    Funciona que se le asigna al boton de la interfaz del menú con la opción "On click".
    Hace que el tiempo siga avanzando y esconde el menu.
    */
    public void ResumeGame(){
        Time.timeScale = 1f;
        containerMenuPausa.SetActive(false);
    }   
}
