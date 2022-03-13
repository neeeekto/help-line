using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketFeedback : ValueObject
    {
        public int Score { get; }
        public string? Message { get; }
        public bool? Solved { get; }
        public IReadOnlyDictionary<string, int>? OptionalScores { get; }

        public TicketFeedback(int score, string? message = "", bool? solved = null, IReadOnlyDictionary<string, int>? optionalScores = null)
        {
            Score = score;
            Message = message;
            Solved = solved;
            OptionalScores = optionalScores;
        }
    }
}
