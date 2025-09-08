using UnityEngine;

public class CambiarColor : MonoBehaviour
{
    public GameObject playerBase;   // referencia al bloque
    private Renderer rend;

    // Colores para alternar
    public Color[] colores;
    private int indiceActual = 0;

    void Start()
    {
        if (playerBase != null)
        {
            rend = playerBase.GetComponent<Renderer>();
            rend.material.color = colores[indiceActual];
        }
    }

    public void Cambiar()
    {
        if (rend == null) return;

        indiceActual = (indiceActual + 1) % colores.Length;
        rend.material.color = colores[indiceActual];
    }
}