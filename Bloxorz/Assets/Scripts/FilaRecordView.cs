using UnityEngine;
using TMPro; // Librería para los textos modernos
using System;

public class FilaRecordView : MonoBehaviour
{
    [Header("Arrastra aquí los textos del prefab")]
    public TMP_Text textoNombre;
    public TMP_Text textoTiempo;
    public TMP_Text textoFecha;

    public void ConfigurarFila(string nombre, float tiempo, DateTime fecha)
    {
        textoNombre.text = nombre;
        textoTiempo.text = tiempo.ToString("F2") + " s";
        textoFecha.text = fecha.ToString("dd/MM/yyyy");
    }
}