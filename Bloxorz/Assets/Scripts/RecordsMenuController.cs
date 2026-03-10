using System;
using System.Data;
using UnityEngine;

public class RecordsMenuController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelRecordsVisual;
    public MainMenuController mainMenuController;

    [Header("Configuración de la Lista")]
    public Transform contenedorLista;   // Aquí arrastrarás el "Content" del ScrollView
    public GameObject prefabFilaRecord; // Aquí arrastrarás el Prefab de la fila

    // Variable interna para manejar la base de datos
    private IRecordDAO recordDAO;

    void Start()
    {
        // 1. Inicializamos la conexión a la base de datos al arrancar el juego
        recordDAO = new SQLiteRecordDAO();
        //InsertarDatosDePrueba();
    }

    public void mostrar()
    {
        // Si la conexión no existe aún (porque es la primera vez), la creamos al instante
        if (recordDAO == null)
        {
            recordDAO = new SQLiteRecordDAO();
        }

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
        if (mainMenuController != null)
        {
            mainMenuController.mostrarMenuPrincipal();
        }
    }

    private void CargarDatos()
    {
        // Limpiar lista visual (Borra los récords viejos por si entras y sales del menú varias veces)
        foreach (Transform hijo in contenedorLista)
        {
            Destroy(hijo.gameObject);
        }

        // Obtener datos de SQLite
        IDataReader reader = recordDAO.obtenerRanking();

        while (reader.Read())
        {
            // OJO al orden del SELECT: 0 = nombre, 1 = tiempo, 2 = fecha
            string nombre = reader.GetString(0);
            float tiempo = reader.GetFloat(1);
            DateTime fecha = reader.GetDateTime(2);

            // Crear fila visual
            GameObject nuevaFila = Instantiate(prefabFilaRecord, contenedorLista, false);

            // ⚠️ TRUCO ANTI-MÁSCARA: Forzamos la escala a 1 y la profundidad Z a 0
            nuevaFila.transform.localScale = Vector3.one;
            Vector3 posLocal = nuevaFila.transform.localPosition;
            nuevaFila.transform.localPosition = new Vector3(posLocal.x, posLocal.y, 0f);

            // Pasamos los datos a tu script de la fila
            FilaRecordView scriptVista = nuevaFila.GetComponent<FilaRecordView>();
            if (scriptVista != null)
            {
                scriptVista.ConfigurarFila(nombre, tiempo, fecha);
            }
        }

        // Importante cerrar el reader para no bloquear la base de datos
        reader.Close();
    }
    private void InsertarDatosDePrueba()
    {
        // Instanciamos el DAO de jugadores para poder crearlos
        IJugadorDAO jugadorDAO = new SQLiteJugadorDAO();

        // 1. Insertamos un par de jugadores de prueba
        jugadorDAO.insertar("Lautaro");
        jugadorDAO.insertar("Carlos");

        // 2. Obtenemos qué ID les asignó la base de datos
        int idLautaro = jugadorDAO.obtenerIdPorNombre("Lautaro");
        int idCarlos = jugadorDAO.obtenerIdPorNombre("Carlos");

        // 3. Les guardamos partidas inventadas (usando DateTime.Now para la fecha)
        if (idLautaro != -1)
        {
            recordDAO.insertarPartida(idLautaro, 45.2f, DateTime.Now.AddDays(-2)); // Partida de hace 2 días
            recordDAO.insertarPartida(idLautaro, 38.5f, DateTime.Now);             // Partida de hoy (mejor tiempo)
        }

        if (idCarlos != -1)
        {
            recordDAO.insertarPartida(idCarlos, 50.1f, DateTime.Now.AddDays(-1));
        }

        Debug.Log("¡Datos de prueba inyectados con éxito en SQLite!");
    }
}