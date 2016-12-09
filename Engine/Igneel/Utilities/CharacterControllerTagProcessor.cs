using System;
using System.Text.RegularExpressions;
using Igneel.Physics;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Utilities
{
    public class CharacterControllerTagProcessor : TagProcessor
    {
        Regex _regex = new Regex(@"__cc(_b(?<BINDING>\w+))?__");

        public static event Action<CharacterControllerDesc, Frame> ControllerCreated;

        public override object Process(Scene scene, Frame node)
        {
            if (scene == null || scene.Physics == null)
                return null;

            string tag = node.Tag;
            if (tag == null) return null;

            var match = _regex.Match(tag);
            if (!match.Success) return null;

            var obj = node.Component as IFrameMesh;
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

            Frame bindedNode = null;
            if (match.Groups["BINDING"].Success)
            {
                bindedNode = scene.FindNode(match.Groups["BINDING"].Value);
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
