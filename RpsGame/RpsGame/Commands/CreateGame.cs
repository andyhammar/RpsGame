using System;

namespace RpsGame.Commands
{
    public class CreateGame : ICommand
    {
        public CreateGame(Guid entityId, string createdBy, string opponent, string reason, int firstTo)
        {
            AggregateId = entityId;
            CreatedBy = createdBy;
            Opponent = opponent;
            Reason = reason;
            FirstTo = firstTo;
        }

        public Guid AggregateId { get; private set; }
        public string CreatedBy { get; private set; }
        public string Opponent { get; private set; }
        public string Reason { get; private set; }
        public int FirstTo { get; private set; }
    }
}