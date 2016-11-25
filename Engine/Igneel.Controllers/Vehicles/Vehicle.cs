using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;
using Igneel.SceneManagement;
using NxVec3 = Igneel.Vector3;
using NxMaterial = Igneel.Physics.PhysicMaterial;
using NxActor = Igneel.Physics.Actor;
using NxU32 = System.UInt32;

namespace Igneel.Controllers
{
    public class Vehicle : INameable
    {
        const int NumTrailPoints = 1600;
        const float TrailFrecuency = 0.025f;

        string _name;
        List<Wheel> _wheels = new List<Wheel>();
        List<Vehicle> _childrens = new List<Vehicle>();
        Actor _bodyActor;
        Physic _scene;

        VehicleMotor _vehicleMotor;
        VehicleGears _vehicleGears;

        float _accelerationPedal;
        float _steeringTurnSpeed = Numerics.ToRadians(20.0f);
        float _steeringAngle;
        float _steeringMaxAngleRad = Euler.ToRadians(30.0f);
        float _steeringWheelState;

        float _brakePedal;
        bool _brakePedalChanged;
        bool _handBrake;
        float _acceleration;

        float _motorForce = 3500f;
        float _transmissionEfficiency = 1.0f;
        float _differentialRatio = 1.0f;

        NxVec3 _localVelocity;
        bool _braking;
        bool _releaseBraking;
        float _maxVelocity = 80;
        NxMaterial _carMaterial;
        NxActor _mostTouchedActor;

        public event Action<Vehicle, float> ControlVehicle;
        IVehicleInputMap _forward, _backward, _left, _right, _handBrakeMap;

        object _userData;
        private float _digitalSteeringDelta;

