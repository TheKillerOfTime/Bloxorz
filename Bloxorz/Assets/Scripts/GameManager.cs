using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public bool gameOver=false;
    public Camera camara;
    public Transform origenCamara;
    public Transform origenFinalCamara;
    public Transform finalCamara;
    private float duracion = 1f;

    void Start(){
        camara = Camera.main;
        cronometro = new Cronometro(timeText, false);
        repintoNivelText();
        if(instance==null){
            instance=this;
            instance.cronometro = cronometro;
            instance.camara = this.camara;
            DontDestroyOnLoad(this);
        }else{
            pasarAtributos(this);
            Destroy(this);
        }
        instance.StartCoroutine(moverCamaraInicio());
    }

    // Update is called once per frame
    void Update(){
        instance.cronometro.correTimer();
        repintoNivelText();
        setNextMoves();
    }

    private IEnumerator moverCamaraInicio(){
        float tiempo = 0f;

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
        Debug.Log(nivel);
        SceneManager.LoadScene("Level "+nivel.ToString());
    }

    private void pasarAtributos(GameManager gm){
        instance.flechasPosiciones = gm.flechasPosiciones;
        instance.nivelText = gm.nivelText;
        instance.timeText = gm.timeText;
        gm.timeText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", Mathf.FloorToInt(instance.cronometro.getTiempoTranscurrido() / 3600), Mathf.FloorToInt((instance.cronometro.getTiempoTranscurrido() % 3600) / 60), 
        Mathf.FloorToInt(instance.cronometro.getTiempoTranscurrido() % 60), Mathf.FloorToInt((instance.cronometro.getTiempoTranscurrido() * 1000) % 1000));
        instance.cronometro.cambiarText(gm.timeText);
        instance.panelPause = gm.panelPause;
        instance.panelJuego = gm.panelJuego;
        instance.mapa = gm.mapa;
        instance.player = gm.player;
        instance.gameOver = false;
        instance.camara = gm.camara;
    }


    private void setNextMoves(){
        int j = PlayerMovement.colaMovimientos.Count;
        for (int i = 0; i < j && i < flechasPosiciones.Length; i++){
            switch(PlayerMovement.colaMovimientos[i]){
                case "W": flechasPosiciones[i].GetComponent<Image>().sprite = flechas[0]; break;
                case "S": flechasPosiciones[i].GetComponent<Image>().sprite = flechas[1]; break;
                case "D": flechasPosiciones[i].GetComponent<Image>().sprite = flechas[2]; break;
                case "A": flechasPosiciones[i].GetComponent<Image>().sprite = flechas[3]; break;
            }
        }
        for(int i = 4; i>=j; i--){
            flechasPosiciones[i].GetComponent<Image>().sprite = flechas[4];
        }

        
    }

    private void repintoNivelText(){
        nivelText.text = "Nivel: " + nivel.ToString();
    }

    public void restartGame(){
        nivel = 1;
        SceneManager.LoadScene("Nivel "+nivel.ToString());
    }

    public void setInicioGameManager(){
        gameOver = false;
    }

    public void getColisionSiPierdeOGana(bool bloquePerdedor){
        if(!gameOver){
            if(!bloquePerdedor){
                panelPause.SetActive(true);
                panelJuego.SetActive(false);
                restartGame();
            }else{
                if(player.GetComponent<PlayerMovement>().getIsParado()){
                    Debug.Log("GANASTE");
                    instance.StartCoroutine(moverCamaraFinal());
                }
            }
        }
        gameOver = true;
    }

}
