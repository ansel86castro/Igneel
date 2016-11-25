using System;
using Igneel.Components;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Physics;

namespace Igneel.Rendering
{
    public class ActorRender : Render<Actor, RenderMeshIdEffect>, IComponentRender<Actor>       
    {      
        Mesh _sphere;        
        Mesh _box;
        Mesh _capsule;
        Mesh _cylindre;                                   
        private RasterizerState _rasterizer;
        Color4 _color = new Color4(0, 1, 0);

        public ActorRender(GraphicDevice device):base(device)
        {
            _sphere = Mesh.CreateSphere(16, 16, 1);
            _box = Mesh.CreateBox(2, 2, 2);
            _cylindre = Mesh.CreateCylindre(1, 16, 1, 1, false);
            _capsule = Mesh.CreateCapsule(1, 1, 16, 16, 1, 16);
            
            _rasterizer = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc(true)
            {
                 Fill = FillMode.Wireframe,                  
            });
        }

        public Color4 Color { get { return _color; } set { _color = value; } }
             
        public override void Draw(Actor comp)
        {
            if (Engine.Scene.ActiveCamera == null)
                return;
            var device = GraphicDeviceFactory.Device;
            device.RasterizerStack.Push(_rasterizer);
            device.PrimitiveTopology = IAPrimitive.TriangleList;

            Bind(Engine.Scene.ActiveCamera);

            Effect.Input.gId = (Vector4)_color;
            foreach (var shape in comp.Shapes)
            {               
                DrawShape(shape, device);
            }

            device.RasterizerStack.Pop();            
        }

        private void DrawShape(ActorShape shape, GraphicDevice device)
        {
            Matrix scale;
            if (shape is BoxShape)
            {
                BoxShape boxShape = (BoxShape)shape;
                scale = Matrix.Scale(boxShape.Dimensions);
                Effect.Input.World = scale * boxShape.GlobalPose;
                DrawMesh(_box, device);      
            }
            else if (shape is SphereShape)
            {
                SphereShape sphereShape = (SphereShape)shape;
                scale = Matrix.Scale(new Vector3(sphereShape.Radius));
                Effect.Input.World = scale * shape.GlobalPose;
                DrawMesh(_sphere, device);      
            }
            else if (shape is PlaneShape)
            {
                scale = Matrix.Scale(Engine.Scene.ActiveCamera.ZFar,0, Engine.Scene.ActiveCamera.ZFar);
                Effect.Input.World = scale * shape.GlobalPose;
                DrawMesh(_box, device);    
            }
            else if (shape is WheelShape)
            {
                WheelShape wheel = (WheelShape)shape;

                Effect.Input.World = Matrix.Scale(wheel.Radius, 0, wheel.Radius) *
                                         Matrix.RotationZ(Numerics.PIover2) *
                                         Matrix.RotationY(wheel.SteerAngle) *
                                         shape.GlobalPose;

                DrawMesh(_cylindre, device);      

            }
            else if (shape is CapsuleShape)
            {
                CapsuleShape capsuleShape = (CapsuleShape)shape;
                float radius = capsuleShape.Radius;
                float height = capsuleShape.Height;
                Effect.Input.World = Matrix.Translate(0, -0.5f, 0) *
                                         Matrix.Scale(radius, radius, radius) *
                                         Matrix.Translate(0, height * 0.5f, 0) * shape.GlobalPose;

                DrawMeshPart(_capsule, _capsule.Layers[0], device);

                Effect.Input.World = Matrix.Translate(0, 0.5f, 0) *
                                          Matrix.Scale(radius, radius, radius) *
                                          Matrix.Translate(0, -(height * 0.5f), 0) * shape.GlobalPose;
                DrawMeshPart(_capsule, _capsule.Layers[2], device);

                Effect.Input.World = Matrix.Scale(radius, height, radius) * shape.GlobalPose;
                DrawMeshPart(_capsule, _capsule.Layers[1], device);
            }
            else if (shape is TriangleMeshShape)
            {
                TriangleMesh tmesh = ((TriangleMeshShape)shape).Mesh;
                var mesh = (Mesh)tmesh.GraphicMesh;
                if (mesh != null)
                {
                    Effect.Input.World = shape.GlobalPose;

                    DrawMesh(mesh, device);                    

                }
            }
        }

