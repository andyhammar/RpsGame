using System;

namespace RpsGame.Events
{
    class GameCreated : GameEvent
    {
        public GameCreated(Guid gameId, string createdBy, string opponent, string reason, int firstTo) : base(gameId)
        {
            CreatedBy = createdBy;
            Opponent = opponent;
            Reason = reason;
            FirstTo = firstTo;
        }

        public string CreatedBy { get; private set; }
        public string Opponent { get; private set; }
        public string Reason { get; private set; }
        public int FirstTo { get; private set; }
    }
}