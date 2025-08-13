using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour{

    //private float duracionRotacion = 0.5f;     // Tiempo en segundos que durará la rotación
    //private bool horizontal=false, vertical=false;
    //private Vector3 eje;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
/*
    public void IniciarRotacion(){
        StartCoroutine(RotarRelativa(duracionRotacion));
    }

    IEnumerator RotarRelativa(float tiempo){
        float gradosRotados = 0f;
            float velocidadRotacion = grados / tiempo;  // grados por segundo
            while (gradosRotados < grados){
                float deltaGrados = velocidadRotacion * Time.deltaTime;
                if (gradosRotados + deltaGrados > grados){
                    deltaGrados = grados - gradosRotados;
                    transform.RotateAround(centro, eje, deltaGrados);
                    gradosRotados += deltaGrados;
                    yield return null;
                }
            }
    }

    private void movimientos(){

        if(Input.GetKeyDown(KeyCode.W)){

        }

        if(Input.GetKeyDown(KeyCode.S)){

        }

        if(Input.GetKeyDown(KeyCode.A)){

        }

        if(Input.GetKeyDown(KeyCode.D)){

        }

    }*/

}
