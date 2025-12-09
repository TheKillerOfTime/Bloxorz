using UnityEngine;

public class LogoAnimator : MonoBehaviour
{
    [Header("Configuración de Flote")]
    [Tooltip("Qué tan rápido se mueve arriba y abajo")]
    public float velocidadFlote = 2f;
    [Tooltip("Qué tanta distancia se mueve hacia arriba y abajo (en píxeles)")]
    public float amplitudFlote = 15f;

    [Header("Configuración de Pulso (Escala)")]
    [Tooltip("Qué tan rápido se agranda y achica")]
    public float velocidadPulso = 1.5f;
    [Tooltip("Cuánto crece extra (ej. 0.05 es un 5% más grande)")]
    public float fuerzaPulso = 0.05f;

    private Vector3 posicionInicial;
    private Vector3 escalaInicial;

    void Start()
    {
        // Guardamos la posición y escala originales al iniciar la escena
        posicionInicial = transform.localPosition;
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        AnimarFlote();
        AnimarPulso();
    }

    void AnimarFlote()
    {
        // Usamos la función Seno (Sin) para crear un movimiento de ola suave
        float nuevoY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlote) * amplitudFlote;
        // Aplicamos la nueva posición Y, manteniendo X y Z iguales
        transform.localPosition = new Vector3(posicionInicial.x, nuevoY, posicionInicial.z);
    }

    void AnimarPulso()
    {
        // Usamos Seno de nuevo, pero quizás con diferente velocidad
        float factorEscala = 1f + Mathf.Sin(Time.time * velocidadPulso) * fuerzaPulso;
        // Multiplicamos la escala inicial por este factor
        transform.localScale = escalaInicial * factorEscala;
    }
}