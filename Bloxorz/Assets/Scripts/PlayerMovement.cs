using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour{
    [Header("Logica de la rotacion")]
    private bool isParado=true;
    [SerializeField]private float duracionRotacionPlayer = 2f;      // Tiempo que tarda en rotar
    private static readonly float[] angleParado     = { -90f, 90f, 0f };
    private static readonly float[] angleVertical   = { -180f, 180f, 0f };
    private static readonly float[] angleHorizontal = { 0f, 90f, 0f };
    private bool isHorizontal=false; //true: esta acostado  pero en vertical
    //Si esta vertical cambia la X, si esta en horizontal la Z ---- EJ vertical: (0, 0.5, -1.5),,, EJ horizontal (0.5, 0.5,0)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        controlarRotacionPlayer();
    }

    private void controlarRotacionPlayer(){
        if(Input.GetKeyDown(KeyCode.W)){
            if(isParado){
                StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0,1f,-0.5f), Vector3.right, 90,transform.position + new Vector3(0,-0.5f,1.5f),new Vector3(angleVertical[0], angleVertical[1],angleVertical[2])));
                isParado=false;
                isHorizontal=false;
            }else{
                if(isHorizontal){
                    StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0,0.5f,-0.5f), Vector3.right, 90, transform.position + new Vector3(0,0,1f),new Vector3(angleHorizontal[0], angleHorizontal[1], angleHorizontal[2])));
                }else{
                    StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0,0.5f,-1f), Vector3.right, 90, transform.position + new Vector3(0,0.5f,1.5f) ,new Vector3(angleParado[0], angleParado[1], angleParado[2])));
                    isParado=true;
                }
            }
        }else if(Input.GetKeyDown(KeyCode.S)){
            if(isParado){
                StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0,1f,0.5f), -Vector3.right, 90,transform.position + new Vector3(0,-0.5f,-1.5f),new Vector3(angleVertical[0], angleVertical[1],angleVertical[2])));
                isParado=false;
                isHorizontal=false;
            }else{
                if(isHorizontal){
                    StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0,0.5f,0.5f), -Vector3.right, 90, transform.position + new Vector3(0,0,-1f),new Vector3(angleHorizontal[0], angleHorizontal[1], angleHorizontal[2])));
                }else{
                    StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0,0.5f,1f), -Vector3.right, 90, transform.position + new Vector3(0,0.5f,-1.5f) ,new Vector3(angleParado[0], angleParado[1], angleParado[2])));
                    isParado=true;
                }
            }
        }else if(Input.GetKeyDown(KeyCode.D)){
            if(isParado){
                StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(-0.5f,1f,0), -Vector3.forward, 90, transform.position + new Vector3(1.5f,-0.5f,0),new Vector3(angleHorizontal[0], angleHorizontal[1], angleHorizontal[2])));
                isParado=false;
                isHorizontal=true;
            }else{
                if(isHorizontal){
                    StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(-1f,0.5f,0), -Vector3.forward, 90, transform.position + new Vector3(1.5f,0.5f,0f) ,new Vector3(angleParado[0], angleParado[1], angleParado[2])));
                    isParado=true;
                }else{
                    StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(-0.5f,0.5f,0), -Vector3.forward, 90, transform.position + new Vector3(1f,0f,0f),new Vector3(angleVertical[0], angleVertical[1],angleVertical[2])));
                }
            }
        }else if(Input.GetKeyDown(KeyCode.A)){
            if(isParado){
                StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0.5f,1f,0), Vector3.forward, 90, transform.position + new Vector3(-1.5f,-0.5f,0),new Vector3(angleHorizontal[0], angleHorizontal[1], angleHorizontal[2])));
                isParado=false;
                isHorizontal=true;
            }else{
                if(isHorizontal){
                    StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(1f,0.5f,0), Vector3.forward, 90, transform.position + new Vector3(-1.5f,0.5f,0f) ,new Vector3(angleParado[0], angleParado[1], angleParado[2])));
                    isParado=true;
                }else{
                    StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0.5f,0.5f,0), Vector3.forward, 90, transform.position + new Vector3(-1f,0f,0f),new Vector3(angleVertical[0], angleVertical[1],angleVertical[2])));
                }
            }
        }
    }

    IEnumerator RotarEnUnTiempo(Vector3 punto, Vector3 eje, float anguloTotal, Vector3 posFinal, Vector3 rotFinal){
        float anguloRotado = 0f;
        while (anguloRotado < Mathf.Abs(anguloTotal)){
            float anguloPaso = (Time.deltaTime / duracionRotacionPlayer) * Mathf.Abs(anguloTotal);
            transform.RotateAround(punto, eje, anguloPaso * Mathf.Sign(anguloTotal));
            anguloRotado += anguloPaso;
            yield return null;
        }

        correccionDeErrorRotacion(posFinal,rotFinal);

    }

    private void correccionDeErrorRotacion(Vector3 posFinal, Vector3 rotFinal){
        transform.position = posFinal;
        transform.eulerAngles = rotFinal;
    }


}
