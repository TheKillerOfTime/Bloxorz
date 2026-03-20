using System;
using System.Data;
using UnityEngine;

public class RecordsMenuController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelRecordsVisual;
    public MainMenuController mainMenuController;

    // 🔹 NUEVO: Referencia a tu texto de "No hay récords"
    public GameObject mensajeSinRecords;

    [Header("Configuración de la Lista")]
    public Transform contenedorLista;
    public GameObject prefabFilaRecord;

    private IRecordDAO recordDAO;

    void Start()
    {
        recordDAO = new SQLiteRecordDAO();
        Debug.Log("Ruta de la BDD: " + Application.persistentDataPath);
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
        // 1. Limpiamos la lista visual vieja
        foreach (Transform hijo in contenedorLista) Destroy(hijo.gameObject);

        IDataReader reader = recordDAO.obtenerRanking();

        // 🔹 NUEVO: Creamos un contador para saber si encontramos algo
        int cantidadRecords = 0;

        while (reader.Read())
        {
            cantidadRecords++; // Sumamos 1 por cada fila que exista

            // Nota: Asumo que tu SELECT trae: 0=id, 1=nombre, 2=tiempo, 3=fecha.
            // Si le quitaste el ID a tu SELECT, ajusta los números (0=nombre, 1=tiempo, 2=fecha)
            string nombre = reader.GetString(1);
            float tiempo = reader.GetFloat(2);
            DateTime fecha = reader.GetDateTime(3);

            GameObject nuevaFila = Instantiate(prefabFilaRecord, contenedorLista, false);
            nuevaFila.transform.localScale = Vector3.one;
            nuevaFila.transform.localPosition = new Vector3(nuevaFila.transform.localPosition.x, nuevaFila.transform.localPosition.y, 0f);

            FilaRecordView scriptVista = nuevaFila.GetComponent<FilaRecordView>();
            if (scriptVista != null)
            {
                // Como quitaste los botones, volvemos a la configuración simple
                scriptVista.ConfigurarFila(nombre, tiempo, fecha);
            }
        }
        reader.Close();

        if (mensajeSinRecords != null)
        {
            mensajeSinRecords.SetActive(cantidadRecords == 0);
        }
    }
}