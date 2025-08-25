using UnityEngine;
using TMPro;

public class Cronometro{ //00:00:00:000

    private float tiempoTranscurrido=0f;
    private bool corriendo;
    private TextMeshProUGUI timeText;
    private int milisegundos=0,segundos=0,minutos=0,horas=0;

    public Cronometro(TextMeshProUGUI t, bool running){
        timeText=t;
        corriendo=running;
    }

    public void correTimer(){
        if(corriendo){
            // Incrementa el tiempo por el tiempo transcurrido desde el último frame
            tiempoTranscurrido += Time.deltaTime;
            horas = Mathf.FloorToInt(tiempoTranscurrido / 3600);
            minutos = Mathf.FloorToInt((tiempoTranscurrido % 3600) / 60);
            segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);
            milisegundos = Mathf.FloorToInt((tiempoTranscurrido * 1000) % 1000);
            if(horas<60){
                timeText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", horas, minutos, segundos, milisegundos);
            }else{
                corriendo=false;
                timeText.text = "Tiempo excedido";
            }

        }
    }



    public void cambiarText(TextMeshProUGUI t){
        timeText = t;
    }

    public void reanudo(){
        if(!corriendo){
            corriendo=true;
        }
    }

    public void pauso(){
        if(corriendo){
            corriendo=false;
        }
    }

}
