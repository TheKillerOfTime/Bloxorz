using UnityEngine;

public class BotonesUI : MonoBehaviour
{
    // Variable para guardar tu archivo de sonido
    public AudioSource parlanteSFX;
    public AudioClip sonidoClick;

    // Función para reproducir el sonido de forma segura
    public void reproducirSonido()
    {
        if (parlanteSFX != null && sonidoClick != null && Camera.main != null)
        {
            // Reproduce el sonido justo donde está la cámara para que se escuche al 100% de volumen
            parlanteSFX.PlayOneShot(sonidoClick);
        }
    }

    public void clickRestart()
    {
        if (GameManager.instance != null) GameManager.instance.restartGame();
    }

    public void clickMenu()
    {
        if (GameManager.instance != null) GameManager.instance.goBackToMenu();
    }

    public void clickReanudar()
    {
        if (GameManager.instance != null) GameManager.instance.ReanudarJuego();
    }
}
