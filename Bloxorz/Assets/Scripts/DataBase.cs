using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DataBase : MonoBehaviour{

    public TextMeshProUGUI recordText;
    public TMP_InputField nombre;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        recordText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", Mathf.FloorToInt(GameManager.instance.cronometro.getTiempoTranscurrido() / 3600), Mathf.FloorToInt((GameManager.instance.cronometro.getTiempoTranscurrido() % 3600) / 60), Mathf.FloorToInt(GameManager.instance.cronometro.getTiempoTranscurrido() % 60), Mathf.FloorToInt((GameManager.instance.cronometro.getTiempoTranscurrido() * 1000) % 1000));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void guardarYsalir(){
        DatabaseManager.Instance.InsertarJugador(nombre.text);
        DatabaseManager.Instance.InsertarRecord(GameManager.instance.cronometro.getTiempoTranscurrido(), System.DateTime.Now.ToString("yyyy-MM-dd") , DatabaseManager.ultimoID);
        SceneManager.LoadScene("Main Menu");
    }
}
