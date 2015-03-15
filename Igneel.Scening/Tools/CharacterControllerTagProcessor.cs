using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Igneel.Scenering.TagProcesors
{
    public class CharacterControllerTagProcessor : TagProcessor
    {
        Regex regex = new Regex(@"__cc(_b(?<BINDING>\w+))?__");

        public static event Action<CharacterControllerDesc, SceneNode> ControllerCreated;

        public override object Process(Scene scene, SceneNode node)
        {
            if (scene == null || scene.Physics == null)
                return null;

            string tag = node.Tag;
            if (tag == null) return null;

            var match = regex.Match(tag);
            if (!match.Success) return null;

            var obj = node.NodeObject as IMeshContainer;
            if (obj == null)
                return null;

            CharacterControllerDesc controllerDesc;
            var shapeDesc = obj.Mesh.CreateShapeDescriptor();
            if (shapeDesc.Type == Physics.ShapeType.BOX)
            {
                BoxShapeDesc box = (BoxShapeDesc)shapeDesc;
                controllerDesc = new BoxControllerDesc
                {
                    Extents = box.Dimensions,
                    Position =box.LocalPose.Translation + node.GlobalPose.Translation,
                    UpDirection = HeightFieldAxis.Y,
                    SlopeLimit = (float)Math.Cos(Numerics.ToRadians(45.0f)),
                    Name = node.Name
                };
            }
            else if (shapeDesc.Type == Physics.ShapeType.CAPSULE)
            {
                CapsuleShapeDesc capsuleShape = (CapsuleShapeDesc)shapeDesc;
                controllerDesc = new CapsuleControllerDesc
                {
                    Height = capsuleShape.Height,
                    Position =capsuleShape.LocalPose.Translation + node.GlobalPose.Translation,
                    Radius = capsuleShape.Radius,
                    UpDirection = HeightFieldAxis.Y,
                    SlopeLimit = (float)Math.Cos(Numerics.ToRadians(45.0f)),
                    Name = node.Name
                };
            }
            else
                return null;

            SceneNode bindedNode = null;
            if (match.Groups["BINDING"].Success)
            {
                bindedNode = scene.GetNode(match.Groups["BINDING"].Value);
            }
            if (ControllerCreated != null)
                ControllerCreated(controllerDesc, bindedNode);


            var controller = CharacterControllerManager.Instance.CreateController(scene.Physics, controllerDesc);
            if (bindedNode!=null)
                bindedNode.BindTo(controller);

            obj.Mesh.Dispose();
            node.Remove();
            node.Dispose();
            
            return controller;
        }
    }
}
