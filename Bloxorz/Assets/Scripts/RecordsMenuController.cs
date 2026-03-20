using System;
using System.Data;
using UnityEngine;
using TMPro; // Necesario para el InputField

public class RecordsMenuController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelRecordsVisual;
    public MainMenuController mainMenuController;

    [Header("Configuración de la Lista")]
    public Transform contenedorLista;
    public GameObject prefabFilaRecord;

    [Header("Edición de Récord")]
    public GameObject panelEditarVisual; // Un panel pequeño que crearemos ahora
    public TMP_InputField inputEdicionNombre; // Donde el usuario escribirá el nuevo nombre
    private int idRecordAEditar = -1;

    private IRecordDAO recordDAO;

    void Start()
    {
        recordDAO = new SQLiteRecordDAO();
    }

    public void mostrar()
    {
        if (recordDAO == null) recordDAO = new SQLiteRecordDAO();
        panelRecordsVisual.SetActive(true);
        CargarDatos();
    }

    public void ocultar()
    {
        panelRecordsVisual.SetActive(false);
    }

    public void cerrarVentanaRecords()
    {
        ocultar();
        if (mainMenuController != null) mainMenuController.mostrarMenuPrincipal();
    }

    private void CargarDatos()
    {
        foreach (Transform hijo in contenedorLista) Destroy(hijo.gameObject);

        IDataReader reader = recordDAO.obtenerRanking();

        while (reader.Read())
        {
            int idRecord = reader.GetInt32(0);
            string nombre = reader.GetString(1);
            float tiempo = reader.GetFloat(2);
            DateTime fecha = reader.GetDateTime(3);

            GameObject nuevaFila = Instantiate(prefabFilaRecord, contenedorLista, false);
            nuevaFila.transform.localScale = Vector3.one;
            nuevaFila.transform.localPosition = new Vector3(nuevaFila.transform.localPosition.x, nuevaFila.transform.localPosition.y, 0f);

            FilaRecordView scriptVista = nuevaFila.GetComponent<FilaRecordView>();
            if (scriptVista != null)
            {
                // 🔹 AQUÍ ESTÁ LA MAGIA: Le pasamos las funciones directamente a la fila
                scriptVista.ConfigurarFila(idRecord, nombre, tiempo, fecha);
            }
        }
        reader.Close();
    }
}