        public Vehicle(Actor actor, PhysicMaterial carMaterial = null, VehicleMotor motor = null, VehicleGears gears = null)
        {
            if (actor == null)
                throw new ArgumentNullException("actor");

            _transmissionEfficiency = 1.0f;
            _differentialRatio = 1.0f;
            _maxVelocity = 80;

            _bodyActor = actor;
            _scene = actor.Scene;
            _carMaterial = carMaterial;
            _bodyActor.SleepEnergyThreshold = 0.05f;
            _vehicleMotor = motor;
            _vehicleGears = gears;

            if (carMaterial == null)
            {
                PhysicMaterialDesc matDesc = new PhysicMaterialDesc();
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
            _motorForce = 0;

            SetDefaulValues();

            Control(0, true, 0, true, false);
        }

        public Vehicle(Frame node, PhysicMaterial carMaterial = null, VehicleMotor motor = null, VehicleGears gears = null) :
            this((Actor)node.Affector, carMaterial, motor, gears)
        {
                   
        }

        public string Name { get { return _name; } set { _name = value; } }

        public List<Vehicle> Childrens { get { return _childrens; } }

        public List<Wheel> Wheells { get { return _wheels; } }

        public float DriveVelocity { get { return Math.Abs(_localVelocity.X); } }

        public float MaxVelocity { get { return _maxVelocity; } set { _maxVelocity = value; } }

        public VehicleMotor Motor
        {
            get { return _vehicleMotor; }
            set
            {
                _vehicleMotor = value;
                if (_vehicleMotor != null)
                    _motorForce = 0;
            }
        }

        public VehicleGears Gears { get { return _vehicleGears; } set { _vehicleGears = value; } }

        public Actor BodyActor { get { return _bodyActor; } }

        public Matrix GlobalPose { get { return _bodyActor.GlobalPose; } }

        public float MotorForce
        {
            get { return _motorForce; }
            set
            {
                if (_vehicleMotor != null) throw new InvalidOperationException("This vehicle has a motor, this operation is only allowed to motorless vehicles");
                _motorForce = value;
            }
        }

        public NxMaterial CarMaterial
        {
            get { return _carMaterial; }
            set
            {
                _carMaterial = value;
                foreach (var wheel in _wheels)
                {
                    if (wheel is RayCastWheel)
                    {
                        WheelShape shape = ((RayCastWheel)wheel).ActorShape;
                        shape.Material = _carMaterial;
                    }
                }
            }
        }

        public object UserData { get { return _userData; } set { _userData = value; } }

        public IVehicleInputMap Forward { get { return _forward; } set { _forward = value; } }

        public IVehicleInputMap Backward { get { return _backward; } set { _backward = value; } }

        public IVehicleInputMap Left { get { return _left; } set { _left = value; } }

        public IVehicleInputMap Right { get { return _right; } set { _right = value; } }

        public IVehicleInputMap HandBrake { get { return _handBrakeMap; } set { _handBrakeMap = value; } }

        public float Mass
        {
            get
            {
                return _bodyActor != null ? _bodyActor.Mass : -1;
            }
            set
            {
                if (_bodyActor != null)
                    _bodyActor.Mass = value;
            }
        }

        public float DigitalSteeringDelta
        {
            get { return _digitalSteeringDelta; }
            set { _digitalSteeringDelta = value; }
        }

        public float SteeringMaxAngleRad
        {
            get { return _steeringMaxAngleRad; }
            set { _steeringMaxAngleRad = value; }
        }

        public Vector3 Position
        {
            get { if (_bodyActor != null) return _bodyActor.GlobalPosition; else throw new InvalidOperationException(); }
            set { if (_bodyActor != null) _bodyActor.GlobalPosition = value; }
        }

        #region Private Methods

        private void _ComputeMostTouchedActor()
        {
            Dictionary<Actor, int> actorLookup = new Dictionary<NxActor, int>();
            foreach (var whell in _wheels)
            {
                var actor = whell.GetTouchedActor();
                if (actor != null)
                {
                    if (actorLookup.ContainsKey(actor))
                        actorLookup[actor]++;
                    else
                        actorLookup[actor] = 1;
                }
            }

            int count = 0;
            foreach (var item in actorLookup)
            {
                if (item.Value > count)
                {
                    count = item.Value;
                    _mostTouchedActor = item.Key;
                }
            }

        }

        private unsafe void _ComputeLocalVelocity()
        {
            NxVec3 relativeVelocity;
            _ComputeMostTouchedActor();

            if (_mostTouchedActor == null || !_mostTouchedActor.IsDynamic)
                relativeVelocity = _bodyActor.LinearVelocity;
            else
                relativeVelocity = _bodyActor.LinearVelocity - _mostTouchedActor.LinearVelocity;

            _localVelocity = relativeVelocity;
            Matrix rotation = Matrix.Invert( _bodyActor.GlobalOrientation);            
            Vector3.TransformNormal(ref _localVelocity, ref rotation, out _localVelocity);
        }

        private float _ComputeAxisTorque()
        {
            if (_vehicleMotor != null)
            {
                float rpm = _ComputeRpmFromWheels();
                float motorRpm = _ComputeMotorRpm(rpm);
                _vehicleMotor.Rpm = motorRpm;
                float torque = _accelerationPedal * _vehicleMotor.Torque;
                return torque * _GetGearRatio() * _differentialRatio * _transmissionEfficiency;
            }
            else
            {
                return _accelerationPedal * _motorForce;
            }
        }

        private float _ComputeRpmFromWheels()
        {
            float wheelRpms = 0;
            int nbWheels = 0;
            foreach (var wheel in _wheels)
            {
                if (wheel.HasFlag(WheelFlags.Accelerated))
                {
                    nbWheels++;
                    wheelRpms += wheel.Rpm;
                }
            }

            return wheelRpms / (float)nbWheels;
        }

        private float _GetGearRatio()
        {
            return _vehicleGears == null ? 1.0f : _vehicleGears.CurrentGearRatio;
        }

        private float _ComputeMotorRpm(float rpm)
        {
            float temp = _GetGearRatio() * _differentialRatio;
            float motorRpm = rpm * temp;
            if (_vehicleMotor != null)
            {
                int change;
                if (_vehicleGears != null && (change = _vehicleMotor.ChangeGears(_vehicleGears, 0.2f)) != 0)
                {
                    if (change == 1)
                        GearUp();
                    else
                        GearDown();
                }
                temp = _GetGearRatio() * _differentialRatio;
                motorRpm = Math.Max(motorRpm, _vehicleMotor.MinRpm);
            }
            return motorRpm;
        }

        private void _UpdateRpms()
        {
            if (_vehicleMotor != null)
            {
                var rpm = _ComputeRpmFromWheels();
                var motorRpm = _ComputeMotorRpm(rpm);
                _vehicleMotor.Rpm = motorRpm;
            }
        }

        private void _ControlSteering(float steering, bool analogSteering)
        {
            //if (analogSteering)
            //    _steeringWheelState = steering;
            //else if (Math.Abs(steering) > 0.0001f)
            //    _steeringWheelState += Math.Sign(steering) * _digitalSteeringDelta;
            //else if (Math.Abs(_steeringWheelState) > 0.0001f)
            //    _steeringWheelState -= Math.Sign(_steeringWheelState) * _digitalSteeringDelta;

            //_steeringWheelState = Numerics.Clamp(_steeringWheelState, -1.0f, 1.0f);

            //if (Math.Abs(steering) > 0.0001f)
            //    _steeringWheelState = steering;
            //else if (Math.Abs(_steeringWheelState) > 0.0001f)
            //{
            //    _steeringWheelState = 
            //}
            _steeringWheelState = steering;
        }

        private void _ControlAcceleration(float acceleration, bool analogAcceleration)
        {
            if (Math.Abs(acceleration) < 0.001f)
                _releaseBraking = true;
            if (!_braking)
            {
                _accelerationPedal = Numerics.Clamp(acceleration, -1, 1);
                _brakePedalChanged = _brakePedal == 0;
                _brakePedal = 0;
            }
            else
            {
                _accelerationPedal = 0;
                float newv = Numerics.Saturate(Math.Abs(acceleration));
                _brakePedalChanged = _brakePedal == newv;
                _brakePedal = newv;
            }
        }

        private void _NodeUpdateEvent(IDynamic sender, float deltaT)
        {
            OnControl(deltaT);
            //UpdateVehicle(deltaT);
        }

        private bool IsActive(IVehicleInputMap input)
        {
            return input != null && input.IsActive;
        }

        protected void OnControl(float deltaT)
        {
            if (ControlVehicle != null)
                ControlVehicle(this, deltaT);
            else
            {
                float acceleration = 0;
                float steering = 0;
                bool handBrake = IsActive(_handBrakeMap);

                if (IsActive(_forward) && !IsActive(_backward))
                    acceleration = 1;
                else if (!IsActive(_forward) && IsActive(_backward))
                    acceleration = -1;

                if (IsActive(_right) && !IsActive(_left))
                    steering = 1;
                else if (!IsActive(_right) && IsActive(_left))
                    steering = -1;

                Control(steering, false, acceleration, false, handBrake);

                UpdateVehicle(deltaT);           
            }
        }

        #endregion

        #region Public Methods

        public void GearUp()
        {
            if (_vehicleGears != null)
            {
                _vehicleGears.GearDown();
            }
        }

        public void GearDown()
        {
            if (_vehicleGears != null)
                _vehicleGears.GearUp();
        }

        public void Control(float steering, bool analogSteering, float acceleration, bool analogAcceleration, bool handBrake)
        {
            if (steering != 0 || acceleration != 0 || handBrake && _bodyActor.IsSleeping)
            {
                _bodyActor.WakeUp(0.4f);
            }

            _ControlSteering(steering, analogSteering);

            _ComputeLocalVelocity();
            if (!_braking || _releaseBraking)
            {
                _braking = _localVelocity.X * acceleration < -0.1f;
                _releaseBraking = false;
            }
            if (_handBrake != handBrake)
            {
                _handBrake = handBrake;
                _brakePedalChanged = true;
            }

            _ControlAcceleration(acceleration, analogAcceleration);
        }

        public void UpdateVehicle(float elapseTime)
        {
            //float distanceSteeringAxisCarTurnAxis = _steeringSteerPoint.X - _steeringTurnPoint.X;

            //if (_steeringSteerPoint.Z != _steeringTurnPoint.Z)
            //    throw new InvalidOperationException();

            //float distance2 = 0;
            //if (Math.Abs(_steeringWheelState) > 0.01f)
            //    distance2 = distanceSteeringAxisCarTurnAxis / (float)Math.Tan(_steeringWheelState * _steeringMaxAngleRad);        

            _steeringAngle += _steeringTurnSpeed * elapseTime * _steeringWheelState;
            _steeringAngle = Numerics.Clamp(_steeringAngle, -_steeringMaxAngleRad, SteeringMaxAngleRad);

            float nbTouching = 0;
            float nbNotTouching = 0;
            float nbHandBrake = 0;

            #region Steering

            foreach (var wheel in _wheels)
            {
                if (wheel.HasFlag(WheelFlags.SteerableInput))
                {
                    wheel.Angle = _steeringAngle;

                    //if (_steeringWheelState != 0)
                    //{
                    //    var wheelPos = wheel.Position;
                    //    float dz = -wheelPos.Z + distance2;
                    //    float dx = wheelPos.X - _steeringTurnPoint.X;
                    //    float angle = (float)Math.Atan(dx / dz);
                    //    wheel.Angle = angle;                        
                    //}
                    //else
                    //    wheel.Angle = 0f;
                }
                else if (wheel.HasFlag(WheelFlags.SteerableAuto))
                {
                    var localVelocity = _bodyActor.GetLocalPointVelocity(wheel.Position);
                    var rotation =Matrix.Invert( _bodyActor.GlobalOrientation);                    
                    var globalVelocity = Vector3.TransformNormal(localVelocity, rotation);
                    globalVelocity.Y = 0;

                    if (globalVelocity.LengthSquared() < 0.01f)
                        wheel.Angle = 0.0f;
                    else
                    {
                        globalVelocity.Normalize();
                        if (globalVelocity.X < 0)
                            globalVelocity = -globalVelocity;
                        float angle = Numerics.Clamp((float)Math.Atan(globalVelocity.Z / globalVelocity.X), -0.3f, 0.3f);
                        wheel.Angle = angle;
                    }
                }

                if (!wheel.HasFlag(WheelFlags.Accelerated))
                    continue;

                if (_handBrake && wheel.HasFlag(WheelFlags.AffectedByHandbrake))
                    nbHandBrake++;
                else
                {
                    if (!wheel.HasGroundContact())
                        nbNotTouching++;
                    else
                        nbTouching++;
                }
            }

            #endregion

            float motorTorque = 0.0f;
            if (nbTouching > 0 && (Math.Abs(_accelerationPedal) > 0.01f))
            {
                float axisTorque = _ComputeAxisTorque();
                float wheelTorque = axisTorque / (float)(_wheels.Count - nbHandBrake);
                float wheelTorqueNotTouching = nbNotTouching > 0 ? wheelTorque * (float)Math.Pow(0.5, (float)nbNotTouching) : 0;
                float wheelTorqueTouching = wheelTorque - wheelTorqueNotTouching;
                motorTorque = wheelTorqueTouching / (float)nbTouching;
            }
            else
                _UpdateRpms();

            foreach (var wheel in _wheels)
            {
                wheel.Tick(_handBrake, motorTorque, _brakePedal, elapseTime);
            }

        }

        public void ApplyRandomForce()
        {
            Vector3 pos = new NxVec3(Rand.Uniform(-4.0f, 4.0f), Rand.Uniform(-4.0f, 4.0f), Rand.Uniform(-4.0f, 4.0f));
            float force = Rand.Uniform(_bodyActor.Mass * 0.5f, _bodyActor.Mass * 2.0f);
            _bodyActor.AddForceAtLocalPos(new Vector3(0, force * 100.0f, 0), pos);
        }

        public void StandUp()
        {
            NxVec3 pos = _bodyActor.GlobalPosition + new Vector3(0, 2, 0);
            Matrix rot = _bodyActor.GlobalOrientation;
            NxVec3 front = Vector3.TransformNormal(new NxVec3(1, 0, 0), rot);
            front.Y = 0;
            front.Normalize();

            float dotproduct = front.X;

            float angle = Math.Sign(-front.Z) * (float)Math.Acos(dotproduct);

            rot = Matrix.RotationY(angle);

            _bodyActor.GlobalPosition = pos;
            _bodyActor.GlobalOrientation = rot;
            _bodyActor.LinearVelocity = new NxVec3(0, 0, 0);
            _bodyActor.AngularVelocity = new NxVec3(0, 0, 0);
        }

        public void SetSuspention(SpringDesc suspention)
        {
            foreach (var wheel in _wheels)
            {
                if (wheel is RayCastWheel)
                {
                    WheelShape shape = ((RayCastWheel)wheel).ActorShape;
                    shape.Suspension = suspention;
                }
            }
        }

        //public void SetLongitudinalTireFunction(TireFunctionDesc longitudinalFunction)
        //{
        //    foreach (var wheel in _wheels)
        //    {
        //        if (wheel is RayCastWheel)
        //        {
        //            WheelShape shape = ((RayCastWheel)wheel).ActorShape;
        //            shape.LongitudalTireForceFunction = longitudinalFunction;
        //        }
        //    }
        //}

        //public void SetLateralTireFunction(TireFunctionDesc lateralFunction)
        //{
        //    foreach (var wheel in _wheels)
        //    {
        //        if (wheel is RayCastWheel)
        //        {
        //            WheelShape shape = ((RayCastWheel)wheel).ActorShape;
        //            shape.LateralTireForceFunction = lateralFunction;
        //        }
        //    }
        //}

        public void SetDefaulValues()
        {
            if (_bodyActor != null)
                _bodyActor.Mass = 1200;

            _steeringMaxAngleRad = Euler.ToRadians(30.0f);
            _motorForce = 3500f;

            _transmissionEfficiency = 1.0f;
            _differentialRatio = 1.0f;
            _maxVelocity = 80;
        }

        public Wheel GetWheel(string name)
        {
            foreach (var wheel in _wheels)
            {
                if (wheel.Name == name)
                    return wheel;
            }

            return null;
        }

        #endregion


    }

