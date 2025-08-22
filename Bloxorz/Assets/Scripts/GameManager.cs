using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance=null;
    private Cronometro cronometro;
    public GameObject [] flechasPosiciones = new GameObject[5]; 
    public Sprite[] flechas = new Sprite[5]; //ARRIBA,ABAJO,DERECHA,IZQUIERDA,VACIO
    private static bool aumentoNivel=false;
    private static int nivel = 1;
    [SerializeField] private TextMeshProUGUI nivelText;
    public TextMeshProUGUI timeText;

    void Start(){
        if(instance==null){
            instance=this;
            DontDestroyOnLoad(this);
        }else{
            Destroy(this);
        }
        cronometro = new Cronometro(timeText, false);
        cronometro.activoDesactivo();
        nivelText.text = "Nivel: " + nivel;
        
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

    public static void incrementoNivel(){
        nivel++;
    }

    private void repintoNivelText(){
        if(aumentoNivel){
            nivelText.text = "Nivel: " + nivel;
            aumentoNivel=false;
        }
    }

}
