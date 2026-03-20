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
}