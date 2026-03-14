using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class FilaRecordView : MonoBehaviour
{
    public TMP_Text textoNombre;
    public TMP_Text textoTiempo;
    public TMP_Text textoFecha;

    public Button botonEditar;
    public Button botonEliminar;

    private int idRecordActual;

    // Estos "Actions" son como cables que conectan con el menú principal
    private Action<int> accionEliminar;
    private Action<int> accionEditar;

    // 🔹 Se agregaron dos parámetros al final para recibir las órdenes
    public void ConfigurarFila(int idRecord, string nombre, float tiempo, DateTime fecha, Action<int> alEliminar, Action<int> alEditar)
    {
        idRecordActual = idRecord;
        textoNombre.text = nombre;
        textoTiempo.text = tiempo.ToString("F2") + " s";
        textoFecha.text = fecha.ToString("dd/MM/yyyy");

        accionEliminar = alEliminar;
        accionEditar = alEditar;

        if (botonEliminar != null) botonEliminar.onClick.RemoveAllListeners();
        if (botonEditar != null) botonEditar.onClick.RemoveAllListeners();

        if (botonEliminar != null) botonEliminar.onClick.AddListener(BotonEliminarPresionado);
        if (botonEditar != null) botonEditar.onClick.AddListener(BotonEditarPresionado);
    }

    private void BotonEliminarPresionado()
    {
        accionEliminar?.Invoke(idRecordActual); // Le avisa al menú que borre este ID
    }

    private void BotonEditarPresionado()
    {
        accionEditar?.Invoke(idRecordActual); // Le avisa al menú que edite este ID
    }
}