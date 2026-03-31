using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance=null;
    public Cronometro cronometro;
    public GameObject [] flechasPosiciones = new GameObject[5]; 
    public Sprite[] flechas = new Sprite[5]; //ARRIBA,ABAJO,DERECHA,IZQUIERDA,VACIO
    public static int nivel = 1;
    public bool levelUp=false;
    public TextMeshProUGUI nivelText;
    public TextMeshProUGUI timeText;
    public GameObject panelPause;
    public GameObject panelJuego;
    public GameObject mapa;
    public GameObject player;
    public static bool gameOver=false;
    public static bool pasoDeNivel=true;
    public Camera camara;
    public Transform origenCamara;
    public Transform origenFinalCamara;
    public Transform finalCamara;
    private float duracion = 1f;
    private const int nivelMax=4;
    [Header("Menú de Pausa")]
    public GameObject panelMenuPausa; // El nuevo cartel visual de Pausa
    public bool isPaused = false;
    [Header("Sonidos")]
    public AudioClip sonidoVictoria; // 🔹 Aquí guardaremos el sonido de ganar
    [Header("Música de Fondo")]
    public AudioSource musicaFondo;


    void Start(){
        inicializoNivel();
    }

    void Update(){
        // Solo corremos el timer si NO estamos pausados y NO perdimos
        if (!isPaused && !gameOver) {
            instance.cronometro.correTimer();
        }

        repintoNivelText();
        setNextMoves();

        // 🔹 DETECTAR LA TECLA ESCAPE
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (isPaused) {
                ReanudarJuego();
            } else {
                PausarJuego();
            }
        }
    }
    public void PausarJuego()
    {
        isPaused = true;
        Time.timeScale = 0f; // 🔹 Congela el universo de Unity
        instance.cronometro.pauso(); // Pausamos tu cronómetro manual
        panelMenuPausa.SetActive(true); // Mostramos el panel
        PlayerMovement.colaMovimientos.Clear(); // Limpiamos las teclas para que no se acumulen
        if (instance.musicaFondo != null) instance.musicaFondo.Pause();
    }

    public void ReanudarJuego()
    {
        isPaused = false;
        Time.timeScale = 1f; // 🔹 Descongela el universo
        instance.cronometro.reanudo();
        panelMenuPausa.SetActive(false); // Ocultamos el panel
        if (instance.musicaFondo != null) instance.musicaFondo.UnPause();
    }


    private void inicializoNivel()
    {
        camara = Camera.main;
        cronometro = new Cronometro(timeText, false);
        repintoNivelText();

        if (instance == null)
        {
            instance = this;
            instance.cronometro = cronometro;
            instance.camara = this.camara;
            DontDestroyOnLoad(this); // El original se vuelve inmortal
            if (instance.musicaFondo != null)
            {
                instance.musicaFondo.Play();
            }
        }
        else
        {
            pasarAtributos(this); // Le pasamos los datos frescos al inmortal
            Destroy(gameObject);  // 🔹 ¡LA CLAVE! Destruimos este clon para que no estorbe
        }

        instance.StartCoroutine(moverCamaraInicio());
    }

    private IEnumerator moverCamaraInicio(){
        float tiempo = 0f;
        pasoDeNivel=false;
        while (tiempo < duracion)
        {
            // Interpola entre A y B en base al tiempo
            instance.camara.transform.position = Vector3.Lerp(origenCamara.position, origenFinalCamara.position, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null; // esperar al siguiente frame
        }

        // Asegurar que quede exactamente en el punto B
        instance.camara.transform.position = origenFinalCamara.position;

        // 🔹 Aquí van las cosas que quieres que pasen DESPUÉS
        Debug.Log("Movimiento terminado. Ahora pasan otras cosas.");
        instance.cronometro.reanudo();
        pasoDeNivel=true;
    }

    private IEnumerator moverCamaraFinal(){
        float tiempo = 0f;
        instance.cronometro.pauso();
        while (tiempo < duracion)
        {
            // Interpola entre A y B en base al tiempo
            instance.camara.transform.position = Vector3.Lerp(origenFinalCamara.position, finalCamara.position, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null; // esperar al siguiente frame
        }

        // Asegurar que quede exactamente en el punto B
        instance.camara.transform.position = finalCamara.position;

        // 🔹 Aquí van las cosas que quieres que pasen DESPUÉS
        Debug.Log("Movimiento terminado. Ahora pasan otras cosas.");
        nivel++;
        levelUp=true;
        if(nivel <= nivelMax){
            Debug.Log(nivel);
            SceneManager.LoadScene("Level "+nivel.ToString());
        }else{
            gameOver=true;
            if (instance != null && instance.cronometro != null) {
                float tiempoFinal = instance.cronometro.getTiempoTranscurrido();
                PlayerPrefs.SetFloat("TiempoFinalJuego", tiempoFinal);
                PlayerPrefs.Save(); // Confirmamos el guardado
            }
            if (instance != null)
            {
                if (instance.musicaFondo != null) instance.musicaFondo.Stop();
                Destroy(instance.gameObject);
                instance = null;
            }
            SceneManager.LoadScene("FinDelJuego");
        }
    }

    private void pasarAtributos(GameManager gm){
        instance.flechasPosiciones = gm.flechasPosiciones;
        instance.nivelText = gm.nivelText;
        instance.timeText = gm.timeText;
        gm.timeText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", Mathf.FloorToInt(instance.cronometro.getTiempoTranscurrido() / 3600), Mathf.FloorToInt((instance.cronometro.getTiempoTranscurrido() % 3600) / 60), Mathf.FloorToInt(instance.cronometro.getTiempoTranscurrido() % 60), Mathf.FloorToInt((instance.cronometro.getTiempoTranscurrido() * 1000) % 1000));
        instance.cronometro.cambiarText(gm.timeText);
        instance.panelPause = gm.panelPause;
        instance.panelJuego = gm.panelJuego;
        instance.mapa = gm.mapa;
        instance.player = gm.player;
        instance.camara = gm.camara;
        instance.panelMenuPausa = gm.panelMenuPausa;
    }


    private void setNextMoves()
    {
        if (!gameOver && flechasPosiciones != null)
        {
            int j = PlayerMovement.colaMovimientos.Count;
            for (int i = 0; i < j && i < flechasPosiciones.Length; i++)
            {
                // 🔹 Solo pintamos si la flecha no ha sido destruida
                if (flechasPosiciones[i] != null)
                {
                    switch (PlayerMovement.colaMovimientos[i])
                    {
                        case "W": flechasPosiciones[i].GetComponent<Image>().sprite = flechas[0]; break;
                        case "S": flechasPosiciones[i].GetComponent<Image>().sprite = flechas[1]; break;
                        case "D": flechasPosiciones[i].GetComponent<Image>().sprite = flechas[2]; break;
                        case "A": flechasPosiciones[i].GetComponent<Image>().sprite = flechas[3]; break;
                    }
                }
            }
            for (int i = 4; i >= j; i--)
            {
                // 🔹 Verificamos aquí también
                if (flechasPosiciones[i] != null)
                {
                    flechasPosiciones[i].GetComponent<Image>().sprite = flechas[4];
                }
            }
        }
    }

    private void repintoNivelText()
    {
        // Solo intentamos cambiar el texto si el objeto no ha sido destruido
        if (nivelText != null)
        {
            nivelText.text = "Nivel: " + nivel.ToString();
        }
    }

    public void restartGame()
    {
        // 🔹 Restablecemos la variable global al primer nivel
        nivel = 1;

        // Forzamos a que el juego sepa que empezamos de cero
        gameOver = false;
        pasoDeNivel = true;

        // Como nivel ahora es 1, esto siempre cargará "Level 1"
        SceneManager.LoadScene("Level " + nivel.ToString());
    }

    public void goBackToMenu()
    {
        Time.timeScale = 1f; // 🔹 MUY IMPORTANTE: Descongelar el tiempo antes de salir
        isPaused = false;

        if (instance != null)
        {
            if (instance.musicaFondo != null) instance.musicaFondo.Stop();
            Destroy(instance.gameObject);
            instance = null;
        }
        SceneManager.LoadScene("Main Menu");
    }

    public void setInicioGameManager(){
        gameOver = false;
    }

    public void getColisionSiPierdeOGana(bool bloquePerdedor)
    {
        if (!gameOver)
        {
            if (!bloquePerdedor)
            {
                panelPause.SetActive(true);
                panelJuego.SetActive(false);
                gameOver = true;
                instance.cronometro.resetTimer();
            }
            else
            {
                if (player.GetComponent<PlayerMovement>().getIsParado() && player.GetComponent<PlayerMovement>().getEstaQuieto())
                {
                    Debug.Log("GANASTE");
                    if (sonidoVictoria != null && player != null)
                    {
                        AudioSource parlanteJugador = player.GetComponent<AudioSource>();
                        if (parlanteJugador != null)
                        {
                            parlanteJugador.PlayOneShot(sonidoVictoria);
                        }
                    }
                    instance.StartCoroutine(moverCamaraFinal());
                }
                else
                {
                    pasoDeNivel = true;
                }
            }
        }
    }

}
