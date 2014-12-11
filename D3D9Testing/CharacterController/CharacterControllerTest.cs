using Igneel;
using Igneel.Importers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.TagProcesors;
using Igneel.Physics;
using Igneel.Input;
using Igneel.Components;
using Igneel.Animations;
using Igneel.Graphics;
using Igneel.Rendering.Effects;

namespace D3D9Testing
{
    [Test]
    public class CharacterControllerTest
    {
        private Mesh box;
        private RasterizerState rastState;
        Camera targetCamera;
        [TestMethod]
        public void CreateControllerFromFile()
        {
            SceneTests.InitializeScene();
            Engine.PhyScene.Enable = true;
            Engine.PhyScene.Visible = true;

            CharacterControllerTagProcessor.ControllerCreated += CharacterControllerTagProcessor_ControllerCreated;
            ContentImporter.Import(Engine.Scene, @"C:\Users\ansel\Documents\3dsmax\export\controller.DAE");
            CharacterController controller = CharacterControllerManager.Instance.Controllers[0];

            Engine.Scene.Dynamics.Add(new Dynamic( deltaT =>
                {
                    var camera = Engine.Scene.ActiveCamera;                                    
                    Vector3 direction = new Vector3();
                    if (Engine.KeyBoard.IsKeyPressed(Keys.O))
                        direction = camera.Front;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.L))
                        direction = -camera.Front;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.K))
                        direction = -camera.Right;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.SEMICOLON))
                        direction = camera.Right;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.P))
                        direction = camera.Up;
                    if (Engine.KeyBoard.IsKeyPressed(Keys.I))
                        direction = -camera.Up;

                    CollitionResult result = controller.Move(direction * 100 * deltaT, (1 << (int)CollisionGroup.GROUP_NON_COLLIDABLE)|
                                                                          (1 << (int)CollisionGroup.GROUP_COLLIDABLE_NON_PUSHABLE) , 0.000001f, 1.0f);
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
            Engine.PhyScene.Enable = true;
            Engine.PhyScene.Visible = false;

            CharacterControllerTagProcessor.ControllerCreated += CharacterControllerTagProcessor_ControllerCreated;

            ContentImporter.Import(Engine.Scene, @"C:\Users\ansel\Documents\3dsmax\export\game_level0\game_level0.DAE");
            ContentImporter.ImportAnimation(Engine.Scene, @"C:\Users\ansel\Documents\3dsmax\export\Lighting\walk.DAE");
            ContentImporter.ImportAnimation(Engine.Scene, @"C:\Users\ansel\Documents\3dsmax\export\Lighting\run.DAE");

            CharacterController controller = CharacterControllerManager.Instance.Controllers[0];
            var animationWalk = Engine.Scene.AnimManager.Animations[1];
            var animationRun = Engine.Scene.AnimManager.Animations[2];

            var character = (((SceneNode)controller.Affectable)).GetNode((SceneNode x)=>x.Type == NodeType.Bone);
            Engine.Scene.GetNode("camera1").BindTo(character);

            ThirdPersonControllerDesc desc = new ThirdPersonControllerDesc
            {
                CollitionController = controller,
                Character = character,
                CameraMaxDistance = 100,
                CameraMinDistance = 20,
                CameraMaxPich = Numerics.ToRadians(45),
                CameraMinPich = Numerics.ToRadians(25),
                TransitionTime = blendDuration,
                Camera = Engine.Scene.GetNode("camera1"),
                Idle = new AnimationPlayback(animationWalk, startTimeIdle, durationIdle, AnimationLooping.Secuential),
                Walk = new AnimationPlayback(animationWalk, startTimeWalk, durationWalk, AnimationLooping.Secuential, velocity:1),
                //Walk = new AnimationPlayback(animationRun, startTimeRun, durationRun, AnimationLooping.Secuential),
            };
            ThirdPersonController characterController = new ThirdPersonController(desc);          
         
            Engine.Scene.Dynamics.Add(characterController);

            //Engine.Shadow.ShadowMapping.Bias = 0.9e-2f;
            LightInstance.CreateShadowMapForAllLights(Engine.Scene);
         

        }

        void CharacterControllerTagProcessor_ControllerCreated(Igneel.Physics.CharacterControllerDesc arg1, Igneel.Components.SceneNode arg2)
        {
            arg1.HitReport = new MyHitReport();
        }

        void DrawCamera()
        {
            if (box == null)
            {
                box = Mesh.CreateBox(2, 2, 1);
            }
            if (rastState == null)
            {
                rastState = Engine.Graphics.CreateRasterizerState(new RasterizerDesc(true)
                {
                    Fill = FillMode.Wireframe,
                    Cull = CullMode.None
                });
            }
            var device = Engine.Graphics;
            var effect = Service.Require<RenderMeshIdEffect>();
        
            effect.Constants.World = Matrix.Translate(0, 0, 0.5f) * targetCamera.InvViewProjection;
            effect.Constants.ViewProj = Engine.Scene.ActiveCamera.ViewProj;
            effect.Constants.gId = new Vector4(1);

            device.PushGraphicState<RasterizerState>(rastState);
            box.Draw(device, effect);
            device.PopGraphicState<RasterizerState>();
        }
    }

    public class MyHitReport : IUserControllerHitReport
    {

        public ControllerAction OnShapeHit(ref ControllerShapeHit hit)
        {
            return ControllerAction.ACTION_PUSH;
        }

        public ControllerAction OnControllerHit(Igneel.Physics.CharacterController controller, Igneel.Physics.CharacterController other)
        {
            return ControllerAction.ACTION_PUSH;
        }
    }

}
