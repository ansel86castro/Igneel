using Igneel.Assets;
using Igneel.Scenering;
using Igneel.Input;
using Igneel.Scenering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Controllers
{
   [Serializable]
    public class FPController : NodeController
    {
        public enum Behavior { Default, Orthographic }
       
        Vector3 moveDirection;
        Vector2 mouseDelta;
        int mouseWheel;
        Vector2 rotVelocity;
        Vector3 moveVelocity;
        float moveScale = 1.0f;
        float rotScale = 1.0f;
        float breakingTime = 0.25f;
        Vector3 velocityBreak;
        float currentBreakTime;
       //keys mappings
        Keys foward = Keys.W;
        Keys back = Keys.S;
        Keys left = Keys.A;
        Keys right = Keys.D;
        Keys up = Keys.Q;
        Keys down = Keys.E;
        Keys speedupMove = Keys.LSHIFT;
        Keys speedupRot = Keys.LCONTROL;
        int framesToSmoot = 2; 

        [AssetMember]
        public float MoveScale { get { return moveScale; } set { moveScale = value; } }
      
        [AssetMember]
        public float RotationScale { get { return rotScale; } set { rotScale = value; } }

        [AssetMember]
        public float BreakingTime { get { return breakingTime; } set { breakingTime = value; } }

        [AssetMember(typeof(DelegateConverter))]
        public Func<FPController, bool> UpdateCallback { get; set; }

        public override void Initialize(SceneNode node)
        {
            base.Initialize(node);
        }

        private void GetInput()
        {
            if (Engine.KeyBoard.IsKeyPressed(foward))
               moveDirection.Z =+ 1;
            if (Engine.KeyBoard.IsKeyPressed(back))
                moveDirection.Z -= 1;
            if (Engine.KeyBoard.IsKeyPressed(left))
                moveDirection.X -= 1;
            if (Engine.KeyBoard.IsKeyPressed(right))
                moveDirection.X += 1;
            if (Engine.KeyBoard.IsKeyPressed(up))
                moveDirection.Y += 1;
            if (Engine.KeyBoard.IsKeyPressed(down))
                moveDirection.Y -= 1;
            if (Engine.KeyBoard.IsKeyPressed(speedupMove))
                moveScale = Math.Max(0 , moveScale + 1);
            if (Engine.KeyBoard.IsKeyPressed(speedupRot))
                moveScale = Math.Max(0, moveScale - 1);
        }

        private void UpdateVelocity(float elapsedTime)
        {
            rotVelocity = mouseDelta * rotScale;
            var acc = Vector3.Normalize(moveDirection) * moveScale;

            if (acc.LengthSquared() > 0)
            {
                moveVelocity = acc;
                currentBreakTime = breakingTime;
                velocityBreak = acc / currentBreakTime;
            }
            else
            {
                if (currentBreakTime > 0)
                {
                    moveVelocity -= velocityBreak * elapsedTime;
                    currentBreakTime -= elapsedTime;
                }
                else
                {
                    moveVelocity = new Vector3(0);
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
            mouseDelta = new Vector2();
            moveDirection = new Vector3();
            if (UpdateCallback != null && UpdateCallback(this))
            {
                GetInput();
                var newDelta = new Vector2(Engine.Mouse.X, Engine.Mouse.Y);                
                mouseWheel = Engine.Mouse.Z;

                float lerpValue = 1f / (float)framesToSmoot;
                mouseDelta = mouseDelta * (1 - lerpValue) + newDelta * lerpValue;
            }

            UpdateVelocity(elapsedTime);

            Node.Heading += Numerics.ToRadians(rotVelocity.X);
            Node.Pitch += Numerics.ToRadians(rotVelocity.Y);
            
            var posDelta = moveVelocity * elapsedTime;
            Node.LocalPosition += Vector3.TransformNormal(posDelta, Node.LocalRotation);
            Node.UpdateLocalPose();
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
    }
}
