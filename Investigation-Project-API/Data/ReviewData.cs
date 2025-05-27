using Microsoft.Data.SqlClient;
using System.Data;
using Model;

class ReviewData : ConnectionSQL
{
    public SqlConnection conn = GetConnect();

    public Review? InsertReview(int gameId, string reviewerName, string comment, int rating)
    {
        var sqlCommand = new SqlCommand("CREATE_REVIEW", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        sqlCommand.Parameters.AddWithValue("@GAME_ID", gameId);
        sqlCommand.Parameters.AddWithValue("@REVIEWER_NAME", reviewerName);
        sqlCommand.Parameters.AddWithValue("@COMMENT", comment);
        sqlCommand.Parameters.AddWithValue("@RATING", rating);

        conn.Open();
        SqlDataReader reader = sqlCommand.ExecuteReader();

        if (reader.Read())
        {
            var review = new Review
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                GameId = reader.GetInt32(reader.GetOrdinal("GAME_ID")),
                ReviewerName = reader.GetString(reader.GetOrdinal("REVIEWER_NAME")),
                Comment = reader.GetString(reader.GetOrdinal("COMMENT")),
                Rating = reader.GetInt32(reader.GetOrdinal("RATING")),
                ReviewDate = reader.GetDateTime(reader.GetOrdinal("REVIEW_DATE"))
            };

            conn.Close();
            return review;
        }

        conn.Close();
        return null;
    }

    public List<Review> GetAllReviews()
    {
        List<Review> reviews = new();

        var sqlCommand = new SqlCommand("SELECT_REVIEW", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        conn.Open();
        SqlDataReader reader = sqlCommand.ExecuteReader();

        while (reader.Read())
        {
            reviews.Add(new Review
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                GameId = reader.GetInt32(reader.GetOrdinal("GAME_ID")),
                ReviewerName = reader.GetString(reader.GetOrdinal("REVIEWER_NAME")),
                Comment = reader.GetString(reader.GetOrdinal("COMMENT")),
                Rating = reader.GetInt32(reader.GetOrdinal("RATING")),
                ReviewDate = reader.GetDateTime(reader.GetOrdinal("REVIEW_DATE"))
            });
        }

        conn.Close();
        return reviews;
    }

    public Review? GetReviewById(int id)
    {
        var sqlCommand = new SqlCommand("SELECT_REVIEW_BY_ID", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        sqlCommand.Parameters.AddWithValue("@ID", id);

        conn.Open();
        SqlDataReader reader = sqlCommand.ExecuteReader();

        if (reader.Read())
        {
            var review = new Review
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                GameId = reader.GetInt32(reader.GetOrdinal("GAME_ID")),
                ReviewerName = reader.GetString(reader.GetOrdinal("REVIEWER_NAME")),
                Comment = reader.GetString(reader.GetOrdinal("COMMENT")),
                Rating = reader.GetInt32(reader.GetOrdinal("RATING")),
                ReviewDate = reader.GetDateTime(reader.GetOrdinal("REVIEW_DATE"))
            };

            conn.Close();
            return review;
        }

        conn.Close();
        return null;
    }

    public Review? UpdateReview(int id, int gameId, string reviewerName, string comment, int rating)
    {
        var sqlCommand = new SqlCommand("UPDATE_REVIEW", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        sqlCommand.Parameters.AddWithValue("@ID", id);
        sqlCommand.Parameters.AddWithValue("@GAME_ID", gameId);
        sqlCommand.Parameters.AddWithValue("@REVIEWER_NAME", reviewerName);
        sqlCommand.Parameters.AddWithValue("@COMMENT", comment);
        sqlCommand.Parameters.AddWithValue("@RATING", rating);

        conn.Open();
        SqlDataReader reader = sqlCommand.ExecuteReader();

        if (reader.Read())
        {
            var review = new Review
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                GameId = reader.GetInt32(reader.GetOrdinal("GAME_ID")),
                ReviewerName = reader.GetString(reader.GetOrdinal("REVIEWER_NAME")),
                Comment = reader.GetString(reader.GetOrdinal("COMMENT")),
                Rating = reader.GetInt32(reader.GetOrdinal("RATING")),
                ReviewDate = reader.GetDateTime(reader.GetOrdinal("REVIEW_DATE"))
            };

            conn.Close();
            return review;
        }

        conn.Close();
        return null;
    }

    public bool DeleteReview(int id)
    {
        var sqlCommand = new SqlCommand("DELETE_REVIEW", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        sqlCommand.Parameters.AddWithValue("@ID", id);

        conn.Open();
        int affectedRows = sqlCommand.ExecuteNonQuery();
        conn.Close();
        return affectedRows > 0;
    }
}
// This code defines a ReviewData class that interacts with a SQL database to manage game reviews.
// It includes methods to insert, retrieve, update, and delete reviews using stored procedures.