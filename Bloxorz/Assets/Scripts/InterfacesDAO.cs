using System;
using System.Data; // Necesario para IDataReader

// Interfaz para el Jugador (Según tu diagrama UML)
public interface IJugadorDAO
{
    void insertar(string nombre);
    int obtenerIdPorNombre(string nombre);
}

// Interfaz para los Records (Según tu diagrama UML)
public interface IRecordDAO
{
    // Usamos DateTime para la fecha como me pediste corregir antes
     void insertarPartida(int id_jugador, float tiempo, DateTime fecha);

    // Devuelve un IDataReader para leer los datos crudos de SQLite
    IDataReader obtenerRanking();
    
}