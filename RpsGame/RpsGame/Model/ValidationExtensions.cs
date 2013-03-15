namespace RpsGame.Model
{
    public static class ValidationExtensions
    {
        public static bool IsValid(this int firstTo)
        {
            return firstTo > 0;
        }

        public static bool IsValid(this string player)
        {
            return !string.IsNullOrWhiteSpace(player);
        }
    }
}