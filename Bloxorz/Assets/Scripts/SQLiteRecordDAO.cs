using System;
using System.Data;
using System.IO;
using Mono.Data.SqliteClient;
using UnityEngine;

public class SQLiteRecordDAO : IRecordDAO
{
    private string connectionString;

    public SQLiteRecordDAO()
    {
        // Definimos la ruta donde se guardará la base de datos en tu equipo
        string dbPath = Application.persistentDataPath + "/BloxorzDatabase.db";
        connectionString = "URI=file:" + dbPath;

        // Nos aseguramos de que las tablas existan al iniciar
        CrearTablasSiNoExisten();
    }

    private void CrearTablasSiNoExisten()
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                // Activamos las claves foráneas (Foreign Keys) que vienen desactivadas por defecto en SQLite
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();

                // Creamos las tablas respetando exactamente tu diagrama UML
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Jugador (
                        id_jugador INTEGER PRIMARY KEY AUTOINCREMENT,
                        nombre VARCHAR(20) UNIQUE
                    );
                    
                    CREATE TABLE IF NOT EXISTS Record (
                        id_record INTEGER PRIMARY KEY AUTOINCREMENT,
                        id_jugador INTEGER,
                        tiempo REAL,
                        fecha DATE,
                        FOREIGN KEY(id_jugador) REFERENCES Jugador(id_jugador)
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Implementación del método de la interfaz IRecordDAO
    public void insertarPartida(int id_jugador, float tiempo, DateTime fecha)
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                // Insertamos los datos en la tabla Record
                cmd.CommandText = "INSERT INTO Record (id_jugador, tiempo, fecha) VALUES (@id, @t, @f)";
                cmd.Parameters.Add(new SqliteParameter("@id", id_jugador));
                cmd.Parameters.Add(new SqliteParameter("@t", tiempo));

                // Formateamos la fecha para que SQLite la guarde de forma estándar (Año-Mes-Día)
                cmd.Parameters.Add(new SqliteParameter("@f", fecha.ToString("yyyy-MM-dd HH:mm:ss")));

                cmd.ExecuteNonQuery();
            }
        }
    }

    // Implementación del método de la interfaz IRecordDAO
    public IDataReader obtenerRanking()
    {
        SqliteConnection conn = new SqliteConnection(connectionString);
        conn.Open();
        SqliteCommand cmd = (SqliteCommand)conn.CreateCommand();

        // Realizamos el JOIN para traer el nombre del jugador junto con sus récords.
        // Agrupamos primero por nombre (alfabético) y luego ordenamos sus tiempos de menor a mayor.
        cmd.CommandText = @"
            SELECT j.nombre, r.tiempo, r.fecha 
            FROM Record r 
            JOIN Jugador j ON r.id_jugador = j.id_jugador
            ORDER BY j.nombre ASC, r.tiempo ASC";

        // Devolvemos el DataReader y le indicamos que cierre la conexión al terminar de leer
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }
}