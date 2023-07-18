using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PM2E2Grupo1.Models;

public class LugaresController
{
    private const string ConnectionString = "";

    public async Task<List<Lugar>> GetLugaresAsync()
    {
        List<Lugar> lugares = new List<Lugar>();

        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT id, audio, latitud, longitud, imagen FROM lugares";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (System.Data.Common.DbDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Lugar lugar = new Lugar
                        {
                            Id = reader.GetInt32(0),
                            Audio = (byte[])reader["audio"],
                            Latitud = reader.GetString(2),
                            Longitud = reader.GetString(3),
                            Imagen = (byte[])reader["imagen"]
                        };
                        lugares.Add(lugar);
                    }
                }
            }
        }

        return lugares;
    }
}