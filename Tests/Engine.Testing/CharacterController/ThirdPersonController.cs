using Igneel;
using Igneel.Animations;
using Igneel.IA;
using Igneel.Input;
using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;
using Igneel.Animations;
using Igneel.SceneManagement;

namespace D3D9Testing
{
    public struct ThirdPersonControllerDesc
    {
        public Frame Character;
        public CharacterController CollitionController;
        public KeyFrameAnimationPlayback Idle;
        public KeyFrameAnimationPlayback Walk;                
        public float TransitionTime;      
        public float CameraMaxDistance;
        public float CameraMinDistance;
        public float CameraMaxPich;
        public float CameraMinPich;
        public Frame Camera;
        
    }

    public class ThirdPersonController : Automata
    {
        const int CollitionMask = (1 << (int)CollisionGroup.GroupNonCollidable) | 
                                  (1 << (int)CollisionGroup.GroupCollidableNonPushable);
        const string IDLE = "idle";
        const string WALK= "walk";
        const string WalkAround = "walk_around";      
        const string Run = "run";
        const string StradeRight = "r_strade";
        const string StradeLeft = "l_strade";
        const string Jump = "jump";
        const string TURN_RIGHT = "turn_right";
        const string TURN_LEFT = "turn_left";
        const string TURN_BACK = "turn_back";        

        CharacterController _collitionController;
        Frame _character;
        BonesResetter _resetter;
        Frame _camera;

        KeyFrameAnimationPlayback _walk;
        KeyFrameAnimationPlayback _idle;
        AnimationTransition _idleWalk;
        AnimationTransition _walkIdle;       
      
        private Vector3 _lastAnimPosition;       
        private Vector3 _initialForwardPositon;
        private float _initialHeading;
        private float _heading;        
        private Vector3 _displacement;
        private Vector3 _walkForwardSpeed;       
        private float _turn90Speed;
        private float _turn180Speed;
        private float _cameraMaxDistance;
        private float _cameraMinDistance;
        private float _cameraMaxPich;
        private float _cameraMinPich;
        private bool _cameraBindedToCharacter;
        private float _turnHeading;
        private bool _update;        

        public ThirdPersonController(ThirdPersonControllerDesc desc)
        {
            _character = desc.Character;
            _collitionController = desc.CollitionController;
            _walk = desc.Walk;         
            _idle = desc.Idle;

            _turn90Speed = Numerics.PI / 2 / desc.TransitionTime;
            _turn180Speed = Numerics.PI / desc.TransitionTime;

            _cameraMaxDistance = desc.CameraMaxDistance;
            _cameraMinDistance = desc.CameraMinDistance;
            _cameraMaxPich = desc.CameraMaxPich;
            _cameraMinPich = desc.CameraMinPich;
            _camera = desc.Camera;          

            _resetter = new BonesResetter(_character);

            _idleWalk = new AnimationTransition(_idle, _walk, desc.TransitionTime);
            _walkIdle = new AnimationTransition(_walk, _idle, desc.TransitionTime);            

            _walk.FirstPlayback.Animation.Sample(_walk.FirstPlayback.Cursor.StartTime);
            _initialForwardPositon = _character.LocalPosition;                

            _walk.FirstPlayback.Animation.Sample(_walk.FirstPlayback.Cursor.StartTime + desc.TransitionTime);
            _lastAnimPosition = _character.LocalPosition;
            _walkForwardSpeed = _character.LocalPosition - _initialForwardPositon;

            _resetter.Reset();           
            _initialHeading = _character.Heading;                     
            
            CreateStates();

            _cameraBindedToCharacter = _camera.Affector == _character;
        }

        public CharacterController CollitionController
        {
            get { return _collitionController; }
            set { _collitionController = value; }
        }

        public Frame Camera { get { return _camera; } }      

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

            AddTransition(IDLE, T(IDLE, WALK), x => Engine.KeyBoard.IsKeyPressed(Keys.Uparrow));
            AddTransition(T(IDLE, WALK), WALK, x => _idleWalk.TransitionComplete);
            AddTransition(WALK, T(WALK, IDLE), x => !Engine.KeyBoard.IsKeyPressed(Keys.Uparrow));
            AddTransition(T(WALK, IDLE), IDLE, x => { if (_walkIdle.TransitionComplete) { _idleWalk.Reset(); return true; } return false; });

            AddTransition(IDLE, TURN_RIGHT, x => Engine.KeyBoard.IsKeyPressed(Keys.Rightarrow) && StartTurning());
            AddTransition(TURN_RIGHT, IDLE, x => !Engine.KeyBoard.IsKeyPressed(Keys.Rightarrow) && FromTurningToIdle());
            AddTransition(IDLE, TURN_LEFT, x => Engine.KeyBoard.IsKeyPressed(Keys.Leftarrow) && StartTurning());
            AddTransition(TURN_LEFT, IDLE, x => !Engine.KeyBoard.IsKeyPressed(Keys.Leftarrow) && FromTurningToIdle());
            AddTransition(IDLE, TURN_BACK, x => Engine.KeyBoard.IsKeyPressed(Keys.Downarrow) && StartTurning());
            AddTransition(TURN_BACK, IDLE, x => !Engine.KeyBoard.IsKeyPressed(Keys.Downarrow) && _turnHeading == 0 && FromTurningToIdle());

