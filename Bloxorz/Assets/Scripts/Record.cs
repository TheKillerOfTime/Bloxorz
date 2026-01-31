using SQLite;

public class Record
{
    [PrimaryKey, AutoIncrement]
    public int id_record { get; set; }
    public float tiempo { get; set; }
    public string fecha { get; set; } // SQLite no tiene 'DateTime' nativo, se usa string o long
    
    // Relación: Guardamos el ID del jugador al que pertenece este récord
    [Indexed] // Agregamos un índice para que las búsquedas por jugador sean rápidas
    public int id_jugador { get; set; }
}