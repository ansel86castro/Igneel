using Igneel;
using Igneel.Animations;
using Igneel.Scenering;
using Igneel.IA;
using Igneel.Input;
using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Scenering.Animations;

namespace D3D9Testing
{
    public struct ThirdPersonControllerDesc
    {
        public SceneNode Character;
        public CharacterController CollitionController;
        public AnimationPlayback Idle;
        public AnimationPlayback Walk;                
        public float TransitionTime;      
        public float CameraMaxDistance;
        public float CameraMinDistance;
        public float CameraMaxPich;
        public float CameraMinPich;
        public SceneNode Camera;
        
    }

    public class ThirdPersonController : Automata
    {
        const int collitionMask = (1 << (int)CollisionGroup.GROUP_NON_COLLIDABLE) | 
                                  (1 << (int)CollisionGroup.GROUP_COLLIDABLE_NON_PUSHABLE);
        const string IDLE = "idle";
        const string WALK= "walk";
        const string WALK_AROUND = "walk_around";      
        const string RUN = "run";
        const string STRADE_RIGHT = "r_strade";
        const string STRADE_LEFT = "l_strade";
        const string JUMP = "jump";
        const string TURN_RIGHT = "turn_right";
        const string TURN_LEFT = "turn_left";
        const string TURN_BACK = "turn_back";        

        CharacterController collitionController;
        SceneNode character;
        BonesResetter resetter;
        SceneNode camera;

        AnimationPlayback walk;
        AnimationPlayback idle;
        AnimationTransition idle_walk;
        AnimationTransition walk_idle;       
      
        private Vector3 lastAnimPosition;       
        private Vector3 initialForwardPositon;
        private float initialHeading;
        private float heading;        
        private Vector3 displacement;
        private Vector3 walkForwardSpeed;       
        private float turn90Speed;
        private float turn180Speed;
        private float cameraMaxDistance;
        private float cameraMinDistance;
        private float cameraMaxPich;
        private float cameraMinPich;
        private bool cameraBindedToCharacter;
        private float turnHeading;
        private bool update;        

        public ThirdPersonController(ThirdPersonControllerDesc desc)
        {
            character = desc.Character;
            collitionController = desc.CollitionController;
            walk = desc.Walk;         
            idle = desc.Idle;

            turn90Speed = Numerics.PI / 2 / desc.TransitionTime;
            turn180Speed = Numerics.PI / desc.TransitionTime;

            cameraMaxDistance = desc.CameraMaxDistance;
            cameraMinDistance = desc.CameraMinDistance;
            cameraMaxPich = desc.CameraMaxPich;
            cameraMinPich = desc.CameraMinPich;
            camera = desc.Camera;          

            resetter = new BonesResetter(character);

            idle_walk = new AnimationTransition(idle, walk, desc.TransitionTime);
            walk_idle = new AnimationTransition(walk, idle, desc.TransitionTime);            

            walk.FirstPlayback.Animation.Sample(walk.FirstPlayback.Cursor.StartTime);
            initialForwardPositon = character.LocalPosition;                

            walk.FirstPlayback.Animation.Sample(walk.FirstPlayback.Cursor.StartTime + desc.TransitionTime);
            lastAnimPosition = character.LocalPosition;
            walkForwardSpeed = character.LocalPosition - initialForwardPositon;

            resetter.Reset();           
            initialHeading = character.Heading;                     
            
            CreateStates();

            cameraBindedToCharacter = camera.Affector == character;
        }

        public CharacterController CollitionController
        {
            get { return collitionController; }
            set { collitionController = value; }
        }

        public SceneNode Camera { get { return camera; } }      

        private string T(string current, string next)
        {
            return current + "->" + next;
        }    

        private void CreateStates()
        {
            #region States

            AddState(IDLE, Idle);
            AddState(T(IDLE, WALK), Idle_Walk);
            AddState(WALK, Walk);
            AddState(T(WALK, IDLE), Walk_Idle);
            AddState(TURN_RIGHT, TurnRight);
            AddState(TURN_LEFT, TurnLeft);
            AddState(TURN_BACK, TurnBack);       

            #endregion
           
            #region Transitions

            AddTransition(IDLE, T(IDLE, WALK), x => Engine.KeyBoard.IsKeyPressed(Keys.UPARROW));
            AddTransition(T(IDLE, WALK), WALK, x => idle_walk.TransitionComplete);
            AddTransition(WALK, T(WALK, IDLE), x => !Engine.KeyBoard.IsKeyPressed(Keys.UPARROW));
            AddTransition(T(WALK, IDLE), IDLE, x => { if (walk_idle.TransitionComplete) { idle_walk.Reset(); return true; } return false; });

            AddTransition(IDLE, TURN_RIGHT, x => Engine.KeyBoard.IsKeyPressed(Keys.RIGHTARROW) && StartTurning());
            AddTransition(TURN_RIGHT, IDLE, x => !Engine.KeyBoard.IsKeyPressed(Keys.RIGHTARROW) && FromTurningToIdle());
            AddTransition(IDLE, TURN_LEFT, x => Engine.KeyBoard.IsKeyPressed(Keys.LEFTARROW) && StartTurning());
            AddTransition(TURN_LEFT, IDLE, x => !Engine.KeyBoard.IsKeyPressed(Keys.LEFTARROW) && FromTurningToIdle());
            AddTransition(IDLE, TURN_BACK, x => Engine.KeyBoard.IsKeyPressed(Keys.DOWNARROW) && StartTurning());
            AddTransition(TURN_BACK, IDLE, x => !Engine.KeyBoard.IsKeyPressed(Keys.DOWNARROW) && turnHeading == 0 && FromTurningToIdle());

            //AddTransition(TURN_RIGHT, WALK, x => Engine.KeyBoard.IsKeyPressed(Keys.RIGHTARROW) && turnHeading == 0);
            //AddTransition(TURN_LEFT, WALK, x => Engine.KeyBoard.IsKeyPressed(Keys.LEFTARROW) && turnHeading == 0);
            AddTransition(TURN_BACK, WALK, x => Engine.KeyBoard.IsKeyPressed(Keys.DOWNARROW) && turnHeading == 0);

            #endregion
        }

       
        public override void Update(float elapsedTime)
        {
            //store position previus animation
            var localPosition = character.LocalPosition;
            //reset displacement
            displacement = new Vector3();            

            //reset transforms for blending
            resetter.Reset();

            //update states
            base.Update(elapsedTime);

            //set character orientation character
            character.Heading = initialHeading + heading;

            //clear the animation position
            character.Tx = localPosition.X;
            character.Tz = localPosition.Z;

            if (collitionController != null)
            {
                //apply displacement only on x,z 
                Vector3 disp = displacement;
                disp.Y = 0;

                //the moving of the character associated is defered until the physics scene updates 
                //its state
                collitionController.Move(disp, collitionMask, 0.000001f, 1.0f);
            }
            else
            {
                character.Tx += displacement.X;
                character.Tz += displacement.Z;

                //camera.Tx += displacement.X;
                //camera.Tz += displacement.Z;
                //camera.CommitChanges();
            }           

            character.UpdateLocalPose();
            character.CommitChanges();
        }

