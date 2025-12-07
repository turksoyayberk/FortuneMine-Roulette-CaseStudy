namespace Core.Utilities
{
    public static class Extensions
    {
        public static string FormatNumber(this int value, int threshold = 1000)
            => ((long)value).FormatNumber(threshold);

        public static string FormatNumber(this long value, int threshold = 1000)
        {
            if (value < threshold)
                return value.ToString();

            if (value < 1_000_000) return (value / 1000f).ToString("0.#") + "K";
            if (value < 1_000_000_000) return (value / 1_000_000f).ToString("0.#") + "M";
            return (value / 1_000_000_000f).ToString("0.#") + "B";
        }
    }
}