using Microsoft.Data.SqlClient;
using System.Data;
using Model;

class ReviewData : ConnectionSQL
{
    public SqlConnection conn = GetConnect();

    public Review? InsertReview()
    {
        return null;
    }

    public List<Review> getAllReviews()
    {
        return null;
    }

    public Review? updateReview()
    {
        return null;
    }

    public bool deleteReview()
    {
        return false;
    }

    public Review? GetReviewById()
    {
        return null;
    }

}