            //AddTransition(TURN_RIGHT, WALK, x => Engine.KeyBoard.IsKeyPressed(Keys.RIGHTARROW) && turnHeading == 0);
            //AddTransition(TURN_LEFT, WALK, x => Engine.KeyBoard.IsKeyPressed(Keys.LEFTARROW) && turnHeading == 0);
            AddTransition(TURN_BACK, WALK, x => Engine.KeyBoard.IsKeyPressed(Keys.Downarrow) && _turnHeading == 0);

            #endregion
        }

       
        public override void Update(float elapsedTime)
        {
            //store position previus animation
            var localPosition = _character.LocalPosition;
            //reset displacement
            _displacement = new Vector3();            

            //reset transforms for blending
            _resetter.Reset();

            //update states
            base.Update(elapsedTime);

            //set character orientation character
            _character.Heading = _initialHeading + _heading;

            //clear the animation position
            _character.X = localPosition.X;
            _character.Z = localPosition.Z;

            if (_collitionController != null)
            {
                //apply displacement only on x,z 
                Vector3 disp = _displacement;
                disp.Y = 0;

                //the moving of the character associated is defered until the physics scene updates 
                //its state
                _collitionController.Move(disp, CollitionMask, 0.000001f, 1.0f);
            }
            else
            {
                _character.X += _displacement.X;
                _character.Z += _displacement.Z;

                //camera.Tx += displacement.X;
                //camera.Tz += displacement.Z;
                //camera.CommitChanges();
            }           

            _character.ComputeLocalPose();
            _character.CommitChanges();
        }

        private void UpdateRotation(float elapsedTime)
        {
            float rotSpeed = Numerics.ToRadians(90f);

            if (Engine.KeyBoard.IsKeyPressed(Keys.Leftarrow))
                _heading -= rotSpeed * elapsedTime;
            else if (Engine.KeyBoard.IsKeyPressed(Keys.Rightarrow))
                _heading += rotSpeed * elapsedTime;
        }

        #region States Updates       

        private void Idle(float deltaT)
        {
            _idle.Update(deltaT);        
        }      

        private void TurnBack(float deltaT)
        {
            _idleWalk.Update(deltaT);

            _turnHeading += _turn180Speed * deltaT;
            _heading += _turn180Speed * deltaT;
            if (_turnHeading >= Numerics.PI)
            {
                EndTurning();
            }
        }       

        private void TurnLeft(float deltaT)
        {
            _idleWalk.Update(deltaT);

            _turnHeading -= _turn90Speed * deltaT;
            _heading -= _turn90Speed * deltaT;

            if (_turnHeading <= -Numerics.PIover2)
            {
                EndTurning();
            }
        }

        private void TurnRight(float deltaT)
        {
            _idleWalk.Update(deltaT);

            _turnHeading += _turn90Speed * deltaT;
            _heading += _turn90Speed * deltaT;

            if (_turnHeading >= Numerics.PIover2)
            {
                EndTurning();
            }
        }

        private void Walk(float elapsedTime)
        {
            _walk.Update(elapsedTime);
            UpdateRotation(elapsedTime);

            if (!_update)
            {
                _lastAnimPosition = _character.LocalPosition;
                _update = true;
                return;
            }

            var cursor = _walk.FirstPlayback.Cursor;          
            if (!cursor.TimeRestart)
                _displacement = _character.LocalPosition - _lastAnimPosition;
            else
                _displacement = _character.LocalPosition - _initialForwardPositon;

            _displacement = Vector3.TransformCoordinates(_displacement, Matrix.RotationY(_heading));
            _displacement = WalkAroundCamera(_displacement);
            
            _lastAnimPosition = _character.LocalPosition;
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
            _idleWalk.Update(elapsed);
            UpdateRotation(elapsed);

            _update = false;
            _displacement = Vector3.TransformCoordinates(_walkForwardSpeed * elapsed, Matrix.RotationY(_heading));
            _displacement = WalkAroundCamera(_displacement);         
        }

        private void Walk_Idle(float elapsed)
        {
            _walkIdle.Update(elapsed);
            UpdateRotation(elapsed);

            _displacement= Vector3.TransformCoordinates(_walkForwardSpeed * elapsed, Matrix.RotationY(_heading));
            _displacement = WalkAroundCamera(_displacement);
        }

        #endregion

        private bool StartTurning()
        {
            _turnHeading = 0;

            if (_cameraBindedToCharacter)
            {
                _camera.UnBindFrom(_character);
                _camera.LocalPose = _camera.GlobalPose;
                _camera.CommitChanges();
            }

            return true;
        }

        private void EndTurning()
        {
            _turnHeading = 0;
            _idleWalk.Reset();          
        }

        private bool FromTurningToIdle()
        {
            if (_cameraBindedToCharacter)
            {
                _camera.BindTo(_character);
            }
            return true;
        }

        private bool StartWalkAround(float value)
        {
            if (_turnHeading == 0)
            {
                return true;
            }
            return false;
        }
    }
}
