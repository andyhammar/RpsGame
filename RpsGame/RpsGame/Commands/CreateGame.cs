using System;
using RpsGame.Model;

namespace RpsGame.Commands
{
    public class CreateGame : ICommand
    {
        public CreateGame(Guid entityId, string createdBy, string opponent, string reason, int firstTo)
        {
            if (entityId == Guid.Empty
                || !firstTo.IsValid()
                || !createdBy.IsValid()
                || !opponent.IsValid()
                || !reason.IsValid()
            )
                throw new InvalidCommandException(this);

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