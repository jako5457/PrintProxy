namespace PrintProxy.Hub.Translators
{
    public static class StatusTranslator
    {

        public static string[] DangerColors = ["error", "stopped","offline"];
        public static string[] InfoColors = ["printing"];
        public static string[] SucessColors = ["completed"];

        public static MudBlazor.Color GetStatusColor(this string text)
        {

            if (DangerColors.Any(t => text.ToLower().Contains(t)))
            {
                return MudBlazor.Color.Error;
            }
            else if (InfoColors.Any(t => text.ToLower().Contains(t)))
            {
                return MudBlazor.Color.Primary;
            }
            else if (SucessColors.Any(t => text.ToLower().Contains(t)))
            {
                return MudBlazor.Color.Success;
            }
            else
            {
                return MudBlazor.Color.Info;
            }
        }

    }
}
