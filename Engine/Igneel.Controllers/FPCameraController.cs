using Igneel.Assets;
using Igneel.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;
using Igneel.Assets.StorageConverters;
using Igneel.SceneManagement;

namespace Igneel.Controllers
{
   [Serializable]
    public class FpController : NodeController
    {
        public enum Behavior { Default, Orthographic }
       
        Vector3 _moveDirection;
        Vector2 _mouseDelta;
        int _mouseWheel;
        Vector2 _rotVelocity;
        Vector3 _moveVelocity;
        float _moveScale = 1.0f;
        float _rotScale = 1.0f;
        float _breakingTime = 0.25f;
        Vector3 _velocityBreak;
        float _currentBreakTime;
       //keys mappings
        Keys _foward = Keys.W;
        Keys _back = Keys.S;
        Keys _left = Keys.A;
        Keys _right = Keys.D;
        Keys _up = Keys.Q;
        Keys _down = Keys.E;
        Keys _speedupMove = Keys.Lshift;
        Keys _speedupRot = Keys.Lcontrol;
        int _framesToSmoot = 2; 

        [AssetMember]
        public float MoveScale { get { return _moveScale; } set { _moveScale = value; } }
      
        [AssetMember]
        public float RotationScale { get { return _rotScale; } set { _rotScale = value; } }

        [AssetMember]
        public float BreakingTime { get { return _breakingTime; } set { _breakingTime = value; } }

       
        public Func<FpController, bool> UpdateCallback { get; set; }

        public override void Initialize(Frame node)
        {
            base.Initialize(node);
        }

        private void GetInput()
        {
            if (Engine.KeyBoard.IsKeyPressed(_foward))
               _moveDirection.Z =+ 1;
            if (Engine.KeyBoard.IsKeyPressed(_back))
                _moveDirection.Z -= 1;
            if (Engine.KeyBoard.IsKeyPressed(_left))
                _moveDirection.X -= 1;
            if (Engine.KeyBoard.IsKeyPressed(_right))
                _moveDirection.X += 1;
            if (Engine.KeyBoard.IsKeyPressed(_up))
                _moveDirection.Y += 1;
            if (Engine.KeyBoard.IsKeyPressed(_down))
                _moveDirection.Y -= 1;
            if (Engine.KeyBoard.IsKeyPressed(_speedupMove))
                _moveScale = Math.Max(0 , _moveScale + 1);
            if (Engine.KeyBoard.IsKeyPressed(_speedupRot))
                _moveScale = Math.Max(0, _moveScale - 1);
        }