    //public class Vehicle:INameable
    //{
    //    const int NUM_TRAIL_POINTS = 1600;
    //    const float TRAIL_FRECUENCY = 0.025f;

    //    string name;
    //    List<Wheel> _wheels= new List<Wheel>();
    //    List<Vehicle> _childrens = new List<Vehicle>();
    //    Actor _bodyActor;
    //    PhysicScene _scene;

    //    VehicleMotor _vehicleMotor;
    //    VehicleGears _vehicleGears;
        
    //    float _accelerationPedal;
    //    float _steeringTurnSpeed = Numerics.ToRadians(20.0f);
    //    float _steeringAngle;
    //    float _steeringMaxAngleRad = Euler.ToRadians(30.0f);
    //    float _steeringWheelState;

    //    float _brakePedal;
    //    bool _brakePedalChanged;
    //    bool _handBrake;
    //    float _acceleration;
       
    //    float _motorForce = 3500f;
    //    float _transmissionEfficiency = 1.0f;
    //    float _differentialRatio = 1.0f;

    //    NxVec3 _localVelocity;
    //    bool _braking;
    //    bool _releaseBraking;
    //    float _maxVelocity = 80;
    //    NxMaterial _carMaterial;          
    //    NxActor _mostTouchedActor;

    //    public event Action<Vehicle, float> ControlVehicle;
    //    IVehicleInputMap _forward, _backward, _left, _right, _handBrakeMap;

