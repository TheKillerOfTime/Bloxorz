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
        string dbPath = Application.persistentDataPath + "/BloxorzDatabase.db";
        connectionString = "URI=file:" + dbPath;

        CrearTablasSiNoExisten();
    }

    private void CrearTablasSiNoExisten()
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();

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

    public void insertarPartida(int id_jugador, float tiempo, DateTime fecha)
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Record (id_jugador, tiempo, fecha) VALUES (@id, @t, @f)";
                cmd.Parameters.Add(new SqliteParameter("@id", id_jugador));
                cmd.Parameters.Add(new SqliteParameter("@t", tiempo));
                cmd.Parameters.Add(new SqliteParameter("@f", fecha.ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.ExecuteNonQuery();
            }
        }
    }

    public IDataReader obtenerRanking()
    {
        SqliteConnection conn = new SqliteConnection(connectionString);
        conn.Open();
        SqliteCommand cmd = (SqliteCommand)conn.CreateCommand();

        // 🔹 EL CAMBIO ESTÁ EN EL 'ORDER BY': 
        // Le quitamos 'j.nombre ASC' para que solo le importe el tiempo más bajo
        cmd.CommandText = @"
            SELECT r.id_record, j.nombre, r.tiempo, r.fecha 
            FROM Record r 
            JOIN Jugador j ON r.id_jugador = j.id_jugador
            ORDER BY r.tiempo ASC";

        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }

    //Borra la fila exacta de la base de datos usando su ID
    public void eliminarRecord(int id_record)
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Record WHERE id_record = @id";
                cmd.Parameters.Add(new SqliteParameter("@id", id_record));
                cmd.ExecuteNonQuery();
            }
        }
    }
    public void actualizarNombreRecord(int id_record, string nuevoNombre)
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                // 1. Guardamos el nuevo jugador (Si ya existe, el 'IGNORE' evita errores)
                cmd.CommandText = "INSERT OR IGNORE INTO Jugador (nombre) VALUES (@nombre)";
                cmd.Parameters.Add(new SqliteParameter("@nombre", nuevoNombre));
                cmd.ExecuteNonQuery();

                // 2. Buscamos el ID de ese jugador (nuevo o existente)
                cmd.CommandText = "SELECT id_jugador FROM Jugador WHERE nombre = @nombre";
                int id_jugador = Convert.ToInt32(cmd.ExecuteScalar());

                // 3. Actualizamos el récord para que pertenezca a este jugador
                cmd.CommandText = "UPDATE Record SET id_jugador = @idJugador WHERE id_record = @idRecord";
                cmd.Parameters.Add(new SqliteParameter("@idJugador", id_jugador));
                cmd.Parameters.Add(new SqliteParameter("@idRecord", id_record));
                cmd.ExecuteNonQuery();
            }
        }
    }
}