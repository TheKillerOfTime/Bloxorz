using UnityEngine;

public class CambiarColor : MonoBehaviour
{
    [SerializeField] private GameObject playerVisualMenu;
    [SerializeField] private Color[] colores;

    private static int indiceColorSeleccionado = 0;

    // NUEVA VARIABLE ESTÁTICA: Guarda el color real para pasarlo al juego
    public static Color ColorSeleccionado = Color.white;

    public static int IndiceColorSeleccionado
    {
        get { return indiceColorSeleccionado; }
    }

    private Renderer rendMenu;

    void Start()
    {
        if (playerVisualMenu != null)
        {
            rendMenu = playerVisualMenu.GetComponentInChildren<Renderer>();

            if (rendMenu != null)
            {
                actualizarColorMenu();
            }
            else
            {
                Debug.LogError("No se encontró Renderer en: " + playerVisualMenu.name);
            }
        }
    }

    public void cambiar()
    {
        if (rendMenu == null || colores.Length == 0) return;

        indiceColorSeleccionado = (indiceColorSeleccionado + 1) % colores.Length;
        actualizarColorMenu();
    }

    private void actualizarColorMenu()
    {
        if (rendMenu != null && colores.Length > 0)
        {
            Color c = colores[indiceColorSeleccionado];

            // Guardamos el color en la variable estática para usarlo en otros niveles
            ColorSeleccionado = c;

            Material[] materiales = rendMenu.materials;
            for (int i = 0; i < materiales.Length; i++)
            {
                if (materiales[i].HasProperty("_BaseColor"))
                    materiales[i].SetColor("_BaseColor", c);
                else if (materiales[i].HasProperty("_Color"))
                    materiales[i].SetColor("_Color", c);
                else
                    materiales[i].color = c;
            }
            rendMenu.materials = materiales;
        }
    }
}