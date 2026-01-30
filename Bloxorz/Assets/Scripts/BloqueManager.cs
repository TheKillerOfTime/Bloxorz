using UnityEngine;

public class BloqueManager : MonoBehaviour{

    private Vector3 origin;
    private Vector3 direction;
    private bool bloquePerdedor=true;
    public GameManager mainMenuManager;
    private bool colisiono=false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        origin = transform.position;
        direction = transform.up;
        if(this.gameObject.CompareTag("Fin")){
            bloquePerdedor=false;
        }else{
            mainMenuManager.setInicioGameManager();
        }
    }

    // Update is called once per frame
    void Update(){
        rayoDetectorDeJugador();
        if(colisiono && GameManager.pasoDeNivel){
            colisiono=false;
            GameManager.pasoDeNivel=false;
            mainMenuManager.getColisionSiPierdeOGana(bloquePerdedor); 
        }
    }

    private void rayoDetectorDeJugador(){

        //Debug.DrawRay(origin, direction * 10, Color.red);

        // Lanza el rayo
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 10f)){
            if(hit.collider.gameObject.CompareTag("Players")){
                colisiono=true;
            }
        }
    }

}