        private void UpdateVelocity(float elapsedTime)
        {
            _rotVelocity = _mouseDelta * _rotScale;
            var acc = Vector3.Normalize(_moveDirection) * _moveScale;

            if (acc.LengthSquared() > 0)
            {
                _moveVelocity = acc;
                _currentBreakTime = _breakingTime;
                _velocityBreak = acc / _currentBreakTime;
            }
            else
            {
                if (_currentBreakTime > 0)
                {
                    _moveVelocity -= _velocityBreak * elapsedTime;
                    _currentBreakTime -= elapsedTime;
                }
                else
                {
                    _moveVelocity = new Vector3(0);
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="elapsedTime">Time pased after the last frame rendered</param>
        ///// <param name="direction">direcion 1 forward , -1 backward , 0 breacking</param>
        //public void Advance(float elapsedTime, int direction)
        //{
        //    //acumulateLinearAcc += linearAceleration * elapsedTime;
        //    currentLinearVelocity += linearAceleration * elapsedTime;

        //    if (currentLinearVelocity > linearVelocity)
        //        currentLinearVelocity = linearVelocity;
        //    if (currentLinearVelocity < -linearVelocity)
        //        currentLinearVelocity = -linearVelocity;

        //    if (direction == 0)
        //        currentLinearVelocity *= breakingFactor;

        //    if (currentLinearVelocity < Numerics.Epsilon && currentLinearVelocity > -Numerics.Epsilon)
        //        currentLinearVelocity = 0;

        //    float delta = currentLinearVelocity * elapsedTime * direction;
        //    Node.LocalPosition += delta * front;
        //}

        //public void Strafe(float elapsedTime, int direction)
        //{
        //    currentLinearVelocity += linearAceleration * elapsedTime;
        //    if (currentLinearVelocity > linearVelocity) currentLinearVelocity = linearVelocity;
        //    if (currentLinearVelocity < -linearVelocity) currentLinearVelocity = -linearVelocity;

        //    if (direction == 0) currentLinearVelocity *= breakingFactor;

        //    if (currentLinearVelocity < Numerics.Epsilon && currentLinearVelocity > -Numerics.Epsilon) currentLinearVelocity = 0;

        //    float delta = currentLinearVelocity * elapsedTime * direction;
        //    Node.LocalPosition += delta * right;
        //}

        //public void Elevate(float elapsedTime, int direction)
        //{
        //    currentLinearVelocity += linearAceleration * elapsedTime;
        //    if (currentLinearVelocity > linearVelocity) currentLinearVelocity = linearVelocity;
        //    if (currentLinearVelocity < -linearVelocity) currentLinearVelocity = -linearVelocity;

        //    if (direction == 0) currentLinearVelocity *= breakingFactor;

        //    if (currentLinearVelocity < Numerics.Epsilon && currentLinearVelocity > -Numerics.Epsilon) currentLinearVelocity = 0;

        //    float delta = currentLinearVelocity * elapsedTime * direction;

        //    Node.LocalPosition += delta * up;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="elapsedTime"></param>
        ///// <param name="direction"></param>
        //public void UpdateHeading(float elapsedTime, float direction)
        //{
        //    if (angularAceleration == 0)
        //        currentAngularVelocityH = angularVelocity;
        //    else
        //        currentAngularVelocityH += angularAceleration * elapsedTime;

        //    if (currentAngularVelocityH > angularVelocity) currentAngularVelocityH = angularVelocity;
        //    if (currentAngularVelocityH < -angularVelocity) currentAngularVelocityH = -angularVelocity;

        //    if (direction == 0) currentAngularVelocityH *= breakingFactor;

        //    if (currentAngularVelocityH < angularEpsilon && currentAngularVelocityH > -angularEpsilon) currentAngularVelocityH = 0;

        //    var heading = Node.Heading;

        //    heading += currentAngularVelocityH * 1f/60f * direction;            

        //    //if (heading > Numerics.TwoPI)
        //    //    heading -= Numerics.TwoPI;
        //    //if (heading < 0.0f)
        //    //    heading += Numerics.TwoPI;

        //    Node.Heading = heading;

        //    //float b = (float)Math.Cos(orientation.Pitch);

        //    //front = new Vector3(b * (float)Math.Sin(orientation.Heading),
        //    // (float)Math.Sin(orientation.Pitch),
        //    // b * (float)Math.Cos(orientation.Heading));

        //    //right = new Vector3((float)Math.Sin(orientation.Heading + Numerics.PIover2),
        //    //   (float)Math.Sin(orientation.Roll),
        //    //  (float)Math.Cos(orientation.Heading + Numerics.PIover2));

        //    //up = Vector3.Cross(front, right);
        //    //up.Normalize();

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="elapsedTime"></param>
        ///// <param name="direction"></param>
        //public void UpdatePitch(float elapsedTime, float direction)
        //{
        //    if (angularAceleration == 0)
        //        currentAngularVelocityP = angularVelocity;
        //    else
        //        currentAngularVelocityP += angularAceleration * elapsedTime;

        //    if (currentAngularVelocityP > angularVelocity) currentAngularVelocityP = angularVelocity;
        //    if (currentAngularVelocityP < -angularVelocity) currentAngularVelocityP = -angularVelocity;

        //    if (direction == 0) currentAngularVelocityP *= breakingFactor;

        //    if (currentAngularVelocityP < angularEpsilon && currentAngularVelocityP > -angularEpsilon) currentAngularVelocityP = 0;

        //    var pitch = Node.Pitch;
        //    pitch += currentAngularVelocityP * 1/60f * direction;                                

        //    Node.Pitch = pitch;

        //    //float b = (float)Math.Cos(orientation.Pitch);

        //    //front = new Vector3(b * (float)Math.Sin(orientation.Heading), (float)Math.Sin(orientation.Pitch),
        //    //                    b * (float)Math.Cos(orientation.Heading));         

        //    //right = new Vector3((float)Math.Sin(orientation.Heading + Numerics.PIover2),
        //    //   (float)Math.Sin(orientation.Roll),
        //    //   (float)Math.Cos(orientation.Heading + Numerics.PIover2));

        //    //up = Vector3.Cross(front, right);
        //    //up.Normalize();
        //}

        public override void Update(float elapsedTime)
        {
            _mouseDelta = new Vector2();
            _moveDirection = new Vector3();
            if (UpdateCallback != null && UpdateCallback(this))
            {
                GetInput();
                var newDelta = new Vector2(Engine.Mouse.X, Engine.Mouse.Y);                
                _mouseWheel = Engine.Mouse.Z;

                float lerpValue = 1f / (float)_framesToSmoot;
                _mouseDelta = _mouseDelta * (1 - lerpValue) + newDelta * lerpValue;
            }

            UpdateVelocity(elapsedTime);
            

            Node.Heading += Numerics.ToRadians(_rotVelocity.X);            
            Node.Pitch += Numerics.ToRadians(_rotVelocity.Y);
            
            var posDelta = _moveVelocity * elapsedTime;
            Node.LocalPosition += Vector3.TransformNormal(posDelta, Node.LocalRotation);
            Node.ComputeLocalPose();
            Node.CommitChanges();

            //if (UpdateCallback != null && UpdateCallback(this))
            //{
            //    var rot = Node.GlobalPose;
            //    right = Vector3.Normalize(new Vector3(rot.M11, rot.M12, rot.M13));
            //    up = Vector3.Normalize(new Vector3(rot.M21, rot.M22, rot.M23));
            //    front = Vector3.Normalize(new Vector3(rot.M31, rot.M32, rot.M33));

            //    switch (behavior)
            //    {
            //        case Behavior.Default:
            //            PerformDefaultBehavior(elapsedTime);
            //            break;
            //        case Behavior.Orthographic:
            //            PerformOrthographicBehavior(elapsedTime);
            //            break;
            //    }

            //    Node.UpdateLocalPose();
            //    Node.CommitChanges();
            //}
        }

        //public void PerformDefaultBehavior(float elapsedTime)
        //{
        //    int deltaX = Engine.Mouse.X;
        //    int deltaY = Engine.Mouse.Y;
        //    int whell = Engine.Mouse.Z;

        //    if (Engine.KeyBoard.IsKeyPressed(Keys.LCONTROL) && whell > 0)
        //        linearVelocity+= whell;

        //    if (Engine.KeyBoard.IsKeyPressed(Keys.LCONTROL) && whell < 0)
        //        linearVelocity = Math.Max(0.0f, linearVelocity - whell);
          
        //    if (Engine.KeyBoard.IsKeyPressed(Keys.LCONTROL))
        //    {
        //        whell = -deltaY;
        //        deltaY = 0;
        //        deltaX = 0;
        //    }

        //    UpdateHeading(elapsedTime, deltaX);
        //    UpdatePitch(elapsedTime, deltaY);
        //    Advance(elapsedTime, whell);

        //    if (Engine.KeyBoard.IsKeyPressed(Keys.W))
        //        Advance(elapsedTime, 1);
        //    if (Engine.KeyBoard.IsKeyPressed(Keys.S))
        //        Advance(elapsedTime, -1);
        //    if (Engine.KeyBoard.IsKeyPressed(Keys.A))
        //        Strafe(elapsedTime, -1);
        //    if (Engine.KeyBoard.IsKeyPressed(Keys.D))
        //        Strafe(elapsedTime, 1);
        //    if (Engine.KeyBoard.IsKeyPressed(Keys.Q))
        //        Elevate(elapsedTime, 1);
        //    if (Engine.KeyBoard.IsKeyPressed(Keys.E))
        //        Elevate(elapsedTime, -1);

        //    if (Engine.KeyBoard.IsKeyPressed(Keys.SPACE))
        //        Elevate(elapsedTime, whell);
        //}

        //public void PerformOrthographicBehavior(float elapsedTime)
        //{          
        //    if (Engine.KeyBoard.IsKeyPressed(Keys.LSHIFT))
        //        linearVelocity += linearAceleration * elapsedTime;

        //    if (Engine.KeyBoard.IsKeyPressed(Keys.LCONTROL))
        //        linearVelocity = Math.Max(0.0f, linearVelocity - linearAceleration * elapsedTime);

        //    int deltaX = Engine.Mouse.X;
        //    int deltaY = Engine.Mouse.Y;
        //    int whell = Math.Sign(Engine.Mouse.Z);

        //    if (Engine.KeyBoard.IsKeyPressed(Keys.LALT))
        //    {
        //        whell = -deltaY;
        //        deltaY = 0;
        //        deltaX = 0;
        //    }

        //    var camera = (Camera)Node.NodeObject;
        //    camera.OrthoWidth = Math.Max(camera.OrthoWidth + whell, 0);
        //    camera.OrthoHeight = Math.Max(camera.OrthoHeight + whell, 0);

        //    Advance(elapsedTime, whell);
        //    Strafe(elapsedTime, deltaX);
        //    Elevate(elapsedTime, deltaY);
        //}       

        protected override void OnDispose(bool disposing)
        {
           
        }
    }
}
