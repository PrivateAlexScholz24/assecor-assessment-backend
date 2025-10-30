namespace AssecorAssessment.Helpers
{
    /// <summary>
    /// Central color mapping used across repository, service, and validation.
    /// </summary>
    public static class ColorMapper
    {
        /// <summary>
        /// Maps color IDs (from CSV) to color names.
        /// </summary>
        public static readonly Dictionary<int, string> IdToNamedColor = new()
        {
            { 1, "blau" },
            { 2, "grün" },
            { 3, "violett" },
            { 4, "rot" },
            { 5, "gelb" },
            { 6, "türkis" },
            { 7, "weiß" }
        };

        /// <summary>
        /// Returns the color name for a given ID, or null if not found.
        /// </summary>
        public static string? GetColorName(int id)
        {
            return IdToNamedColor.TryGetValue(id, out var name) ? name : null;
        }

        /// <summary>
        /// Checks whether a given color name is valid.
        /// </summary>
        public static bool IsValidColor(string colorName)
        {
            foreach (var color in IdToNamedColor.Values)
            {
                if (string.Equals(color, colorName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}