    //    object userData;

    //    public Vehicle(Actor actor, PhysicMaterial carMaterial = null, VehicleMotor motor = null, VehicleGears gears = null)
    //    {
    //        if(actor == null)
    //            throw new ArgumentNullException("actor");

    //        _transmissionEfficiency = 1.0f;
    //        _differentialRatio = 1.0f;
    //        _maxVelocity = 80;

    //        _bodyActor = actor;
    //        _scene = actor.Scene;
    //        _carMaterial = carMaterial;
    //        _bodyActor.SleepEnergyThreshold = 0.05f;
    //        _vehicleMotor = motor;
    //        _vehicleGears = gears;

    //        if (carMaterial == null)
    //        {
    //            MaterialDesc matDesc = new MaterialDesc();
    //            matDesc.DynamicFriction = 0.4f;
    //            matDesc.StaticFriction = 0.4f;
    //            matDesc.Restitution = 0;
    //            _carMaterial =  new PhysicMaterial(_scene, matDesc);
    //            _carMaterial.FrictionCombineMode = CombineMode.MULTIPLY;
    //        }

    //        foreach (var shape in actor.Shapes)
    //        {
    //            if (shape.NativeMaterialIndex == 0)                
    //                shape.NativeMaterialIndex = _carMaterial.NativeMaterialIndex;
    //            if (shape is WheelShape)
    //            {
    //                RayCastWheel wheel = new RayCastWheel((WheelShape)shape);
    //                _wheels.Add(wheel);
    //            }
    //        }
    //        _bodyActor.UserData = this;
    //        _motorForce = 0;

