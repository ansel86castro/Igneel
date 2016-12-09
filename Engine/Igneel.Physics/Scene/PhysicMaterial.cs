using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Physics
{
    public abstract class PhysicMaterial : Resource
    {
        internal protected Physic scene;

        public object UserData { get; set; }

        public Physic Scene { get { return scene; } }

        public abstract int Index { get; }

        public abstract float DynamicFriction { get; set; }

        public abstract float DynamicFrictionV { get; set; }

        public abstract float StaticFriction { get; set; }

        public abstract float StaticFrictionV { get; set; }

        public abstract float Restitution { get; set; }

        public abstract Vector3 DirOfAnisotropy { get; set; }

        public abstract MaterialFlag Flags { get; set; }

        public abstract CombineMode FrictionCombineMode { get; set; }

        public abstract CombineMode RestitutionCombineMode { get; set; }             
     
    }
}
