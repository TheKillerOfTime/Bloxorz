using UnityEngine;

public class RecordsMenuController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelRecordsVisual; // Asigna aquí el 'PanelRecords'

    // Opcional: Referencia al MainMenu para volver a activarlo
    public MainMenuController mainMenuController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void mostrar()
    {
        panelRecordsVisual.SetActive(true);
        // Aquí luego llamaremos a la lógica para cargar datos, pero primero el front.
    }

    // Método del diagrama
    public void ocultar()
    {
        panelRecordsVisual.SetActive(false);
    }

    // Método del diagrama: Se asignará al botón "Volver"
    public void cerrarVentanaRecords()
    {
        ocultar();
        mainMenuController.mostrarMenuPrincipal();

    }
}