    //        SetDefaulValues();

    //        Control(0, true, 0, true, false);
    //    }

    //    public Vehicle(SceneNode node, PhysicMaterial carMaterial = null, VehicleMotor motor = null, VehicleGears gears = null) :
    //        this((Actor)node.Affector, carMaterial, motor, gears)
    //    {
    //        if (!node.IsDynamic)
    //            node.IsDynamic = true;
    //        node.UpdateEvent += _NodeUpdateEvent;
    //    }        

    //    public string Name { get { return name; } set { name = value; } }

    //    public List<Vehicle> Childrens { get { return _childrens; } }

    //    public List<Wheel> Wheells { get { return _wheels; } }

    //    public float DriveVelocity { get { return Math.Abs(_localVelocity.X); } }           

    //    public float MaxVelocity { get { return _maxVelocity; } set { _maxVelocity = value; } }

    //    public VehicleMotor Motor
    //    {
    //        get { return _vehicleMotor; }
    //        set
    //        {
    //            _vehicleMotor = value;
    //            if (_vehicleMotor != null)
    //                _motorForce = 0;
    //        }
    //    }

    //    public VehicleGears Gears { get { return _vehicleGears; } set { _vehicleGears = value; } }

    //    public Actor BodyActor { get { return _bodyActor; } }       

    //    public Matrix GlobalPose { get { return _bodyActor.GlobalPose; } }

    //    public float MotorForce
    //    {
    //        get { return _motorForce; }
    //        set
    //        {
    //            if (_vehicleMotor != null) throw new InvalidOperationException("This vehicle has a motor, this operation is only allowed to motorless vehicles");
    //            _motorForce = value;
    //        }
    //    }

    //    public NxMaterial CarMaterial
    //    {
    //        get { return _carMaterial; }
    //        set
    //        {
    //            _carMaterial = value;
    //            foreach (var wheel in _wheels)
    //            {
    //                if (wheel is RayCastWheel)
    //                {
    //                    WheelShape shape = ((RayCastWheel)wheel).ActorShape;
    //                    shape.NativeMaterialIndex = _carMaterial.NativeMaterialIndex;
    //                }
    //            }
    //        }
    //    }

    //    public object UserData { get { return userData; } set { userData = value; } }

    //    public IVehicleInputMap Forward { get { return _forward; } set { _forward = value; } }
       
    //    public IVehicleInputMap Backward { get { return _backward; } set { _backward = value; } }
       
    //    public IVehicleInputMap Left { get { return _left; } set { _left = value; } }
     
    //    public IVehicleInputMap Right { get { return _right; } set { _right = value; } }
       
    //    public IVehicleInputMap HandBrake { get { return _handBrakeMap; } set { _handBrakeMap = value; } }

    //    public float Mass 
    //    {
    //        get
    //        {
    //            return _bodyActor != null ? _bodyActor.Mass : -1;
    //        }
    //        set
    //        {
    //            if (_bodyActor != null)
    //                _bodyActor.Mass = value;
    //        }
    //    }

    //    //public float DigitalSteeringDelta
    //    //{
    //    //    get { return _digitalSteeringDelta; }
    //    //    set { _digitalSteeringDelta = value; }
    //    //}

    //    public float SteeringMaxAngleRad
    //    {
    //        get { return _steeringMaxAngleRad; }
    //        set { _steeringMaxAngleRad = value; }
    //    }

