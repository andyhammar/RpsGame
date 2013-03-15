namespace RpsGame.Model
{
    public static class ValidationExtensions
    {
        public static bool IsValid(this int firstTo)
        {
            return firstTo > 0;
        }

        public static bool IsValidUser(this string player)
        {
            return !string.IsNullOrWhiteSpace(player);
        }

        public static bool IsWinner(this Move myMove, Move otherMove)
        {
            return (int)myMove == ((int)otherMove + 1) % 3;
        }
    }
}