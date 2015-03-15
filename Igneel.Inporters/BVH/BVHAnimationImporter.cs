using Antlr.Runtime;
using Igneel.Assets;
using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Animations;
using System.IO;

namespace Igneel.Importers.BVH
{
     [ImportFormat(".bvh")]
    public class BVHAnimationImporter:IAnimationImporter
    {
        Scene scene;
        BvhDocument doc;
        Dictionary<string, List<SceneNode>> nodesMap = new Dictionary<string, List<SceneNode>>();

        public ContentPackage ImportAnimation(Components.Scene scene, string filename)
        {
            this.scene = scene;
            ContentPackage pk = new ContentPackage(Path.GetFileNameWithoutExtension(filename));
            var anim = ParseFile(filename, null);
            pk.Add(anim);
            return pk;
        }

        public ContentPackage ImportAnimation(Components.Scene scene, string filename, SceneNode root, string fileNodeRoot)
        {
            this.scene = scene;
            ContentPackage pk = new ContentPackage(Path.GetFileNameWithoutExtension(filename));
            var anim = ParseFile(filename, root);
            pk.Add(anim);
            return pk;
        }

        public Animation ParseFile(string inputFileName, SceneNode root)
        {
            ICharStream input = new ANTLRFileStream(inputFileName);
            bvhLexer lex = new bvhLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lex);          
            bvhParser parser = new bvhParser(tokens);
            doc = parser.document();
            doc.Root.ComputeTransforms();

            #region MapSkelletons

            if (root != null)
                MapNodes(doc.Root, root);
            else
            {
                foreach (var item in scene.EnumerateNodesPosOrden())
                {
                    if (item.IsBoneRoot)
                        MapNodes(doc.Root, item);
                }
            }
            #endregion

            Animation anim = new Animation(Path.GetFileNameWithoutExtension(inputFileName));
          
            float[][] keys = GetAnimationKeys();          
            
            AnimationNode[] animationNodes = CreateAnimationNodes(anim, keys);

            ReadFrames(animationNodes);

            return anim;
        }

        private void ReadFrames(AnimationNode[] animationNodes)
        {
            var motionData = doc.Motion.Data;
            int offset = 0;
            for (int iframe = 0; iframe < doc.Motion.FrameCount; iframe++)
            {
                for (int inode = 0; inode < doc.Nodes.Count; inode++)
                {
                    var bvhNode = doc.Nodes[inode];
                    AnimationNode animNode = animationNodes[inode];

                    if (animNode != null)
                        ReadFrameOutputs(animNode, bvhNode, offset, motionData, iframe);

                    offset += bvhNode.Channels.Length;
                }
            }
        }

        private AnimationNode[] CreateAnimationNodes(Animation anim, float[][] keys)
        {
            AnimationNode[] animationNodes = new AnimationNode[doc.Nodes.Count];
            for (int i = 0; i < animationNodes.Length; i++)
            {
                var animNode = CrateAnimationNode(doc.Nodes[i]);
                animationNodes[i] = animNode;
                if (animNode != null)
                {
                    anim.Nodes.Add(animNode);
                    animNode.CurveKeys = keys;
                }
            }
            return animationNodes;
        }

        private float[][] GetAnimationKeys()
        {
            //create the animation keys
            float[][] keys = new float[1][];
            keys[0] = new float[doc.Motion.FrameCount];
            var framekeys = keys[0];
            float step = doc.Motion.FrameTime;
            for (int i = 0; i < framekeys.Length; i++)
            {
                framekeys[i] = i * step;
            }
            return keys;
        }

        private AnimationNode CrateAnimationNode(BvhNode bvhNode)
        {
            var nodeList = nodesMap[bvhNode.Name];
            if (nodeList == null || nodeList.Count == 0)
                return null;

            AnimationNode animNode = new AnimationNode(bvhNode.Name);
            animNode.Curves = new AnimationCurve[]{
                new AnimationCurve()
                {
                    Name = "Translation",
                    Output = new float[doc.Motion.FrameCount * 3],
                    InterpolationType = InterpolationMethod.LINEAR,
                    OutputDim = 3,
                    CurveOutput = TranslationOuput
                },
                 new AnimationCurve()
                {
                    Name = "Rotation",
                    Output = new float[doc.Motion.FrameCount * 4],
                    InterpolationType = InterpolationMethod.QUATSLERP,
                    OutputDim = 4,
                    CurveOutput = RotationOutput
                }
            };
            var channels = bvhNode.Channels;
          
            //set the animation context        
            animNode.KeysIndices = new int[animNode.Curves.Length];
            animNode.Context = AnimationManager.GetContext<SceneNodeTransforms>(nodeList[0]);
            for (int i = 1; i < nodeList.Count; i++)
            {
                animNode.Context.Next = AnimationManager.GetContext<SceneNodeTransforms>(nodeList[i]);
            }
            return animNode;
        }
                     
