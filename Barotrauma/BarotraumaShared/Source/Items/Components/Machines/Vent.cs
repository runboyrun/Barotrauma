﻿using System;
using System.Xml.Linq;

namespace Barotrauma.Items.Components
{
    class Vent : ItemComponent
    {
        private float oxygenFlow;

        public float OxygenFlow
        {
            get { return oxygenFlow; }
            set { oxygenFlow = Math.Max(value, 0.0f); }
        }

        public Vent (Item item, XElement element)
            : base(item, element)
        {

        }

        public override void Update(float deltaTime, Camera cam)
        {
            if (item.CurrentHull == null) return;

            if (item.InWater) return;

            item.CurrentHull.Oxygen += oxygenFlow * deltaTime;
            OxygenFlow -= deltaTime * 1000.0f;
        }
    }
}
