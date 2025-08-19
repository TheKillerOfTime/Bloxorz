using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour{
    [Header("Logica de la rotacion")]
    private bool isParado=true;
    private float duracionRotacionPlayer = 0.5f;      // Tiempo que tarda en rotar
    private static readonly float[] angleParado     = { -90f, 90f, 0f };
    private static readonly float[] angleVertical   = { -180f, 180f, 0f };
    private static readonly float[] angleHorizontal = { 0f, 90f, 0f };
    private bool isHorizontal=false; //true: esta acostado  pero en vertical
    //Si esta vertical cambia la X, si esta en horizontal la Z ---- EJ vertical: (0, 0.5, -1.5),,, EJ horizontal (0.5, 0.5,0)
    [Header("Logica de la secuencia de movimientos")]
    public static List<string> colaMovimientos = new List<string>();
    private bool netxMove=true;
    private string move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        detectorDeMovimientos();
        if(colaMovimientos.Count>0){
            if(netxMove){
                controlarRotacionPlayer();
            }
        }
    }

    private void detectorDeMovimientos(){
        if(Input.GetKeyDown(KeyCode.W)){
            colaMovimientos.Add("W");
        }else if(Input.GetKeyDown(KeyCode.S)){
            colaMovimientos.Add("S");
        }else if(Input.GetKeyDown(KeyCode.D)){
            colaMovimientos.Add("D");
        }else if(Input.GetKeyDown(KeyCode.A)){
            colaMovimientos.Add("A");
        }else if(Input.GetKeyDown(KeyCode.Space)){
            if(duracionRotacionPlayer==0.5f){
                duracionRotacionPlayer=0.25f;
            }else{
                duracionRotacionPlayer=0.5f;
                colaMovimientos.Clear();
            }
        }
    }

    private void controlarRotacionPlayer(){
        netxMove=false;
        move = colaMovimientos[0];
        colaMovimientos.RemoveAt(0);
        if(move=="W"){
            if(isParado){
                StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(0,1f,-0.5f), Vector3.right, 90,transform.position + new Vector3(0,-0.5f,1.5f),new Vector3(angleVertical[0], angleVertical[1],angleVertical[2])));
                isParado=false;
                isHorizontal=false;
            }else{
                if(isHorizontal){
                    StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(0,0.5f,-0.5f), Vector3.right, 90, transform.position + new Vector3(0,0,1f),new Vector3(angleHorizontal[0], angleHorizontal[1], angleHorizontal[2])));
                }else{
                    StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(0,0.5f,-1f), Vector3.right, 90, transform.position + new Vector3(0,0.5f,1.5f) ,new Vector3(angleParado[0], angleParado[1], angleParado[2])));
                    isParado=true;
                }
            }
        }else if(move=="S"){
            if(isParado){
                StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(0,1f,0.5f), -Vector3.right, 90,transform.position + new Vector3(0,-0.5f,-1.5f),new Vector3(angleVertical[0], angleVertical[1],angleVertical[2])));
                isParado=false;
                isHorizontal=false;
            }else{
                if(isHorizontal){
                    StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(0,0.5f,0.5f), -Vector3.right, 90, transform.position + new Vector3(0,0,-1f),new Vector3(angleHorizontal[0], angleHorizontal[1], angleHorizontal[2])));
                }else{
                    StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(0,0.5f,1f), -Vector3.right, 90, transform.position + new Vector3(0,0.5f,-1.5f) ,new Vector3(angleParado[0], angleParado[1], angleParado[2])));
                    isParado=true;
                }
            }
        }else if(move=="D"){
            if(isParado){
                StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(-0.5f,1f,0), -Vector3.forward, 90, transform.position + new Vector3(1.5f,-0.5f,0),new Vector3(angleHorizontal[0], angleHorizontal[1], angleHorizontal[2])));
                isParado=false;
                isHorizontal=true;
            }else{
                if(isHorizontal){
                    StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(-1f,0.5f,0), -Vector3.forward, 90, transform.position + new Vector3(1.5f,0.5f,0f) ,new Vector3(angleParado[0], angleParado[1], angleParado[2])));
                    isParado=true;
                }else{
                    StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(-0.5f,0.5f,0), -Vector3.forward, 90, transform.position + new Vector3(1f,0f,0f),new Vector3(angleVertical[0], angleVertical[1],angleVertical[2])));
                }
            }
        }else if(move=="A"){
            if(isParado){
                StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(0.5f,1f,0), Vector3.forward, 90, transform.position + new Vector3(-1.5f,-0.5f,0),new Vector3(angleHorizontal[0], angleHorizontal[1], angleHorizontal[2])));
                isParado=false;
                isHorizontal=true;
            }else{
                if(isHorizontal){
                    StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(1f,0.5f,0), Vector3.forward, 90, transform.position + new Vector3(-1.5f,0.5f,0f) ,new Vector3(angleParado[0], angleParado[1], angleParado[2])));
                    isParado=true;
                }else{
                    StartCoroutine(rotarEnUnTiempo(transform.position - new Vector3(0.5f,0.5f,0), Vector3.forward, 90, transform.position + new Vector3(-1f,0f,0f),new Vector3(angleVertical[0], angleVertical[1],angleVertical[2])));
                }
            }
        }
    }

    IEnumerator rotarEnUnTiempo(Vector3 punto, Vector3 eje, float anguloTotal, Vector3 posFinal, Vector3 rotFinal){
        float anguloRotado = 0f;
        while (anguloRotado < Mathf.Abs(anguloTotal)){
            float anguloPaso = (Time.deltaTime / duracionRotacionPlayer) * Mathf.Abs(anguloTotal);
            transform.RotateAround(punto, eje, anguloPaso * Mathf.Sign(anguloTotal));
            anguloRotado += anguloPaso;
            yield return null;
        }

        correccionDeErrorRotacion(posFinal,rotFinal);
        netxMove = true;
        if(colaMovimientos.Count==0){
            duracionRotacionPlayer=0.5f;
        }
    }

    private void correccionDeErrorRotacion(Vector3 posFinal, Vector3 rotFinal){
        transform.position = posFinal;
        transform.eulerAngles = rotFinal;
    }


}
