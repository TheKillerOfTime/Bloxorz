using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance=null;
    public Cronometro cronometro;
    public GameObject [] flechasPosiciones = new GameObject[5]; 
    public Sprite[] flechas = new Sprite[5]; //ARRIBA,ABAJO,DERECHA,IZQUIERDA,VACIO
    public int nivel = 1;
    public TextMeshProUGUI nivelText;
    public TextMeshProUGUI timeText;
    public GameObject panelPause;
    public GameObject panelJuego;
    public GameObject mapa;
    public GameObject player;
    public bool gameOver=false;

    void Start(){
        cronometro = new Cronometro(timeText, false);
        cronometro.activoDesactivo();
        nivelText.text = "Nivel: " + nivel;
        if(instance==null){
            instance=this;
            DontDestroyOnLoad(this);
        }else{
            pasarAtributos(this);
            Destroy(this);
        }
        repintoNivelText();
        
    }

    private void pasarAtributos(GameManager gm){
        instance.flechasPosiciones = gm.flechasPosiciones;
        instance.nivelText = gm.nivelText;
        instance.timeText = gm.timeText;
        instance.panelPause = gm.panelPause;
        instance.panelJuego = gm.panelJuego;
        instance.mapa = gm.mapa;
        instance.player = gm.player;
        instance.gameOver=false;

    }

    // Update is called once per frame
    void Update(){
        cronometro.correTimer();
        repintoNivelText();
        setNextMoves();
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
                    nivel++;
                    Debug.Log(nivel);
                    SceneManager.LoadScene("Level "+nivel.ToString());
                }
            }
        }
        gameOver = true;
    }

}
