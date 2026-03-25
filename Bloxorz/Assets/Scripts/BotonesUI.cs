using UnityEngine;

public class BotonesUI : MonoBehaviour
{
    public void clickRestart()
    {
        // Le avisamos directamente a la instancia inmortal
        if (GameManager.instance != null)
        {
            GameManager.instance.restartGame();
        }
    }

    public void clickMenu()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.goBackToMenu();
        }
    }
    public void clickReanudar()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ReanudarJuego();
        }
    }
}

