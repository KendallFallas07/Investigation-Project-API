using Microsoft.AspNetCore.Mvc;
using Model;
using System.Collections.Generic;

namespace Controller;

[ApiController]
[Route("api/reviews")]
public class ReviewController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Review> Get()
    {
        return new ReviewData().GetAllReviews();
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var review = new ReviewData().GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }
        return Ok(review);
    }

    [HttpGet("game/{id}")]
    public IEnumerable<Review> GetGameReviews(int id)
    {
        return new ReviewData().GetGameReviewsById(id);
    }

    [HttpPost]
    public IActionResult Post([FromForm] Review review)
    {

        if (review.Rating < 1 || review.Rating > 5)
        {
            return BadRequest(new { message = "Rating must be between 1 and 5." });
        }

        if (review.ReviewerName == null || review.Comment == null)
        {
            return BadRequest(new { message = "Invalid fields ReviewerName or Comment." });
        }
        var insertedReview = new ReviewData().InsertReview(review.GameId, review.ReviewerName!, review.Comment!, review.Rating);
        return Ok(insertedReview);
    }

    [HttpPut]
    public IActionResult Put([FromForm] int id, [FromForm] int gameId, [FromForm] string reviewerName, [FromForm] string comment, [FromForm] int rating)
    {
        var review = new ReviewData().GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }

        if (rating < 1 || rating > 5)
        {
            return BadRequest(new { message = "Rating must be between 1 and 5." });
        }

        var updatedReview = new ReviewData().UpdateReview(id, gameId, reviewerName, comment, rating);
        return Ok(updatedReview);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var review = new ReviewData().GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }

        var isDeleted = new ReviewData().DeleteReview(id);

        if (isDeleted)
        {
            return BadRequest();
        }

        return Ok(new { message = "Review deleted successfully." });
    }
}
