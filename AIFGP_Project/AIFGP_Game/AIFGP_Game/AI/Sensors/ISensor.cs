﻿namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface ISensor : ISpatialEntity, IUpdateable, IDrawable
    {
        bool IsSensingEnabled
        {
            get;
            set;
        }
    }
}
