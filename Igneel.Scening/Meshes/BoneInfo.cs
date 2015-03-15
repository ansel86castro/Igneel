using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using Igneel.Collections;
using Igneel.Assets;
using Igneel.Animations;
using Igneel.Graphics;

namespace Igneel.Scenering
{

    public static class BoneInfo
    {
        static Dictionary<SceneNode, List<SkinDeformer>> skins = new Dictionary<SceneNode, List<SkinDeformer>>();

        public static void AddSkin(SceneNode bone, SkinDeformer skin)
        {
            var list = GetSkins(bone);
            if (list == null)
            {
                list = new List<SkinDeformer>() { skin };
                bone.Disposing += bone_Disposing;
                skins.Add(bone, list);
            }
            else
                list.Add(skin);
        }

        static void bone_Disposing(object sender, EventArgs e)
        {
            var bone = (SceneNode)sender;
            skins.Remove(bone);           
        }

        public static bool RemoveSkin(SceneNode bone, SkinDeformer skin)
        {
            var list = GetSkins(bone);
            if (list != null)
            {
                if (list.Count > 0)
                    return list.Remove(skin);
                else
                    return skins.Remove(bone);
            }
            return false;
        }

        public static List<SkinDeformer> GetSkins(SceneNode bone)
        {
            List<SkinDeformer> skin;
            skins.TryGetValue(bone, out skin);
            return skin;
        }

        public static SceneNode GetRootBone(SceneNode bone)
        {
            SceneNode cursor = bone;
            while (cursor.Parent != null && cursor.Parent.Type == NodeType.Bone)
                cursor = cursor.Parent;

            return cursor;
        }

        public static IEnumerable<Vector3> EnumerateVertexes(SceneNode bone)
        {
            var skins = GetSkins(bone);
            if (skins != null && skins.Count > 0)
            {
                foreach (var skin in skins)
                {
                    var mesh = skin.Mesh;
                    var positionView = mesh.GetVertexBufferView<Vector3>(IASemantic.Position);
                    var indicesView = mesh.GetVertexBufferView<Vector4>(IASemantic.BlendIndices);

                    var boneIndex = Array.IndexOf(skin.Bones, bone);
                    var pose = skin.BindShapePose * skin.BoneBindingMatrices[boneIndex] * bone.GlobalPose;

                    if (skin.HasBonesPerLayer)
                    {
                        for (int iLayer = 0; iLayer < mesh.LayerCount; iLayer++)
                        {
                            var layer = mesh.Layers[iLayer];
                            var bonesMapping = skin.GetLayerBones(iLayer);
                            for (int ipos = 0; ipos < layer.vertexCount; ipos++)
                            {
                                if (ContainsBone(indicesView[layer.startVertex + ipos], bonesMapping, boneIndex))
                                {
                                    yield return Vector3.Transform(positionView[layer.startVertex + ipos], pose);
                                }
                            }
                        }
                    }

                    for (int ipos = 0; ipos < positionView.Count; ipos++)
                    {
                        if (ContainsBone(indicesView[ipos], null, boneIndex))
                        {
                            yield return Vector3.Transform(positionView[ipos], pose);
                        }
                    }

                    mesh.ReleaseVertexBufferViews();
                }
            }
            //else
            //{
            //    foreach (var position in base.GetVolumePoints())
            //    {
            //        yield return position;
            //    }
            //}

        }

        public static Vector3[] GetVertexes(SceneNode bone)
        {
            return EnumerateVertexes(bone).ToList().ToArray();
        }

        private unsafe static bool ContainsBone(Vector4 blendIndices, int[] boneMapping, int boneIndex)
        {
            float* pBoneIndices = (float*)&blendIndices;
            for (int k = 0; k < 4; k++)
            {
                if (pBoneIndices[k] >= 0 &&
                    (boneMapping != null ? boneMapping[(int)pBoneIndices[k]] : pBoneIndices[k]) == boneIndex)
                {
                    return true;
                }
            }

            return false;
        }      
    }

    //public class Bone:SceneNode
    //{
    //    List<SkinController> skins = new List<SkinController>();

    //    public Bone(Scene scene, string name = null) : base(scene, name) 
    //    {
    //        Type = NodeType.Joint;
    //    }        

    //    public List<SkinController> Skins { get { return skins; } }

    //    public static Bone GetRoot(Bone bone)
    //    {
    //        Bone cursor = bone;;
    //        while (cursor.Parent != null && cursor.Parent is Bone)
    //            cursor = (Bone)cursor.Parent;
    //        return cursor;
    //    }

       
    //}
}
