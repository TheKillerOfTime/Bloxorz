using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour{
    [Header("Logica de la rotacion")]
    private bool isParado=true;
    [SerializeField]private float duracionRotacionPlayer = 2f;      // Tiempo que tarda en rotar
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
                StartCoroutine(RotarEnUnTiempo(transform.position - new Vector3(0,1f,-0.5f), Vector3.right, 90));
            }
        }else if(Input.GetKeyDown(KeyCode.S)){

        }else if(Input.GetKeyDown(KeyCode.D)){

        }else if(Input.GetKeyDown(KeyCode.A)){

        }
    }

    IEnumerator RotarEnUnTiempo(Vector3 punto, Vector3 eje, float anguloTotal){
        float anguloRotado = 0f;
        while (anguloRotado < Mathf.Abs(anguloTotal))
        {
            float anguloPaso = (Time.deltaTime / duracionRotacionPlayer) * Mathf.Abs(anguloTotal);
            transform.RotateAround(punto, eje, anguloPaso * Mathf.Sign(anguloTotal));
            anguloRotado += anguloPaso;
            yield return null;
        }
    }



}
