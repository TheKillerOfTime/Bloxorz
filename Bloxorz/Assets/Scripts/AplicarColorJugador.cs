using UnityEngine;

public class AplicarColorJugador : MonoBehaviour
{
    void Start()
    {
        // Buscamos el Renderer del jugador (el componente que lo dibuja)
        Renderer rendJugador = GetComponentInChildren<Renderer>();

        if (rendJugador != null)
        {
            // Traemos el color guardado desde el menú
            Color c = CambiarColor.ColorSeleccionado;

            // Se lo aplicamos a todos sus materiales de la misma forma que en el menú
            Material[] materiales = rendJugador.materials;
            for (int i = 0; i < materiales.Length; i++)
            {
                if (materiales[i].HasProperty("_BaseColor"))
                    materiales[i].SetColor("_BaseColor", c);
                else if (materiales[i].HasProperty("_Color"))
                    materiales[i].SetColor("_Color", c);
                else
                    materiales[i].color = c;
            }
            rendJugador.materials = materiales;
        }
    }
}