namespace PMT_Prototype.Shared
{
    public static class ExtensionMethods
    {
        public static bool IsSuccessStatusCode(this System.Net.HttpStatusCode statusCode)
        {
            return ((int)statusCode >= 200) && ((int)statusCode <= 299);
        }
    }
}