    //    public Vector3 Position
    //    {
    //        get { if (_bodyActor != null) return _bodyActor.GlobalPosition; else throw new InvalidOperationException(); }
    //        set { if (_bodyActor != null) _bodyActor.GlobalPosition = value; }
    //    }

    //    #region Private Methods

    //    private void _ComputeMostTouchedActor()
    //    {
    //        Dictionary<Actor,int> actorLookup = new Dictionary<NxActor,int>();
    //        foreach (var whell in _wheels)
    //        {
    //            var actor = whell.GetTouchedActor();
    //            if (actor != null)
    //            {
    //                if (actorLookup.ContainsKey(actor))
    //                    actorLookup[actor]++;
    //                else
    //                    actorLookup[actor] = 1;
    //            }
    //        }

    //        int count = 0;
    //        foreach (var item in actorLookup)
    //        {
    //            if (item.Value > count)
    //            {
    //                count = item.Value;
    //                _mostTouchedActor = item.Key;
    //            }
    //        }

    //    }

    //    private unsafe void _ComputeLocalVelocity()
    //    {
    //        NxVec3 relativeVelocity;
    //        _ComputeMostTouchedActor();            

    //        if (_mostTouchedActor == null || !_mostTouchedActor.IsDynamic)
    //            relativeVelocity = _bodyActor.LinearVelocity;
    //        else
    //            relativeVelocity = _bodyActor.LinearVelocity - _mostTouchedActor.LinearVelocity;

    //        _localVelocity = relativeVelocity;
    //        Matrix rotation = _bodyActor.GlobalOrientation;
    //        rotation.Invert();
    //        Vector3.TransformNormal(ref _localVelocity, ref rotation, out _localVelocity);
    //    }

    //    private float _ComputeAxisTorque()
    //    {
    //        if (_vehicleMotor != null)
    //        {
    //            float rpm = _ComputeRpmFromWheels();
    //            float motorRpm = _ComputeMotorRpm(rpm);
    //            _vehicleMotor.Rpm = motorRpm;
    //            float torque = _accelerationPedal * _vehicleMotor.Torque;
    //            return torque * _GetGearRatio() * _differentialRatio * _transmissionEfficiency;
    //        }
    //        else
    //        {              
    //            return _accelerationPedal * _motorForce;
    //        }
    //    }

    //    private float _ComputeRpmFromWheels()
    //    {
    //        float wheelRpms = 0;
    //        int nbWheels = 0;
    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel.HasFlag(WheelFlags.ACCELERATED))
    //            {
    //                nbWheels++;
    //                wheelRpms += wheel.Rpm;
    //            }
    //        }

    //        return wheelRpms / (float)nbWheels;
    //    }

    //    private float _GetGearRatio()
    //    {
    //        return _vehicleGears == null ? 1.0f : _vehicleGears.CurrentGearRatio;
    //    }

    //    private float _ComputeMotorRpm(float rpm)
    //    {
    //        float temp = _GetGearRatio() * _differentialRatio;
    //        float motorRpm = rpm * temp;
    //        if(_vehicleMotor != null)
    //        {
    //            int change;
    //            if(_vehicleGears!=null && (change = _vehicleMotor.ChangeGears(_vehicleGears, 0.2f))!= 0)
    //            {
    //                if(change == 1)			        
    //                    GearUp();
    //                else				        
    //                    GearDown();			        
    //            }
    //            temp = _GetGearRatio() * _differentialRatio;
    //            motorRpm = Math.Max(motorRpm, _vehicleMotor.MinRpm);
    //        }
    //        return motorRpm;
    //    }

    //    private void _UpdateRpms()
    //    {            
    //        if (_vehicleMotor != null)
    //        {
    //            var rpm = _ComputeRpmFromWheels();
    //            var motorRpm = _ComputeMotorRpm(rpm);
    //            _vehicleMotor.Rpm = motorRpm;
    //        }
    //    }

    //    private void _ControlSteering(float steering, bool analogSteering)
    //    {
    //        //if (analogSteering)
    //        //    _steeringWheelState = steering;
    //        //else if (Math.Abs(steering) > 0.0001f)
    //        //    _steeringWheelState += Math.Sign(steering) * _digitalSteeringDelta;
    //        //else if (Math.Abs(_steeringWheelState) > 0.0001f)
    //        //    _steeringWheelState -= Math.Sign(_steeringWheelState) * _digitalSteeringDelta;

    //        //_steeringWheelState = Numerics.Clamp(_steeringWheelState, -1.0f, 1.0f);

    //        //if (Math.Abs(steering) > 0.0001f)
    //        //    _steeringWheelState = steering;
    //        //else if (Math.Abs(_steeringWheelState) > 0.0001f)
    //        //{
    //        //    _steeringWheelState = 
    //        //}
    //        _steeringWheelState = steering;
    //    }

