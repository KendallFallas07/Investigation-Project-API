using Microsoft.Data.SqlClient;
using System.Data;
using Model;

class GameData : ConnectionSQL
{
    public SqlConnection conn = GetConnect();

    public Game? InsertGame(string title, string platform, int hoursPlayed, bool isCompleted, string genre)

    {
        Console.WriteLine("Inserting game: " + title);

        var sqlCommand = new SqlCommand("CREATE_GAME", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        sqlCommand.Parameters.AddWithValue("@TITLE", title);
        sqlCommand.Parameters.AddWithValue("@PLATFORM", platform);
        sqlCommand.Parameters.AddWithValue("@HOURS_PLAYED", hoursPlayed);
        sqlCommand.Parameters.AddWithValue("@COMPLETED", isCompleted ? 1 : 0);
        sqlCommand.Parameters.AddWithValue("@GENRE", genre);

        conn.Open();

        SqlDataReader reader = sqlCommand.ExecuteReader();
        if (reader.Read())
        {
            var game = new Game
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                Title = reader.GetString(reader.GetOrdinal("TITLE")),
                Platform = reader.GetString(reader.GetOrdinal("PLATFORM")),
                HoursPlayed = reader.GetInt32(reader.GetOrdinal("HOURS_PLAYED")),
                IsCompleted = reader.GetByte(reader.GetOrdinal("COMPLETED")) == 1,
                Genre = reader.GetString(reader.GetOrdinal("GENRE"))
            };

            conn.Close();
            return game;
        }
        else
        {
            conn.Close();
            return null;
        }
    }

    public List<Game> GetAllGames()
    {
        List<Game> games = new();

        var sqlCommand = new SqlCommand("SELECT_GAME", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        conn.Open();

        SqlDataReader reader = sqlCommand.ExecuteReader();
        while (reader.Read())
        {
            games.Add(new Game
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                Title = reader.GetString(reader.GetOrdinal("TITLE")),
                Platform = reader.GetString(reader.GetOrdinal("PLATFORM")),
                HoursPlayed = reader.GetInt32(reader.GetOrdinal("HOURS_PLAYED")),
                IsCompleted = Convert.ToByte(reader["COMPLETED"]) == 1,
                Genre = reader.GetString(reader.GetOrdinal("GENRE"))
            });
        }

        conn.Close();
        return games;
    }

    public Game? UpdateGame(int id, string title, string platform, int hoursPlayed, bool isCompleted, string genre)
    {
        var sqlCommand = new SqlCommand("UPDATE_GAME", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        sqlCommand.Parameters.AddWithValue("@ID", id);
        sqlCommand.Parameters.AddWithValue("@TITLE", title);
        sqlCommand.Parameters.AddWithValue("@PLATFORM", platform);
        sqlCommand.Parameters.AddWithValue("@HOURS_PLAYED", hoursPlayed);
        sqlCommand.Parameters.AddWithValue("@COMPLETED", isCompleted ? 1 : 0);
        sqlCommand.Parameters.AddWithValue("@GENRE", genre);

        conn.Open();
        SqlDataReader reader = sqlCommand.ExecuteReader();

        if (reader.Read())
        {
            var game = new Game
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                Title = reader.GetString(reader.GetOrdinal("TITLE")),
                Platform = reader.GetString(reader.GetOrdinal("PLATFORM")),
                HoursPlayed = reader.GetInt32(reader.GetOrdinal("HOURS_PLAYED")),
                IsCompleted = reader.GetByte(reader.GetOrdinal("COMPLETED")) == 1,
                Genre = reader.GetString(reader.GetOrdinal("GENRE"))
            };
            conn.Close();
            return game;
        }

        conn.Close();
        return null;
    }

    public bool DeleteGame(int id)
    {
        var sqlCommand = new SqlCommand("DELETE_GAME", conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        sqlCommand.Parameters.AddWithValue("@ID", id);

        conn.Open();
        int affectedRows = sqlCommand.ExecuteNonQuery();
        conn.Close();

        return affectedRows > 0;
    }

    public Game? GetGameById(int id)
    {
        var sqlCommand = new SqlCommand("SELECT_GAME_BY_ID", conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        sqlCommand.Parameters.AddWithValue("@ID", id);

        conn.Open();
        SqlDataReader reader = sqlCommand.ExecuteReader();

        if (reader.Read())
        {
            var game = new Game
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                Title = reader.GetString(reader.GetOrdinal("TITLE")),
                Platform = reader.GetString(reader.GetOrdinal("PLATFORM")),
                HoursPlayed = reader.GetInt32(reader.GetOrdinal("HOURS_PLAYED")),
                IsCompleted = Convert.ToByte(reader["COMPLETED"]) == 1,
                Genre = reader.GetString(reader.GetOrdinal("GENRE"))
            };
            conn.Close();
            return game;
        }

        conn.Close();
        return null;
    }

}