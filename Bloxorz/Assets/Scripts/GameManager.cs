using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance=null;
    private Cronometro cronometro;
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
