using Igneel.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Importers.BVH
{
    public enum FODChannel
    {
        XPosition, YPosition, ZPosition,
        ZRotation, XRotation, YRotation,
    }

    public class BvhNode:INameable
    {
        Matrix localTransform = Matrix.Identity;
        Matrix globalTransform = Matrix.Identity;

        List<BvhNode> nodes = new List<BvhNode>();

        public bool IsRoot { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Local Translation of the Node with respect to the parent
        /// </summary>
        public Vector3 Offset { get; set; }

        public FODChannel[] Channels { get; set; }

        public List<BvhNode> Nodes { get { return nodes; } }

        /// <summary>
        /// if the node don't have any children
        /// </summary>
        public EndSite End { get; set; }

        public Matrix LocalTransform { get { return localTransform; } }

        public Matrix GlobalTransform { get { return globalTransform; } }

        public void GetNbCurves(out int positions, out int rotations)
        {
            positions = 0;
            rotations = 0;
            foreach (var ch in Channels)
            {
                switch (ch)
                {
                    case FODChannel.XPosition:
                    case FODChannel.YPosition:
                    case FODChannel.ZPosition:
                        positions++;
                        break;
                    case FODChannel.ZRotation:
                    case FODChannel.XRotation:
                    case FODChannel.YRotation:
                        rotations = 1;
                        break;
                }
            }           
        }

        public int GetChannelOffset(FODChannel channel)
        {
            var channels = Channels;            
            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i] == channel)
                    return i;
            }
            return -1;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool IsPositionChannel(FODChannel ch)
        {
            switch (ch)
            {
                case FODChannel.XPosition:
                case FODChannel.YPosition:
                case FODChannel.ZPosition:
                    return true;
            }
            return false;
        }

        public void ComputeTransforms()
        {
            ComputeLocalTransform();
            UpdateGlobalPose(globalTransform);
        }

        private void ComputeLocalTransform()
        {
            foreach (var item in nodes)
            {
                item.ComputeLocalTransform();
            }
            //find the local transform based on the first children Offset if any or 
            // based on the EndSite's Offset
            //the bone's default orientation is asumed as (1,0,0)
            if (nodes.Count > 0)
            {
                var first = nodes[0];

                //find bone's rotation                              
                var invRotation = ComputeLocalTransform(first.Offset);
                foreach (var item in nodes)
                {
                    item.localTransform *= invRotation;
                }
            }
            else if (End != null)
            {
                ComputeLocalTransform(End.Offset);
            }
            else
                throw new InvalidOperationException();
        }

        private Matrix ComputeLocalTransform(Vector3 offset)
        {
            var dir = Vector3.Normalize(Vector3.TransformCoordinates(offset, Matrix.RotationY(-Numerics.PIover2)));
            //(0,0,1) -> euler(0,0,0)
            var euler = Euler.FromDirection(dir);
            var rotationMatrix = euler.ToMatrix() * Matrix.RotationY(Numerics.PIover2);

            localTransform = rotationMatrix;
            localTransform.Translation = Offset;

            //update the childrens local Transforms
            var invRotation = Matrix.Invert(rotationMatrix);
            return invRotation;
        }

        private void UpdateGlobalPose(Matrix parentTransform)
        {            
            globalTransform = localTransform * parentTransform;
            foreach (var item in nodes)
            {
                item.UpdateGlobalPose(globalTransform);
            }
        }
    }    

    public class EndSite
    {
        public Vector3 Offset { get; set; }
    }

    public class BvhMotion
    {
        List<float> data = new List<float>();

        public int FrameCount { get; set; }
        public float FrameTime { get; set; }
        public List<float> Data { get { return data; } }
    }

    public class BvhDocument
    {
        List<BvhNode> nodes = new List<BvhNode>();
       
        public BvhDocument()
        {
          
        }
        
        public BvhNode Root { get; set; }

        public BvhMotion Motion { get; set; }

        public List<BvhNode> Nodes { get { return nodes; } }
    }
}
