using SQLite;

public class Jugador
{
    [PrimaryKey, AutoIncrement]
    public int id_jugador { get; set; }
    public string nombre { get; set; }
}