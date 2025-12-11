using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public GameObject panelMenuPrincipal; // Inicio

    [Header("Referencias")]
    public RecordsMenuController recordsMenuController; // Arrastra aquí el objeto que tiene el script de Récords
    public GameObject panelInicio; // Referencia al objeto 'Inicio'
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void EscenaJuego() {
        SceneManager.LoadScene("Level 1");
    }

    public void CargarNivel(string nombreNivel) {
        SceneManager.LoadScene(nombreNivel);
    }

    public void Salir() {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    public void verRecords()
    {
        panelInicio.SetActive(false);
        recordsMenuController.mostrar();
    }
    public void mostrarMenuPrincipal()
    {
        panelInicio.SetActive(true);
    }

}