    //    private void _ControlAcceleration(float acceleration, bool analogAcceleration)
    //    {
    //        if (Math.Abs(acceleration) < 0.001f)
    //            _releaseBraking = true;
    //        if (!_braking)
    //        {
    //            _accelerationPedal = Numerics.Clamp(acceleration, -1, 1);
    //            _brakePedalChanged = _brakePedal == 0;
    //            _brakePedal = 0;
    //        }
    //        else
    //        {
    //            _accelerationPedal = 0;
    //            float newv = Numerics.Saturate(Math.Abs(acceleration));
    //            _brakePedalChanged = _brakePedal == newv;
    //            _brakePedal = newv;
    //        }
    //    }

    //    private void _NodeUpdateEvent(IDynamic sender, float deltaT)
    //    {
    //        OnControl(deltaT);
    //        //UpdateVehicle(deltaT);
    //    }

    //    private bool IsActive(IVehicleInputMap input)
    //    {
    //        return input != null && input.IsActive;
    //    }

    //    protected void OnControl(float deltaT)
    //    {
    //        if (ControlVehicle != null)
    //            ControlVehicle(this, deltaT);
    //        else
    //        {
    //            float acceleration = 0;
    //            float steering = 0;
    //            bool handBrake = IsActive(_handBrakeMap);

    //            if (IsActive(_forward) && !IsActive(_backward))
    //                acceleration = 1;
    //            else if (!IsActive(_forward) && IsActive(_backward))
    //                acceleration = -1;

    //            if (IsActive(_right) && !IsActive(_left))
    //                steering = 1;
    //            else if (!IsActive(_right) && IsActive(_left))
    //                steering = -1;

    //            Control(steering, false, acceleration, false, handBrake);

    //            UpdateVehicle(deltaT);

    //            //foreach (var wheel in _wheels)
    //            //{
    //            //    if (wheel.HasFlag(WheelFlags.STEERABLE_INPUT))
    //            //    {
    //            //        wheel.Angle += steering * Numerics.ToRadians(30) * deltaT;
    //            //    }

    //            //    //if (wheel.HasFlag(WheelFlags.ACCELERATED))
    //            //        wheel.Tick(handBrake, acceleration * _motorForce, _brakePedal, deltaT);

    //            //}
    //        }
    //    }

    //    #endregion

    //    #region Public Methods

    //    public void GearUp()
    //    {
    //        if (_vehicleGears != null)
    //        {
    //            _vehicleGears.GearDown();
    //        }
    //    }

    //    public void GearDown()
    //    {
    //        if (_vehicleGears != null)
    //            _vehicleGears.GearUp();
    //    }

    //    public void Control(float steering, bool analogSteering, float acceleration, bool analogAcceleration, bool handBrake)
    //    {
    //        if (steering != 0 || acceleration != 0 || handBrake && _bodyActor.IsSleeping)
    //        {              
    //            _bodyActor.WakeUp(0.4f);
    //        }

    //        _ControlSteering(steering, analogSteering);

    //        _ComputeLocalVelocity();
    //        if (!_braking || _releaseBraking)
    //        {
    //            _braking = _localVelocity.X * acceleration < -0.1f;
    //            _releaseBraking = false;
    //        }
    //        if (_handBrake != handBrake)
    //        {
    //            _handBrake = handBrake;
    //            _brakePedalChanged = true;
    //        }

    //        _ControlAcceleration(acceleration, analogAcceleration);
    //    }

    //    public void UpdateVehicle(float elapseTime)
    //    {
    //        //float distanceSteeringAxisCarTurnAxis = _steeringSteerPoint.X - _steeringTurnPoint.X;

    //        //if (_steeringSteerPoint.Z != _steeringTurnPoint.Z)
    //        //    throw new InvalidOperationException();

    //        //float distance2 = 0;
    //        //if (Math.Abs(_steeringWheelState) > 0.01f)
    //        //    distance2 = distanceSteeringAxisCarTurnAxis / (float)Math.Tan(_steeringWheelState * _steeringMaxAngleRad);        

    //        _steeringAngle += _steeringTurnSpeed * elapseTime * _steeringWheelState;
    //        _steeringAngle = Numerics.Clamp(_steeringAngle, -_steeringMaxAngleRad, SteeringMaxAngleRad);

    //        float nbTouching = 0;
    //        float nbNotTouching = 0;
    //        float nbHandBrake = 0;

