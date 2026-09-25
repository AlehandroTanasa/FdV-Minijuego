using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour{

    public GameObject containerMenuPausa;

    // Update is called once per frame
    void Update(){
        if(Keyboard.current.escapeKey.wasPressedThisFrame){
            Time.timeScale = 0f;
            containerMenuPausa.SetActive(true);
        }
    }

    public void ResumeGame(){
        Time.timeScale = 1f;
        containerMenuPausa.SetActive(false);
    }   
}
