using System;
using System.Collections.Generic;
using System.Linq;
using Igneel.Components;
using Igneel.Graphics;
using Igneel.SceneManagement;

namespace Igneel.Utilities
{

    public static class BoneInfo
    {
        static Dictionary<Frame, List<MeshSkin>> _skins = new Dictionary<Frame, List<MeshSkin>>();

        public static void AddSkin(Frame bone, MeshSkin skin)
        {
            var list = GetSkins(bone);
            if (list == null)
            {
                list = new List<MeshSkin>() { skin };             
                _skins.Add(bone, list);
            }
            else
                list.Add(skin);
        }

        public static bool RemoveSkin(Frame bone, MeshSkin skin)
        {
            var list = GetSkins(bone);
            if (list != null)
            {
                if (list.Count > 0)
                    return list.Remove(skin);
                else
                    return _skins.Remove(bone);
            }
            return false;
        }

        public static List<MeshSkin> GetSkins(Frame bone)
        {
            List<MeshSkin> skin;
            _skins.TryGetValue(bone, out skin);
            return skin;
        }

        public static Frame GetRootBone(Frame bone)
        {
            Frame cursor = bone;
            while (cursor.Parent != null && cursor.Parent.Type == FrameType.Bone)
                cursor = cursor.Parent;

            return cursor;
        }

        public static IEnumerable<Vector3> EnumerateVertexes(Frame bone)
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
                            var bonesMapping = skin.GetBones(iLayer);
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

        public static Vector3[] GetVertexes(Frame bone)
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
