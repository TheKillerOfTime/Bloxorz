using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MenuVictoriaController : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_Text textoTiempoFinal;
    public TMP_InputField inputNombre;
    public TMP_Text textoErrorNombre; // <-- NUEVA VARIABLE PARA EL ERROR

    [Header("Configuración de Escenas")]
    public string nombreEscenaMenuPrincipal = "Main Menu";
    public string nombreEscenaNivel1 = "Level 1";

    private float tiempoFinalFloat;
    private IJugadorDAO jugadorDAO;
    private IRecordDAO recordDAO;

    void Start()
    {
        jugadorDAO = new SQLiteJugadorDAO();
        recordDAO = new SQLiteRecordDAO();

        if (textoErrorNombre != null)
        {
            textoErrorNombre.gameObject.SetActive(false);
        }

        if (inputNombre != null)
        {
            inputNombre.onValueChanged.AddListener(delegate { OcultarError(); });
        }

        // 🔹 LA SOLUCIÓN: Leemos el tiempo guardado directamente, sin preguntar por el GameManager
        tiempoFinalFloat = PlayerPrefs.GetFloat("TiempoFinalJuego", 0f);

        int horas = Mathf.FloorToInt(tiempoFinalFloat / 3600);
        int minutos = Mathf.FloorToInt((tiempoFinalFloat % 3600) / 60);
        int segundos = Mathf.FloorToInt(tiempoFinalFloat % 60);
        int milisegundos = Mathf.FloorToInt((tiempoFinalFloat * 1000) % 1000);

        textoTiempoFinal.text = string.Format("Tiempo: {0:00}:{1:00}:{2:00}:{3:000}", horas, minutos, segundos, milisegundos);
    }

    // Esta función se llama sola cada vez que el jugador teclea una letra
    private void OcultarError()
    {
        if (textoErrorNombre != null && textoErrorNombre.gameObject.activeSelf)
        {
            textoErrorNombre.gameObject.SetActive(false);
        }
    }

    private void LimpiarGameManager()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.cronometro.resetTimer();
            GameManager.nivel = 1;
            GameManager.gameOver = false;
        }
    }

    // --- BOTÓN 1: GUARDAR Y VOLVER ---
    public void GuardarRécordYVolver()
    {
        string nombreIngresado = inputNombre.text.Trim();

        // Si está vacío, mostramos el error y abortamos el guardado
        if (string.IsNullOrEmpty(nombreIngresado))
        {
            if (textoErrorNombre != null)
            {
                textoErrorNombre.gameObject.SetActive(true);
            }
            return; // El "return" hace que el código se corte aquí y NO cambie de escena
        }

        // Si hay nombre, guardamos normalmente
        jugadorDAO.insertar(nombreIngresado);
        int idJugador = jugadorDAO.obtenerIdPorNombre(nombreIngresado);

        if (idJugador != -1)
        {
            recordDAO.insertarPartida(idJugador, tiempoFinalFloat, DateTime.Now);
        }

        LimpiarGameManager();
        SceneManager.LoadScene(nombreEscenaMenuPrincipal);
    }

    // --- BOTÓN 2: JUGAR DE NUEVO SIN GUARDAR ---
    public void JugarDeNuevoSinGuardar()
    {
        LimpiarGameManager();
        SceneManager.LoadScene(nombreEscenaNivel1);
    }

    // --- BOTÓN 3: VOLVER AL MENÚ SIN GUARDAR ---
    public void VolverAlMenuSinGuardar()
    {
        LimpiarGameManager();
        SceneManager.LoadScene(nombreEscenaMenuPrincipal);
    }
}
