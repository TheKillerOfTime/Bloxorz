using UnityEngine;

public class CambiarColor : MonoBehaviour
{
    public GameObject playerVisualMenu;
    public Color[] colores;
    public static int indiceColorSeleccionado = 0;
    private Renderer rendMenu;

    void Start()
    {
        if (playerVisualMenu != null)
        {
            rendMenu = playerVisualMenu.GetComponentInChildren<Renderer>();

            if (rendMenu != null)
            {
                // ESTO APARECERÁ EN LA CONSOLA: MIRA EL NOMBRE
                Debug.Log("Renderer encontrado en el objeto: " + rendMenu.gameObject.name);
                ActualizarColorMenu();
            }
            else
            {
                Debug.LogError("Sigue sin encontrarse Renderer en los hijos de " + playerVisualMenu.name);
            }
        }
    }

    public void Cambiar()
    {
        if (rendMenu == null) return;

        indiceColorSeleccionado = (indiceColorSeleccionado + 1) % colores.Length;
        ActualizarColorMenu();
    }

    void ActualizarColorMenu()
    {
        if (rendMenu != null && colores.Length > 0)
        {
            Color c = colores[indiceColorSeleccionado];

            // 1. Obtenemos una copia de TODOS los materiales del objeto
            Material[] materiales = rendMenu.materials;

            // 2. Recorremos cada material para cambiar su color
            for (int i = 0; i < materiales.Length; i++)
            {
                // Opción A: Shader URP Lit (Universal Render Pipeline)
                if (materiales[i].HasProperty("_BaseColor"))
                {
                    materiales[i].SetColor("_BaseColor", c);
                }
                // Opción B: Shader Standard o Legacy
                else if (materiales[i].HasProperty("_Color"))
                {
                    materiales[i].SetColor("_Color", c);
                }
                // Opción C: Forzar el color principal (funciona en la mayoría)
                else
                {
                    materiales[i].color = c;
                }
            }

            // IMPORTANTE: En Unity, al modificar el array, hay que reasignarlo
            // para que los cambios surtan efecto visualmente en algunos casos.
            rendMenu.materials = materiales;

            Debug.Log("Color cambiado a: " + c.ToString());
        }
    }
}