        private void UpdateRotation(float elapsedTime)
        {
            float rotSpeed = Numerics.ToRadians(90f);

            if (Engine.KeyBoard.IsKeyPressed(Keys.LEFTARROW))
                heading -= rotSpeed * elapsedTime;
            else if (Engine.KeyBoard.IsKeyPressed(Keys.RIGHTARROW))
                heading += rotSpeed * elapsedTime;
        }

        #region States Updates       

        private void Idle(float deltaT)
        {
            idle.Update(deltaT);        
        }      

        private void TurnBack(float deltaT)
        {
            idle_walk.Update(deltaT);

            turnHeading += turn180Speed * deltaT;
            heading += turn180Speed * deltaT;
            if (turnHeading >= Numerics.PI)
            {
                EndTurning();
            }
        }       

        private void TurnLeft(float deltaT)
        {
            idle_walk.Update(deltaT);

            turnHeading -= turn90Speed * deltaT;
            heading -= turn90Speed * deltaT;

            if (turnHeading <= -Numerics.PIover2)
            {
                EndTurning();
            }
        }

        private void TurnRight(float deltaT)
        {
            idle_walk.Update(deltaT);

            turnHeading += turn90Speed * deltaT;
            heading += turn90Speed * deltaT;

            if (turnHeading >= Numerics.PIover2)
            {
                EndTurning();
            }
        }

        private void Walk(float elapsedTime)
        {
            walk.Update(elapsedTime);
            UpdateRotation(elapsedTime);

            if (!update)
            {
                lastAnimPosition = character.LocalPosition;
                update = true;
                return;
            }

            var cursor = walk.FirstPlayback.Cursor;          
            if (!cursor.TimeRestart)
                displacement = character.LocalPosition - lastAnimPosition;
            else
                displacement = character.LocalPosition - initialForwardPositon;

            displacement = Vector3.TransformCoordinates(displacement, Matrix.RotationY(heading));
            displacement = WalkAroundCamera(displacement);
            
            lastAnimPosition = character.LocalPosition;
        }

        private Vector3 WalkAroundCamera(Vector3 disp)
        {
            //float walkAroundCamera = Engine.KeyBoard.IsKeyPressed(Keys.RIGHTARROW) ? 1 :
            //                     Engine.KeyBoard.IsKeyPressed(Keys.LEFTARROW) ? -1 : 0;

            //if (walkAroundCamera != 0)
            //{
            //    var p0 = character.LocalPosition - camera.GlobalPosition;
            //    var p1 = p0 + disp;
            //    p1.Normalize();
            //    float angle = (float)Math.Acos(Vector3.Dot(Vector3.Normalize(p0), p1));
            //    p1 *= p0.Length();
            //    heading += angle * walkAroundCamera;
            //    disp = p1 - p0;

            //    camera.Heading += angle * walkAroundCamera;
            //}
            //else
            //{
            //    camera.Tx += disp.X;
            //    camera.Tz += disp.Z;
            //    camera.CommitChanges();
            //}
            return disp;
        }

        private void Idle_Walk(float elapsed)
        {           
            idle_walk.Update(elapsed);
            UpdateRotation(elapsed);

            update = false;
            displacement = Vector3.TransformCoordinates(walkForwardSpeed * elapsed, Matrix.RotationY(heading));
            displacement = WalkAroundCamera(displacement);         
        }

        private void Walk_Idle(float elapsed)
        {
            walk_idle.Update(elapsed);
            UpdateRotation(elapsed);

            displacement= Vector3.TransformCoordinates(walkForwardSpeed * elapsed, Matrix.RotationY(heading));
            displacement = WalkAroundCamera(displacement);
        }

        #endregion

        private bool StartTurning()
        {
            turnHeading = 0;

            if (cameraBindedToCharacter)
            {
                camera.UnBindFrom(character);
                camera.LocalPose = camera.GlobalPose;
                camera.CommitChanges();
            }

            return true;
        }

        private void EndTurning()
        {
            turnHeading = 0;
            idle_walk.Reset();          
        }

        private bool FromTurningToIdle()
        {
            if (cameraBindedToCharacter)
            {
                camera.BindTo(character);
            }
            return true;
        }

        private bool StartWalkAround(float value)
        {
            if (turnHeading == 0)
            {
                return true;
            }
            return false;
        }
    }
}