    //        #region Steering

    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel.HasFlag(WheelFlags.STEERABLE_INPUT))
    //            {
    //                wheel.Angle = _steeringAngle;

    //                //if (_steeringWheelState != 0)
    //                //{
    //                //    var wheelPos = wheel.Position;
    //                //    float dz = -wheelPos.Z + distance2;
    //                //    float dx = wheelPos.X - _steeringTurnPoint.X;
    //                //    float angle = (float)Math.Atan(dx / dz);
    //                //    wheel.Angle = angle;                        
    //                //}
    //                //else
    //                //    wheel.Angle = 0f;
    //            }
    //            else if (wheel.HasFlag(WheelFlags.STEERABLE_AUTO))
    //            {
    //                var localVelocity = _bodyActor.GetLocalPointVelocity(wheel.Position);
    //                var rotation = _bodyActor.GlobalOrientation;
    //                rotation.Invert();
    //                var globalVelocity = Vector3.TransformNormal(localVelocity, rotation);
    //                globalVelocity.Y = 0;

    //                if (globalVelocity.LengthSquared() < 0.01f)
    //                    wheel.Angle = 0.0f;
    //                else
    //                {
    //                    globalVelocity.Normalize();
    //                    if (globalVelocity.X < 0)
    //                        globalVelocity = -globalVelocity;
    //                    float angle = Numerics.Clamp((float)Math.Atan(globalVelocity.Z / globalVelocity.X), -0.3f, 0.3f);
    //                    wheel.Angle = angle;
    //                }
    //            }

    //            if (!wheel.HasFlag(WheelFlags.ACCELERATED))
    //                continue;

    //            if (_handBrake && wheel.HasFlag(WheelFlags.AFFECTED_BY_HANDBRAKE))
    //                nbHandBrake++;
    //            else
    //            {
    //                if (!wheel.HasGroundContact())
    //                    nbNotTouching++;
    //                else
    //                    nbTouching++;
    //            }
    //        }

    //        #endregion

    //        float motorTorque = 0.0f;
    //        if (nbTouching > 0 && (Math.Abs(_accelerationPedal) > 0.01f))
    //        {
    //            float axisTorque = _ComputeAxisTorque();
    //            float wheelTorque = axisTorque / (float)(_wheels.Count - nbHandBrake);
    //            float wheelTorqueNotTouching = nbNotTouching > 0 ? wheelTorque * (float)Math.Pow(0.5, (float)nbNotTouching) : 0;
    //            float wheelTorqueTouching = wheelTorque - wheelTorqueNotTouching;
    //            motorTorque = wheelTorqueTouching / (float)nbTouching;
    //        }
    //        else
    //            _UpdateRpms();

    //        foreach (var wheel in _wheels)
    //        {
    //            wheel.Tick(_handBrake, motorTorque, _brakePedal, elapseTime);
    //        }

    //    }

    //    public void ApplyRandomForce()
    //    {
    //        Vector3 pos = new NxVec3(Rand.Uniform(-4.0f, 4.0f), Rand.Uniform(-4.0f, 4.0f), Rand.Uniform(-4.0f, 4.0f));
    //        float force = Rand.Uniform(_bodyActor.Mass * 0.5f, _bodyActor.Mass * 2.0f);
    //        _bodyActor.AddForceAtLocalPos(new Vector3(0, force * 100.0f, 0), pos);
    //    }

    //    public void StandUp()
    //    {            
    //        NxVec3 pos = _bodyActor.GlobalPosition + new Vector3(0,2,0);
    //        Matrix rot = _bodyActor.GlobalOrientation;
    //        NxVec3 front =Vector3.TransformNormal(new NxVec3(1,0,0),rot);	        
    //        front.Y = 0;
    //        front.Normalize();

    //        float dotproduct  = front.X;

    //        float angle = Math.Sign(-front.Z) * (float)Math.Acos(dotproduct);

    //        rot = Matrix.RotationY(angle);	        

    //        _bodyActor.GlobalPosition = pos;
    //        _bodyActor.GlobalOrientation = rot;
    //        _bodyActor.LinearVelocity = new NxVec3(0,0,0);
    //        _bodyActor.AngularVelocity = new NxVec3(0,0,0);
    //    }

    //    public void SetSuspention(SpringDesc suspention)
    //    {
    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel is RayCastWheel)
    //            {
    //                WheelShape shape = ((RayCastWheel)wheel).ActorShape;
    //                shape.Suspension = suspention;
    //            }
    //        }
    //    }
     
    //    public void SetLongitudinalTireFunction(TireFunctionDesc longitudinalFunction)
    //    {
    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel is RayCastWheel)
    //            {
    //                WheelShape shape = ((RayCastWheel)wheel).ActorShape;
    //                shape.LongitudalTireForceFunction = longitudinalFunction;
    //            }
    //        }
    //    }

    //    public void SetLateralTireFunction(TireFunctionDesc lateralFunction)
    //    {
    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel is RayCastWheel)
    //            {
    //                WheelShape shape = ((RayCastWheel)wheel).ActorShape;
    //                shape.LateralTireForceFunction = lateralFunction;
    //            }
    //        }
    //    }

    //    public void SetDefaulValues()
    //    {
    //        if (_bodyActor != null)
    //            _bodyActor.Mass = 1200;
         
    //        _steeringMaxAngleRad = Euler.ToRadians(30.0f);
    //        _motorForce = 3500f;           

    //        _transmissionEfficiency = 1.0f;
    //        _differentialRatio = 1.0f;
    //        _maxVelocity = 80;
    //    }

    //    public Wheel GetWheel(string name)
    //    {
    //        foreach (var wheel in _wheels)
    //        {
    //            if (wheel.Name == name)
    //                return wheel;
    //        }

    //        return null;
    //    }

    //    #endregion


    //}
}
