namespace PandaTour.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using PandaTour.Data;
    using PandaTour.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    public class CommentRepository
    {
        private PandaTourContext _context;

        public CommentRepository(PandaTourContext context)
        {
            _context = context;
        }

        public List<Ticket> GetNotCommentedTripsByUserId(int userId)
        {
            var notCommentedTrips = _context.Tickets
                .Where(t => !_context.Comments.Select(c => c.UserId).Contains(userId))
                .Where(t => t.ForDate <= DateTime.UtcNow.Date.AddDays(1)) // We don't want to let users comment on trips they haven't yet made
                .Select(t => t)
                .ToList();

            return notCommentedTrips;
        }

        public bool InsertComment(int ticketId, int userId, string content)
        {
            var comment = new Comment
            {
                TicketId = ticketId,
                UserId = userId,
                Content = content
            };
            _context.Comments.Add(comment);

            bool operationIsSuccessfull = true;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    if (ex.InnerException.InnerException is SqlException sqlException)
                    {
                        if (sqlException.Number == 547) // Constraint check violation
                        {
                            // Later on we can deal specifically with constraint violation
                            operationIsSuccessfull = false;
                        }

                    }
                }

                operationIsSuccessfull = false;
            }

            return operationIsSuccessfull;
        }
    }
}
