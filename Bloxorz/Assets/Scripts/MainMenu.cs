using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject panelMenuPrincipal; // Inicio
    public GameObject panelOpciones;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void EscenaJuego() {
        SceneManager.LoadScene("EscenaJuego");
    }

    public void CargarNivel(string nombreNivel) {
        SceneManager.LoadScene(nombreNivel);
    }

    public void Salir() {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}
