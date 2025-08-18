using UnityEngine;

using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject panelMenuPrincipal; // Inicio
    public GameObject panelOpciones;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void AbrirOpciones()
    {
        Debug.Log(">>> AbrirOpciones ejecutado");
        panelMenuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }
    public void CerrarOpciones()
    {
        Debug.Log(">>> CerrarOpciones ejecutado");
        panelOpciones.SetActive(false);
        panelMenuPrincipal.SetActive(true);
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
        Application.Quit();
    }
}
