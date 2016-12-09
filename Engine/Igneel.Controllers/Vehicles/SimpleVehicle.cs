using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Controllers
{
    public class VehicleBase:INameable
    {
        string _name;
        List<Wheel> _wheels = new List<Wheel>();
        Actor _bodyActor;
        PhysicMaterial _carMaterial;
       
        public VehicleBase(Actor actor, PhysicMaterial carMaterial = null)
        {
            if (actor == null)
                throw new ArgumentNullException("actor");
            _bodyActor = actor;

            if (carMaterial == null)
            {
                var matDesc = new PhysicMaterialDesc();
                matDesc.DynamicFriction = 0.4f;
                matDesc.StaticFriction = 0.4f;
                matDesc.Restitution = 0;
                _carMaterial = actor.Scene.CreateMaterial(matDesc);
                _carMaterial.FrictionCombineMode = CombineMode.MULTIPLY;
            }

            foreach (var shape in actor.Shapes)
            {
                if (shape.Material.Index == 0)
                    shape.Material = _carMaterial;
                if (shape is WheelShape)
                {
                    RayCastWheel wheel = new RayCastWheel((WheelShape)shape);
                    _wheels.Add(wheel);
                }
            }
            _bodyActor.UserData = this;
        }

        public string Name { get { return _name; } set { _name = value; } }

        public List<Wheel> Wheells { get { return _wheels; } }

        public Actor BodyActor { get { return _bodyActor; } }

        public PhysicMaterial Material { get { return _carMaterial; } }

        public Wheel GetWheel(string name)
        {
            foreach (var wheel in _wheels)
            {
                if (wheel.Name == name)
                    return wheel;
            }

            return null;
        }
    }
}
