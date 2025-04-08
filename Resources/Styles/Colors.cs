namespace artsy.mobile.Resources.Styles;

public class Colors : ResourceDictionary
{
    public Colors()
    {
        // Manual theme resolution
        bool isDark = Application.Current?.RequestedTheme == AppTheme.Dark;

        // App branding
        this["Primary"] = Color.FromArgb(isDark ? "#393939" : "#E4E2DD");
        this["PrimaryDark"] = Color.FromArgb(isDark ? "#2B2B2B" : "#C8C5BD");
        this["PrimaryDarkText"] = Color.FromArgb(isDark ? "#E4E2DD" : "#242424");
        this["Secondary"] = Color.FromArgb(isDark ? "#ACACAC" : "#DFD8F7");
        this["SecondaryDarkText"] = Color.FromArgb(isDark ? "#C8C8C8" : "#9880e5");
        this["Tertiary"] = Color.FromArgb(isDark ? "#C8C5BD" : "#2B0B98");

        // Standard colors
        this["White"] = Color.FromArgb("#FFFFFF");
        this["Black"] = Color.FromArgb("#000000");
        this["Magenta"] = Color.FromArgb(isDark ? "#FF66D3" : "#D600AA");
        this["MidnightBlue"] = Color.FromArgb(isDark ? "#C8C5BD" : "#190649");
        this["OffBlack"] = Color.FromArgb(isDark ? "#0E0E0E" : "#1F1F1F");

        // Grays (static)
        this["Gray100"] = Color.FromArgb("#E1E1E1");
        this["Gray200"] = Color.FromArgb("#C8C8C8");
        this["Gray300"] = Color.FromArgb("#ACACAC");
        this["Gray400"] = Color.FromArgb("#919191");
        this["Gray500"] = Color.FromArgb("#6E6E6E");
        this["Gray600"] = Color.FromArgb("#404040");
        this["Gray900"] = Color.FromArgb("#212121");
        this["Gray950"] = Color.FromArgb("#141414");

        // Brushes
        AddBrush("Primary");
        AddBrush("PrimaryDark");
        AddBrush("PrimaryDarkText");
        AddBrush("Secondary");
        AddBrush("SecondaryDarkText");
        AddBrush("Tertiary");
        AddBrush("White");
        AddBrush("Black");
        AddBrush("Magenta");
        AddBrush("MidnightBlue");
        AddBrush("OffBlack");
        AddBrush("Gray100");
        AddBrush("Gray200");
        AddBrush("Gray300");
        AddBrush("Gray400");
        AddBrush("Gray500");
        AddBrush("Gray600");
        AddBrush("Gray900");
        AddBrush("Gray950");
    }

    private void AddBrush(string key)
    {
        this[$"{key}Brush"] = new SolidColorBrush((Color)this[key]);
    }
}