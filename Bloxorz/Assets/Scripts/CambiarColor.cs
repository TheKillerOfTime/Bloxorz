using UnityEngine;

public class CambiarColor : MonoBehaviour
{
    // [SerializeField] permite ver variables privadas en el Inspector
    [SerializeField] private GameObject playerVisualMenu;
    [SerializeField] private Color[] colores;

    // Estado interno estático (privado)
    private static int indiceColorSeleccionado = 0;

    // Propiedad pública para que otros scripts puedan LEER qué color se eligió
    // Se usa así desde otro script: CambiarColor.IndiceColorSeleccionado
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