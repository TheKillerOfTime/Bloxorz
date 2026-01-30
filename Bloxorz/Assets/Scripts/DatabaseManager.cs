using SQLite;
using System.IO;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }
    private SQLiteConnection _db;
    public static int ultimoID;

    void Awake()
    {
        Instance = this;

        // 1. Definir la ruta de "trabajo" (donde se puede escribir)
        string dbName = "bd.db"; // El nombre exacto de tu archivo
        string destinationPath = Path.Combine(Application.persistentDataPath, dbName);

        // 2. Lógica de copiado (solo la primera vez que se abre el juego)
        if (!File.Exists(destinationPath))
        {
            // En PC/Editor se busca en StreamingAssets
            string sourcePath = Path.Combine(Application.streamingAssetsPath, dbName);
            File.Copy(sourcePath, destinationPath);
            Debug.Log(destinationPath);
        }

        // 3. Abrir la conexión al archivo que YA TIENE las tablas
        _db = new SQLiteConnection(destinationPath);
        
        Debug.Log("Conectado a la base de datos existente.");
    }

    // Ya no necesitas usar CreateTable<>() porque ya las creaste tú
    public void InsertarRecord(float time, string date, int player){
        var record = new Record { tiempo = time, fecha = date, id_jugador = player };
        _db.Insert(record);
    }

    public void InsertarJugador(string name){
        var jugador = new Jugador {nombre = name};
        _db.Insert(jugador);
        ultimoID = jugador.id_jugador;
    }
}