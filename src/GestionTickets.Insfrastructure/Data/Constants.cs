namespace GestionTickets.Infrastructure.Data
{
    public static class Constants
    {
        public const string DatabaseFilename = "GestionTickets.db3";
        public const string DatabaseFolder = "GestionTickets";

        public static string DatabasePath
        {
            get
            {
                string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string folderPath = Path.Combine(appDataDirectory, DatabaseFolder);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                return Path.Combine(folderPath, DatabaseFilename);
            }
        }
    }
}