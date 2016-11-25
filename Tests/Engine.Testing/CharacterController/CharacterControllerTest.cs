using Igneel;
using Igneel.Importers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Physics;
using Igneel.Input;
using Igneel.Animations;
using Igneel;
using Igneel.Components;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.Utilities;

namespace D3D9Testing
{
    [Test]
    public class CharacterControllerTest
    {
        private Mesh _box;
        private RasterizerState _rastState;
        Camera _targetCamera;
        [TestMethod]
        public void CreateControllerFromFile()
        {
            SceneTests.InitializeScene();
          
            SceneManager.Scene.Physics.Enable = true;
            SceneManager.Scene.Physics.Visible = true;

            CharacterControllerTagProcessor.ControllerCreated += CharacterControllerTagProcessor_ControllerCreated;
            ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\controller.DAE");
            CharacterController controller = CharacterControllerManager.Instance.Controllers[0];

            SceneManager.Scene.Dynamics.Add(new Dynamic( deltaT =>
                {
                    var camera = SceneManager.Scene.ActiveCamera;                                    
                    Vector3 direction = new Vector3();
                    if (Engine.KeyBoard.IsKeyPressed(Keys.O))
                        direction = camera.Front;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.L))
                        direction = -camera.Front;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.K))
                        direction = -camera.Right;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.Semicolon))
                        direction = camera.Right;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.P))
                        direction = camera.Up;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.I))
                        direction = -camera.Up;

                    CollitionResult result = controller.Move(direction * 100 * deltaT, (1 << (int)CollisionGroup.GroupNonCollidable)|
                                                                          (1 << (int)CollisionGroup.GroupCollidableNonPushable) , 0.000001f, 1.0f);
                }));
        }

        [TestMethod]
        public void CreateThirdPersonController()
        {
            const float startTimeWalk = 34f / 30f;
            const float endTimeWalk = 63f / 30f;
            const float durationWalk = endTimeWalk - startTimeWalk;
            const float startTimeIdle = 0;
            const float endTimeIdle = 0;
            const float durationIdle = endTimeIdle;
            const float blendDuration = 0.25f;

            const float startTimeRun = 15f / 30f;
            const float endRunTime = 80f / 30f;
            const float durationRun = endRunTime - startTimeRun;

            SceneTests.InitializeScene();
            SceneManager.Scene.Physics.Enable = true;
            SceneManager.Scene.Physics.Visible = false;

            CharacterControllerTagProcessor.ControllerCreated += CharacterControllerTagProcessor_ControllerCreated;

            ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\game_level0\game_level0.DAE");
            ContentImporter.ImportAnimation(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\Lighting\walk.DAE");
            ContentImporter.ImportAnimation(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\Lighting\run.DAE");

            CharacterController controller = CharacterControllerManager.Instance.Controllers[0];
            var animationWalk = SceneManager.Scene.AnimManager.Animations[1];
            var animationRun = SceneManager.Scene.AnimManager.Animations[2];

            var character = (((Frame)controller.Affectable)).FindNode((Frame x)=>x.Type == FrameType.Bone);
            SceneManager.Scene.FindNode("camera1").BindTo(character);

            ThirdPersonControllerDesc desc = new ThirdPersonControllerDesc
            {
                CollitionController = controller,
                Character = character,
                CameraMaxDistance = 100,
                CameraMinDistance = 20,
                CameraMaxPich = Numerics.ToRadians(45),
                CameraMinPich = Numerics.ToRadians(25),
                TransitionTime = blendDuration,
                Camera = SceneManager.Scene.FindNode("camera1"),
                Idle = new KeyFrameAnimationPlayback(animationWalk, startTimeIdle, durationIdle, AnimationLooping.Loop),
                Walk = new KeyFrameAnimationPlayback(animationWalk, startTimeWalk, durationWalk, AnimationLooping.Loop, velocity:1),
                //Walk = new AnimationPlayback(animationRun, startTimeRun, durationRun, AnimationLooping.Secuential),
            };
            ThirdPersonController characterController = new ThirdPersonController(desc);          
         
            SceneManager.Scene.Dynamics.Add(characterController);

            //EngineState.Shadow.ShadowMapping.Bias = 0.9e-2f;
            FrameLight.CreateShadowMapForAllLights(SceneManager.Scene);
         

        }

        void CharacterControllerTagProcessor_ControllerCreated(Igneel.Physics.CharacterControllerDesc arg1, Frame arg2)
        {
            arg1.HitReport = new MyHitReport();
        }

        void DrawCamera()
        {
            if (_box == null)
            {
                _box = Mesh.CreateBox(2, 2, 1);
            }
            if (_rastState == null)
            {
                _rastState = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc(true)
                {
                    Fill = FillMode.Wireframe,
                    Cull = CullMode.None
                });
            }
            var device = GraphicDeviceFactory.Device;
            var effect = Service.Require<RenderMeshIdEffect>();
        
            effect.U.World = Matrix.Translate(0, 0, 0.5f) * _targetCamera.InvViewProjection;
            effect.U.ViewProj = SceneManager.Scene.ActiveCamera.ViewProj;
            effect.U.gId = new Vector4(1);

            device.RasterizerStack.Push(_rastState);
            _box.Draw(device, effect);
            device.RasterizerStack.Pop();
        }
    }

    public class MyHitReport : IUserControllerHitReport
    {

        public ControllerAction OnShapeHit(ref ControllerShapeHit hit)
        {
            return ControllerAction.ActionPush;
        }

        public ControllerAction OnControllerHit(Igneel.Physics.CharacterController controller, Igneel.Physics.CharacterController other)
        {
            return ControllerAction.ActionPush;
        }
    }

}
