using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance=null;
    private Cronometro cronometro;
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
        
    }

    // Update is called once per frame
    void Update(){
        cronometro.correTimer();
    }

}
