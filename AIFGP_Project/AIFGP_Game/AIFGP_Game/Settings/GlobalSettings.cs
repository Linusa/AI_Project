namespace AIFGP_Game
{
    using System.IO;
    using System.Xml.Serialization;

    public class GlobalSettings
    {
        #region Game Options
        public struct GameInfo
        {
            public string Name;
            public string Scenario;

            public bool IsMouseVisible;
            public bool IsFixedTimeStep;
        }

        public GameInfo Game;
        #endregion


        #region Screen
        public struct ScreenInfo
        {
            public bool IsFullScreen;
            public int ResolutionWidth;
            public int ResolutionHeight;
        }

        public ScreenInfo Screen;
        #endregion


        #region Save and Load
        public static void Save(string filename, GlobalSettings settings)
        {
            Stream stream = File.Create(filename);

            XmlSerializer serializer = new XmlSerializer(typeof(GlobalSettings));
            serializer.Serialize(stream, settings);
            stream.Close();
        }

        public static void Load(string filename, out GlobalSettings settings)
        {
            Stream stream = File.OpenRead(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(GlobalSettings));
            settings = (GlobalSettings)serializer.Deserialize(stream);
        }
        #endregion
    }
}
