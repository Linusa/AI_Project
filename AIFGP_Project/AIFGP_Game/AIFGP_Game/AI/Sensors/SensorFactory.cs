namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// SensorFactory singleton.
    /// </summary>
    public class SensorFactory
    {
        public enum SensorTypes
        {
            Rangefinder,
            Radar,
            PieSlice
        }

        private static SensorFactory sensorFactory;

        public ISensor CreateSensor(SensorTypes type)
        {
            ISensor sensor = new Rangefinder();

            if (type == SensorTypes.Rangefinder)
            {
                sensor = createRangefinderSensor();
            }
            else if (type == SensorTypes.Radar)
            {
                sensor = createRadarSensor();
            }
            else if (type == SensorTypes.PieSlice)
            {
                sensor = createPieSliceSensor();
            }

            return sensor;
        }

        public void Initialize(ContentManager content)
        {
            
        }

        private ISensor createRangefinderSensor()
        {
            return new Rangefinder();
        }

        private ISensor createRadarSensor()
        {
            return new Radar();
        }

        private ISensor createPieSliceSensor()
        {
            return new PieSliceSensor();
        }

        public static SensorFactory Instance
        {
            get
            {
                if (sensorFactory == null)
                {
                    sensorFactory = new SensorFactory();
                }

                return sensorFactory;
            }
        }

        private SensorFactory() { }
    }
}