        private void DrawMesh(Mesh mesh, GraphicDevice device)
        {
            device.SetVertexBuffer(0, mesh.VertexBuffer, 0);
            device.SetIndexBuffer(mesh.IndexBuffer);
            var effect = Effect;
            foreach (var pass in effect.Passes())
            {
                effect.Apply(pass);
                device.DrawIndexed(mesh.Layers[0].IndexCount, 0, 0);
            }
        }

        private void DrawMeshPart(Mesh mesh, MeshPart part ,GraphicDevice device)
        {
            device.SetVertexBuffer(0, mesh.VertexBuffer, 0);
            device.SetIndexBuffer(mesh.IndexBuffer);
            var effect = Effect;
            foreach (var pass in effect.Passes())
            {
                effect.Apply(pass);
                device.DrawIndexed(part.IndexCount, part.StartIndex, 0);
            }
        }


        //private void DrawCapsuleLayer(Device device, ShapeLayer layer)
        //{
        //    effect.Apply(() =>
        //                  device.DrawIndexedUserPrimitives(IAPrimitive.TriangleList, layer.StartIndex, layer.StartVertex,
        //                   layer.VertexCount, layer.PrimitiveCount, capsuleBuilder.Indices, Format.Index16, capsuleBuilder.Vertices, vd.Size));
        //}        

        //public void DrawPlane(Matrix pose)
        //{
        //    var camera = Engine.Scene.ActiveCamera;
        //    DrawBox(pose, new Vector3(camera.ZFar, 0, camera.ZFar));
        //}

        //public void DrawBox(Matrix pose, Vector3 dimensions)
        //{
        //    var scale = Matrix.Scaling(dimensions);
        //    effect.SetWorldMatrix(scale * pose);
        //    effect.Apply(() =>
        //    GraphicDeviceFactory.Device.DrawIndexedUserPrimitives(IAPrimitive.TriangleList, 0, boxBuilder.Vertices.Length,
        //                            boxBuilder.Indices.Length / 3, boxBuilder.Indices, true, boxBuilder.Vertices));
        //}

        //public override void Draw(object component)
        //{
        //    Draw((Actor)component);
        //}

        public void DrawController(CharacterController characterController)
        {
            var device = GraphicDeviceFactory.Device;
            device.RasterizerStack.Push(_rasterizer);
            device.PrimitiveTopology = IAPrimitive.TriangleList;
            var effect = Effect;
            if (characterController is BoxController)
            {
                BoxController boxController = (BoxController)characterController;
                effect.Input.World = Matrix.Scale(boxController.Extents) * characterController.GlobalPose;
                DrawMesh(_box, GraphicDeviceFactory.Device);      
            }
            else
            {
                CapsuleController capsuleController = (CapsuleController)characterController;
                float radius = capsuleController.Radius;
                float height = capsuleController.Height;
                effect.Input.World = Matrix.Translate(0, -0.5f, 0) *
                                         Matrix.Scale(radius, radius, radius) *
                                         Matrix.Translate(0, height * 0.5f, 0) * capsuleController.GlobalPose;

                DrawMeshPart(_capsule, _capsule.Layers[0], GraphicDeviceFactory.Device);

                effect.Input.World = Matrix.Translate(0, 0.5f, 0) *
                                          Matrix.Scale(radius, radius, radius) *
                                          Matrix.Translate(0, -(height * 0.5f), 0) * capsuleController.GlobalPose;
                DrawMeshPart(_capsule, _capsule.Layers[2], GraphicDeviceFactory.Device);

                effect.Input.World = Matrix.Scale(radius, height, radius) * capsuleController.GlobalPose;
                DrawMeshPart(_capsule, _capsule.Layers[1], GraphicDeviceFactory.Device);
            }

            device.RasterizerStack.Pop();
        }
    }

    class ColorActorBinding : RenderBinding<Actor>
    {

        public override void OnBind(Actor value)
        {
            throw new NotImplementedException();
        }

        public override void OnUnBind(Actor value)
        {
            throw new NotImplementedException();
        }
    }

    class IdShapeBinding : RenderBinding<ActorShape>
    {
        public override void OnBind(ActorShape value)
        {
            throw new NotImplementedException();
        }

        public override void OnUnBind(ActorShape value)
        {
            throw new NotImplementedException();
        }
    }    
}
