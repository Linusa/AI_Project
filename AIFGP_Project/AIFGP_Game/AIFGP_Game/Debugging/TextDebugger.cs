namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TextDebugger<T> : IUpdateable, IDrawable
        where T : IUpdateable, IDrawable
    {
        public T ObjectDebugging;
        public bool Enabled = true;

        protected string objectDebuggingName;
        protected Type typeOfT = typeof(T);

        private Dictionary<string, PropertyInfo> objectProperties = new Dictionary<string, PropertyInfo>();
        private List<string> propertiesDebugging = new List<string>();

        public Vector2 DebugLocation = Vector2.Zero;
        public StringBuilder DebugText = new StringBuilder();

        private bool enabled = false;

        public TextDebugger(T instance)
        {
            ObjectDebugging = instance;
            objectDebuggingName = typeOfT.Name;

            PropertyInfo[] properties = typeOfT.GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (!objectProperties.Keys.Contains(prop.Name))
                {
                    objectProperties.Add(prop.Name, prop);
                }
            }
        }

        public bool DebuggingEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool RegisterProperty(string propToDebug)
        {
            bool success = false;

            if (objectProperties.Keys.Contains(propToDebug))
            {
                propertiesDebugging.Add(propToDebug);
                success = true;
            }

            return success;
        }

        public void Update(GameTime gameTime)
        {
            DebugText.Clear();

            DebugText.AppendLine(objectDebuggingName);
            foreach (string propName in propertiesDebugging)
            {
                PropertyInfo propInfo = objectProperties[propName];

                if (propInfo.CanRead)
                {
                    MethodInfo getMethod = propInfo.GetGetMethod();
                    var propValue = getMethod.Invoke(ObjectDebugging, null);
                    DebugText.AppendFormat("{0}:{1}\n", propName, propValue);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(SensorsGame.DebugFont, DebugText,
                    DebugLocation, Color.Yellow);
        }
    }
}