        private void ReadFrameOutputs(AnimationNode animNode, BvhNode bvhNode , int offset, List<float>data ,int iframe)
        {            
            AnimationCurve translations = animNode.Curves.SingleOrDefault(x=>x.OutputDim == 3);
            AnimationCurve rotations = animNode.Curves.SingleOrDefault(x => x.OutputDim == 4);

            Matrix frameTransform = Matrix.Identity;
            Matrix rotation;
            Vector3 translation = new Vector3(); //bvhNode.Offset;

            for (int i = bvhNode.Channels.Length - 1; i >= 0 ; i--)		
            {
                var ch = bvhNode.Channels[i];
                var value = data[offset + i];
                switch (ch)
                {
                    case FODChannel.XPosition:
                        translation.X  += value;
                        break;
                    case FODChannel.YPosition:
                        translation.Y += value;
                        break;
                    case FODChannel.ZPosition:
                        translation.Z += value;
                        break;
                    case FODChannel.XRotation:
                        frameTransform *= Matrix.RotationAxis(Vector3.UnitX, -Numerics.ToRadians(value)); 
                        break;
                    case FODChannel.YRotation:
                        frameTransform *= Matrix.RotationAxis(Vector3.UnitY, -Numerics.ToRadians(value));
                        break;
                    case FODChannel.ZRotation:
                        frameTransform *= Matrix.RotationAxis(Vector3.UnitZ, -Numerics.ToRadians(value)); 
                        break;
                }
            }
            rotation = frameTransform;
            frameTransform.Translation = translation;

            ////bvhNode.LocalTransform * relativeToLocalTransform = frameTransform             
            //Matrix relativeToLocalTransform = Matrix.Invert(bvhNode.LocalTransform) * frameTransform;

            var nodes = nodesMap[bvhNode.Name];
            //Matrix nodeFramePose = nodes[0].LocalPose * relativeToLocalTransform;

            //////decompose nodeFramePose for store into the curves          
            //var comp = nodeFramePose.GetComposition();

            unsafe
            {
                if (rotations != null)
                {

                    fixed (float* pter = rotations.Output)
                    {
                        *(Quaternion*)(pter + 4 * iframe) = Quaternion.RotationMatrix(rotation);
                    }
                }

                if (translations != null)
                {

                    fixed (float* pter = translations.Output)
                    {
                        *(Vector3*)(pter + 3 * iframe) = nodes[0].LocalPose.Translation;
                    }
                }

            }
        }

        private bool MapNodes(BvhNode bvhNode, SceneNode sceneNode)
        {
            bool mapped = false;
            if (bvhNode.IsRoot && sceneNode.IsBoneRoot)
            {
                List<SceneNode> list;
                if(!nodesMap.TryGetValue(bvhNode.Name , out list))
                {
                    list = new List<SceneNode>();
                    nodesMap[bvhNode.Name] = list;
                }
                list.Add(sceneNode);
                mapped = true;
            }
            else
            {
                foreach (var name in namesMap[bvhNode.Name])
                {
                    SceneNode mapNode;
                    if ((mapNode = sceneNode.GetNode(x => x.Name.Contains(name))) != null)
                    {
                        List<SceneNode> list;
                        if (!nodesMap.TryGetValue(bvhNode.Name, out list))
                        {
                            list = new List<SceneNode>();
                            nodesMap[bvhNode.Name] = list;
                        }
                        list.Add(mapNode);
                        mapped = true;
                        break;
                    }
                }
            }

            foreach (var child in bvhNode.Nodes)
            {
                foreach (var nodeChild in sceneNode.Childrens)
                {
                    if (MapNodes(child, nodeChild))
                        break;
                }
            }

            return mapped;
        }

        static readonly Dictionary<string, string[]> namesMap = new Dictionary<string, string[]>()
        {
            {"Hips" , new string[]{"Root"}},
            {"ToSpine" , new string[]{"Spine","Spine_Lower"}},
            {"Spine" , new string[]{"Spine1", "Spine_Upper"}},
            {"Spine1" , new string[]{"Spine2"}},
            {"Neck" , new string[]{"Neck"}},
            {"Head" , new string[]{"Head"}},
            {"LeftShoulder" , new string[]{"L_Clavicle"}},
            {"LeftArm" , new string[]{"L_UpperArm"}},
            {"LeftForeArm" , new string[]{"L_Forearm"}},
            {"LeftHand" , new string[]{"L_Hand"}},
            {"RightShoulder" , new string[]{"R_Clavicle"}},
            {"RightArm" , new string[]{"R_UpperArm"}},
            {"RightForeArm" , new string[]{"R_Forearm"}},
            {"RightHand" , new string[]{"R_Hand"}},
            {"LeftUpLeg" , new string[]{"L_Thigh"}},
            {"LeftLeg" , new string[]{"L_Calf"}},
            {"LeftFoot" , new string[]{"L_Foot"}},
            {"LeftToeBase" , new string[]{"L_Toe"}},
            {"RightUpLeg" , new string[]{"R_Thigh"}},
            {"RightLeg" , new string[]{"R_Calf"}},
            {"RightFoot" , new string[]{"R_Foot"}},
            {"RightToeBase" , new string[]{"R_Toe"}},

            {"Chest" ,  new string[]{"Spine","Spine_Lower"}},
            {"LeftCollar" ,  new string[]{"L_Clavicle"}},
            {"LeftUpArm" , new string[]{"L_UpperArm"}},
            {"LeftLowArm" , new string[]{"L_Forearm"}},           
            {"RightCollar" , new string[]{"R_Clavicle"}},
            {"RightUpArm" , new string[]{"R_UpperArm"}},
            {"RightLowArm" , new string[]{"R_Forearm"}},
            {"LeftLowLeg" , new string[]{"L_Calf"}},
            {"RightLowLeg" , new string[]{"R_Calf"}},
            {"RightToeFoot" , new string[]{"R_Toe"}},
            {"LeftToeFoot" , new string[]{"L_Toe"}}
        };

        static readonly ICurveOutput TranslationOuput = new CurveOutput<SceneNodeTransforms>((x, v)=>
        {
            unsafe { x.Traslation = *(Vector3*)v; }
        });

         static readonly ICurveOutput RotationOutput = new  CurveOutput<SceneNodeTransforms>((x, v)=>
        {
            unsafe { x.Rotation = *(Quaternion*)v; }
        });

    }
}
