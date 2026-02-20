using System;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine;

public class SQLiteJugadorDAO : IJugadorDAO
{
    private string connectionString;

    public SQLiteJugadorDAO()
    {
        // IMPORTANTE: Usamos exactamente el mismo archivo .db que en SQLiteRecordDAO
        string dbPath = Application.persistentDataPath + "/BloxorzDatabase.db";
        connectionString = "URI=file:" + dbPath;

        // Por seguridad, verificamos que la tabla exista
        CrearTablaSiNoExiste();
    }

    private void CrearTablaSiNoExiste()
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                // Mismos nombres exactos que en tu diagrama UML
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Jugador (
                        id_jugador INTEGER PRIMARY KEY AUTOINCREMENT,
                        nombre VARCHAR(20) UNIQUE
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void insertar(string nombre)
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                // Usamos INSERT OR IGNORE para que, si el Lautaro ya existe, 
                // no tire error, simplemente lo ignore y no lo duplique.
                cmd.CommandText = "INSERT OR IGNORE INTO Jugador (nombre) VALUES (@nombre)";
                cmd.Parameters.Add(new SqliteParameter("@nombre", nombre));
                cmd.ExecuteNonQuery();
            }
        }
    }

    public int obtenerIdPorNombre(string nombre)
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                // Buscamos el ID del jugador basándonos en su nombre
                cmd.CommandText = "SELECT id_jugador FROM Jugador WHERE nombre = @nombre";
                cmd.Parameters.Add(new SqliteParameter("@nombre", nombre));

                // ExecuteScalar() nos devuelve solamente la primera celda del resultado (el ID)
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return -1; // Retorna -1 si el jugador no existe
                }
            }
        }
    }
}