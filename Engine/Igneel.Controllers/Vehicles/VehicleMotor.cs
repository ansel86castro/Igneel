using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NxReal = System.Single;

namespace Igneel.Controllers
{
    [Serializable]
    public class VehicleMotor
    {
        LinearInterpolationValues _torqueCurve;
        NxReal _rpm;
        NxReal _maxTorque;
        NxReal _maxTorquePos;
        NxReal _maxRpmToGearUp;
        NxReal _minRpmToGearDown;
        NxReal _maxRpm;
        NxReal _minRpm;
        NxReal _torque;

        public VehicleMotor()
        {
            SetToDefault();
        }

        public float Rpm
        {
            get { return _rpm; }
            set
            {
                _rpm = value;
                if (_torqueCurve != null)
                    _torque = _torqueCurve.GetValue(_rpm);
            }
        }
        
        public float MinRpm { get { return _minRpm; }  }
        
        public float MaxRpm { get { return _maxRpm; } }
        
        public float Torque { get { return _torque; } }
        
        public LinearInterpolationValues TorqueCurve
        {
            get
            {
                return _torqueCurve;
            }
            set
            {
                _torqueCurve = value;
                if (_torqueCurve != null)
                {
                    _torque = _torqueCurve.GetValue(_rpm);
                    _maxTorque = 0;
                    _maxTorquePos = -1;
                    for (int i = 0; i < _torqueCurve.Count; i++)
                    {
                        var t = _torqueCurve.GetValueAt(i);
                        if (t > _maxTorque)
                        {
                            _maxTorque = t;
                            _maxTorquePos = i;
                        }
                    }
                }
            }
        }

        public int ChangeGears(VehicleGears gears, float threshold)
        {
            int gear = gears.CurrentGear;
            if (_rpm > _maxRpmToGearUp && gear < gears.MaxGear)
                return 1;
            else if (_rpm < _minRpmToGearDown && gear > 1)
                return -1;

                    /*
	        NxReal normalTorque = _torqueCurve.getValue(_rpm);

	        NxReal lowerGearRatio = gears->getRatio(gear-1);
	        NxReal normalGearRatio = gears->getCurrentRatio();
	        NxReal upperGearRatio = gears->getRatio(gear+1);
	        NxReal lowerGearRpm = _rpm / normalGearRatio * lowerGearRatio;
	        NxReal upperGearRpm = _rpm / normalGearRatio * upperGearRatio;
	        NxReal lowerTorque = _torqueCurve.getValue(lowerGearRpm);
	        NxReal upperTorque = _torqueCurve.getValue(upperGearRpm);
	        NxReal lowerWheelTorque = lowerTorque * lowerGearRatio;
	        NxReal normalWheelTorque = normalTorque * normalGearRatio;
	        NxReal upperWheelTorque = upperTorque * upperGearRatio;
	        //printf("%2.3f %2.3f %2.3f\n", lowerWheelTorque, normalWheelTorque, upperWheelTorque);
	        */
            return 0;
        }

        public void SetToCorvette()
        {
            _torqueCurve.Insert(1000.0f, 393.0f);
            _torqueCurve.Insert(2000.0f, 434.0f);
            _torqueCurve.Insert(4000.0f, 475.0f);
            _torqueCurve.Insert(5000.0f, 475.0f);
            _torqueCurve.Insert(6000.0f, 366.0f);
            _minRpmToGearDown = 2500.0f;
            _maxRpmToGearUp = 5000.0f;
            _minRpm = 1500.0f;
            _maxRpm = 6000.0f;
        }

        public void SetToDefault()
        {           
            _minRpmToGearDown = 1500.0f;
            _maxRpmToGearUp = 4000.0f;
            _maxRpm = 5000.0f;
            _minRpm = 1000.0f;
        }
    }

    /// <summary>
    /// Transmition
    /// </summary>
    [Serializable]
    public class VehicleGears
    {
       public const int VehicleMaxNbGears = 32;
       
        float[] _forwardGearRatios;
        float _backwardGearRatio;
        int _curGear;

        public VehicleGears()
            : this(new float[0], 0)
        {

        }

        public VehicleGears(float[]forwardGearRatios , float backwardGearRatio)
        {
            _forwardGearRatios = forwardGearRatios;
            _curGear = 1;
            _backwardGearRatio = backwardGearRatio;
        }      

        public float CurrentGearRatio
        {
            get{return GetRatio(_curGear);}
        }

        public int CurrentGear { get{ return _curGear; }}

        public int MaxGear { get{ return _forwardGearRatios.Length; } }

        public float GetRatio(int gear)
        {
            if (gear > 0)
                return _forwardGearRatios[gear - 1];
            if (gear == -1)
                return _backwardGearRatio;
            return 0;
        }

        public void GearUp()
        {
            _curGear = Math.Min(_curGear + 1, _forwardGearRatios.Length);
        }

        public void GearDown()
        {
            _curGear = Math.Max(_curGear - 1, -1);
        }

        public void SetToCorvete()
        {
            _forwardGearRatios = new NxReal[6];

            _forwardGearRatios[0] = 2.66f;
            _forwardGearRatios[1] = 1.78f;
            _forwardGearRatios[2] = 1.30f;
            _forwardGearRatios[3] = 1;
            _forwardGearRatios[4] = 0.74f;
            _forwardGearRatios[5] = 0.50f;            

            _backwardGearRatio = -2.90f;
        }
    }
}
