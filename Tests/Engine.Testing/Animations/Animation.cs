using Antlr.Runtime;
using Igneel;
using Igneel.Animations;
using Igneel.IA;
using Igneel.Importers;
using Igneel.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;
using Igneel.Animations;
using Igneel.SceneManagement;

namespace D3D9Testing.Animations
{
    [Test]
    public class Animation
    {
        private bool _loadingWeapong;
        Vector3 _disp;

        [TestMethod]
        public void PlayFileAnimation()
        {
            using (System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog())
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SceneTests.InitializeScene();
                    var pk = ContentImporter.Import(SceneManager.Scene, d.FileName);
                    pk.OnSceneAttach(SceneManager.Scene);

                    var animation = SceneManager.Scene.AnimManager.Animations[0];                   
                    var resetter = new AnimationBonesResetter(animation); //  new HeirarchyPoseResetter(root);
                    var state = animation.GetCursor(0);
                    state.Looping = AnimationLooping.Loop;
                   SceneManager.Scene.Dynamics.Add(new Dynamic(deltaT =>
                    {
                        resetter.Reset();

                        animation.Update(deltaT, state, 1, true);                        
                        resetter.CommitChanges();
                    }));                    
                }
            }
        }

        [TestMethod]
        public void Transitions_Idle_Walk()
        {
            SceneTests.InitializeScene();

            const float startTimeWalk = 34f / 30f;
            const float endTimeWalk = 63f / 30f;
            const float durationWalk = endTimeWalk - startTimeWalk;
            const float startTimeIdle = 0;
            const float endTimeIdle = 0;
            const float durationIdle = endTimeIdle;
            const float blendDuration = 0.25f;

            Vector3 speedVector = new Vector3(0, 0, -1f);

            var content = ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\lighting_shadowed.DAE");
            content.OnSceneAttach(SceneManager.Scene);

            Frame cameraNode = SceneManager.Scene.FindNode("camera1");

            Frame root = SceneManager.Scene.EnumerateNodesPosOrden().First(x => x.Type == FrameType.Bone);
            BonesResetter resetter = new BonesResetter(root);

            Vector3 iniTrasnlation = root.LocalPosition;
            Vector3 translation = root.LocalPosition;
            var iniHeading = root.Heading;

            var animation = SceneManager.Scene.AnimManager.Animations[0];

            animation.Sample(startTimeWalk);
            var iniKeyValue = root.LocalPosition;

            animation.Sample(endTimeWalk);
            var lastKeyValue = root.LocalPosition;

            Vector3 lastAnimTrans = root.LocalPosition; //new Vector3();
            bool update = false;
            float deltaH = 0;

            Action<SecuenceNode, float> updateAction = (node, deltaT) =>
            {
                if (!update)
                {
                    lastAnimTrans = root.LocalPosition;
                    update = true;
                    return;
                }

                var cursor = node.Animations[0].Cursor;
                Vector3 disp;
                if (!cursor.TimeRestart)
                    disp = root.LocalPosition - lastAnimTrans;
                else
                    disp = root.LocalPosition - (cursor.PlayDirection > 0 ? iniKeyValue : lastKeyValue);

                disp = Vector3.TransformCoordinates(disp, Matrix.RotationY(deltaH));

                translation += disp;
                lastAnimTrans = root.LocalPosition;
            };

            SecuenceStateMachine states = new SecuenceStateMachine()
                .WithState(new SecuenceNode("idle", animation, startTimeIdle, durationIdle, AnimationLooping.Loop))
                .WithState(new SecuenceNode("walk", animation, startTimeWalk, durationWalk, AnimationLooping.Loop)
                        .Deactivating(node => update = false)
                        .AfterUpdate(updateAction))
                .WithTransition("idle", "walk", new SecuenceTransition(blendDuration)
                        .FiredWhen(t =>
                        {
                            var destNode = t.DestNode;
                            if (Engine.KeyBoard.IsKeyPressed(Keys.Uparrow))
                            {
                                destNode.PlayDirection = 1;
                                update = false;
                                return true;
                            }
                            else if (Engine.KeyBoard.IsKeyPressed(Keys.Downarrow))
                            {
                                destNode.PlayDirection = -1;
                                update = false;
                                return true;
                            }
                            return false;
                        })
                        .WhenBlending((t, dt) =>
                        {
                            Vector3 disp = new Vector3();
                            var cursor = t.DestNode.Animations[0].Cursor;
                            if (Engine.KeyBoard.IsKeyPressed(Keys.Uparrow))
                                disp = speedVector;
                            else
                                disp = -speedVector;
                            translation += Vector3.TransformCoordinates(disp, Matrix.RotationY(deltaH));
                        }))
                .WithTransition("walk", "idle", new SecuenceTransition(blendDuration)
                        .FiredWhen(t =>
                        {
                            if (!Engine.KeyBoard.IsKeyPressed(Keys.Uparrow) && !Engine.KeyBoard.IsKeyPressed(Keys.Downarrow))
                            {
                                update = false;
                                return true;
                            }
                            return false;
                        })
                        .WhenBlending((t, dt) =>
                        {

                        }));

            SceneManager.Scene.Dynamics.Add(
                new Dynamic(deltaT =>
                    {
                        var oldposition = root.GlobalPosition;

                        float rotSpeed = Numerics.ToRadians(90f);

                        if (Engine.KeyBoard.IsKeyPressed(Keys.Leftarrow))
                            deltaH -= rotSpeed * deltaT;
                        else if (Engine.KeyBoard.IsKeyPressed(Keys.Rightarrow))
                            deltaH += rotSpeed * deltaT;

                        resetter.Reset();
                        states.Update(deltaT);

                        root.X = translation.X;
                        root.Z = translation.Z;
                        root.Heading = iniHeading + deltaH;

                        root.ComputeLocalPose();
                        root.CommitChanges();

                        var displacement = root.GlobalPosition - oldposition;
                        cameraNode.X += displacement.X;
                        cameraNode.Z += displacement.Z;
                        cameraNode.CommitChanges();
                    })
                );               
        }

        [TestMethod]
        public void Automata_Idle_Walk()
        {
            SceneTests.InitializeScene();

            const float startTimeWalk = 34f / 30f;
            const float endTimeWalk = 63f / 30f;
            const float durationWalk = endTimeWalk - startTimeWalk;
            const float startTimeIdle = 0;
            const float endTimeIdle = 0;
            const float durationIdle = endTimeIdle;
            const float blendDuration = 0.25f;

            Vector3 speedVector = new Vector3(0, 0, -1f);

            ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\lighting_shadowed.DAE")
               .OnSceneAttach(SceneManager.Scene);
            Frame cameraNode = SceneManager.Scene.FindNode("camera1");

            Frame root = SceneManager.Scene.EnumerateNodesPosOrden().First(x => x.Type == FrameType.Bone);
            BonesResetter resetter = new BonesResetter(root);
            
            Vector3 translation = root.LocalPosition;
            var iniHeading = root.Heading;

            var animation = SceneManager.Scene.AnimManager.Animations[0];

            animation.Sample(startTimeWalk);
            var iniKeyValue = root.LocalPosition;

            animation.Sample(endTimeWalk);
            var lastKeyValue = root.LocalPosition;

            Vector3 lastAnimTrans = root.LocalPosition; //new Vector3();
            bool update = false;
            float deltaH = 0;
          
            KeyFrameAnimationPlayback idle = new KeyFrameAnimationPlayback(animation, startTimeIdle, durationIdle, AnimationLooping.Loop);
            KeyFrameAnimationPlayback walk = new KeyFrameAnimationPlayback(animation, startTimeWalk, durationWalk, AnimationLooping.Loop);
            AnimationTransition idleWalk = new AnimationTransition(idle, walk, blendDuration);
            AnimationTransition walkIdle = new AnimationTransition(walk, idle, blendDuration);

            var walkCursor =  walk.GetCursor(animation);

            Action<float> updateAction = (deltaT) =>
            {
                walk.Update(deltaT);

                if (!update)
                {
                    lastAnimTrans = root.LocalPosition;
                    update = true;
                    return;
                }

                var cursor = walkCursor;            
                if (!cursor.TimeRestart)
                    _disp = root.LocalPosition - lastAnimTrans;
                else
                    _disp = root.LocalPosition - (cursor.PlayDirection > 0 ? iniKeyValue : lastKeyValue);

                _disp = Vector3.TransformCoordinates(_disp, Matrix.RotationY(deltaH));

                translation += _disp;
                lastAnimTrans = root.LocalPosition;
            };

            Automata stateMachine = new Automata()
                .AddState("idle", x =>                
                    idle.Update(x))
                .AddState("idle-walk", dt =>
                {
                    idleWalk.Update(dt);
                    _disp = new Vector3();
                    if (Engine.KeyBoard.IsKeyPressed(Keys.Uparrow))
                        _disp = speedVector;
                    else
                        _disp = -speedVector;
                    _disp =  Vector3.TransformCoordinates(_disp, Matrix.RotationY(deltaH));                  
                })
                .AddState("walk", updateAction)
                .AddState("walk-idle", x => 
                    walkIdle.Update(x))
                .AddTransition("idle", "idle-walk", x =>
                {
                    if (Engine.KeyBoard.IsKeyPressed(Keys.Uparrow))
                    {
                        walk.FirstPlayback.Cursor.PlayDirection = 1;
                        update = false;
                        return true;
                    }
                    else if (Engine.KeyBoard.IsKeyPressed(Keys.Downarrow))
                    {
                        walk.FirstPlayback.Cursor.PlayDirection = -1;
                        update = false;
                        return true;
                    }
                    return false;
                })
                .AddTransition("idle-walk", "walk", x => 
                    idleWalk.TransitionComplete)
                .AddTransition("walk", "walk-idle", x => 
                    !Engine.KeyBoard.IsKeyPressed(Keys.Uparrow) && !Engine.KeyBoard.IsKeyPressed(Keys.Downarrow))
                .AddTransition("walk-idle", "idle", x => 
                    walkIdle.TransitionComplete);

            SceneManager.Scene.Dynamics.Add(new Dynamic(deltaT =>
                {
                    //store position previus animation
                    var localPosition = root.LocalPosition;
                    _disp = new Vector3();

                    float rotSpeed = Numerics.ToRadians(90f);

                    if (Engine.KeyBoard.IsKeyPressed(Keys.Leftarrow))
                        deltaH -= rotSpeed * deltaT;
                    else if (Engine.KeyBoard.IsKeyPressed(Keys.Rightarrow))
                        deltaH += rotSpeed * deltaT;

                    resetter.Reset();
                    stateMachine.Update(deltaT);

                    //root.Tx = translation.X;
                    //root.Tz = translation.Z;
                    root.X = localPosition.X + _disp.X;
                    root.Z = localPosition.Z + _disp.Z;
                    root.Heading = iniHeading + deltaH;

                    root.ComputeLocalPose();
                    root.CommitChanges();

                    //var displacement = root.GlobalPose.Translation - localPosition;
                    //cameraNode.Tx += displacement.X;
                    //cameraNode.Tz += displacement.Z;
                    cameraNode.X += _disp.X;
                    cameraNode.Z += _disp.Z;
                    cameraNode.CommitChanges();
                }));                 
        }
       

        [TestMethod]
        public void BlendAnimation()
        {
            SceneTests.InitializeScene();

            ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\Lighting\armed.DAE")
                .OnSceneAttach(SceneManager.Scene);
            ContentImporter.ImportAnimation(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\Lighting\walk.DAE")
                .OnSceneAttach(SceneManager.Scene);

            var armed = SceneManager.Scene.AnimManager.Animations[0];
            var walk = SceneManager.Scene.AnimManager.Animations[1];            

            Frame root = SceneManager.Scene.EnumerateNodesPosOrden().First(x => x.Type == FrameType.Bone);
            var resetter = new BonesResetter(root);            
            var translation = root.LocalPosition;

            var cursorArmed = armed.GetCursor(0);
            var cursorWalk = walk.GetCursor(0);
            
            cursorArmed.Looping = AnimationLooping.Loop;
            cursorWalk.Looping = AnimationLooping.Loop;

            SceneManager.Scene.Dynamics.Add(
                new Dynamic(deltaT =>
                    {
                        resetter.Reset();

                        walk.Update(deltaT, 0, 1, true);
                        armed.Update(deltaT, 0, 1, true);

                        root.LocalPosition = translation;
                        root.CommitChanges();
                    })
                );            
        }

        //[TestMethod]
        //public void ImportBvhFile()
        //{
        //    string file = @"F:\Motion Capture Data\Example1.bvh";

        //    SceneTests.InitializeScene();

        //    ContentImporter.Import(SceneManager.Scene, @"C:\Users\ansel\Documents\3dsmax\export\Lighting\armed.DAE");
        //    ContentImporter.ImportAnimation(SceneManager.Scene, file);

        //    var armed = SceneManager.Scene.AnimManager.Animations[0];
        //    var walk = SceneManager.Scene.AnimManager.Animations[1];

        //    SceneNode root = SceneManager.Scene.Root.EnumerateNodesPosOrden().First(x => x.Type == NodeType.Bone);
        //    var resetter = new BonesResetter(root);
        //    var translation = root.LocalPosition;

        //    armed.GetCursor(0).Looping = AnimationLooping.Secuential;
        //    walk.GetCursor(0).Looping = AnimationLooping.Secuential;

            
        //    root.IsDynamic = true;
        //    root.UpdateEvent += (IDynamic sender, float deltaT) =>
        //    {
        //        resetter.Reset();

        //        walk.Update(deltaT, 0, 1, true);
        //        //armed.Update(deltaT, 0, 1, true);

        //        root.LocalPosition = translation;
        //        root.CommitChanges();
        //    };
        //}

        //[TestMethod]
        //public void DrawBVHSkelleton()
        //{
        //    SceneTests.InitializeScene();

        //    string file = @"F:\Motion Capture Data\Example1.bvh";
        //    ICharStream input = new ANTLRFileStream(file);
        //    bvhLexer lex = new bvhLexer(input);
        //    CommonTokenStream tokens = new CommonTokenStream(lex);
        //    bvhParser parser = new bvhParser(tokens);
        //    var doc = parser.document();
        //    doc.Root.ComputeTransforms();

        //    var render = new BVHSkelletonRender();
        //    Engine.Presenter.Rendering += () =>
        //        {
        //            render.Draw(doc.Root);
        //        };                        
        //}        
       
    }
}
