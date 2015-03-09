using Igneel.Physics;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Rendering.Effects;
using Igneel.Components;
using System.Drawing;

namespace Igneel.Rendering
{
    public class ActorRender : GraphicObjectRender<RenderMeshIdEffect, Actor>, IComponentRender<Actor>       
    {      
        Mesh sphere;        
        Mesh box;
        Mesh capsule;
        Mesh cylindre;                                   
        private RasterizerState rasterizer;
        Color4 color = new Color4(0, 1, 0);

        public ActorRender()
        {
            sphere = Mesh.CreateSphere(16, 16, 1);
            box = Mesh.CreateBox(2, 2, 2);
            cylindre = Mesh.CreateCylindre(1, 16, 1, 1, false);
            capsule = Mesh.CreateCapsule(1, 1, 16, 16, 1, 16);
            
            rasterizer = Engine.Graphics.CreateRasterizerState(new RasterizerDesc(true)
            {
                 Fill = FillMode.Wireframe,                  
            });
        }

        public Color4 Color { get { return color; } set { color = value; } }
             
        public override void Draw(Actor comp)
        {
            if (Engine.Scene.ActiveCamera == null)
                return;
            var device = Engine.Graphics;
            device.RasterizerStack.Push(rasterizer);
            device.PrimitiveTopology = IAPrimitive.TriangleList;

            Bind(Engine.Scene.ActiveCamera);

            effect.U.gId = (Vector4)color;
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
                effect.U.World = scale * boxShape.GlobalPose;
                DrawMesh(box, device);      
            }
            else if (shape is SphereShape)
            {
                SphereShape sphereShape = (SphereShape)shape;
                scale = Matrix.Scale(new Vector3(sphereShape.Radius));
                effect.U.World = scale * shape.GlobalPose;
                DrawMesh(sphere, device);      
            }
            else if (shape is PlaneShape)
            {
                scale = Matrix.Scale(Engine.Scene.ActiveCamera.ZFar,0, Engine.Scene.ActiveCamera.ZFar);
                effect.U.World = scale * shape.GlobalPose;
                DrawMesh(box, device);    
            }
            else if (shape is WheelShape)
            {
                WheelShape wheel = (WheelShape)shape;

                effect.U.World = Matrix.Scale(wheel.Radius, 0, wheel.Radius) *
                                         Matrix.RotationZ(Numerics.PIover2) *
                                         Matrix.RotationY(wheel.SteerAngle) *
                                         shape.GlobalPose;

                DrawMesh(cylindre, device);      

            }
            else if (shape is CapsuleShape)
            {
                CapsuleShape capsuleShape = (CapsuleShape)shape;
                float radius = capsuleShape.Radius;
                float height = capsuleShape.Height;
                effect.U.World = Matrix.Translate(0, -0.5f, 0) *
                                         Matrix.Scale(radius, radius, radius) *
                                         Matrix.Translate(0, height * 0.5f, 0) * shape.GlobalPose;

                DrawMeshPart(capsule, capsule.Layers[0], device);                

                effect.U.World =  Matrix.Translate(0, 0.5f, 0) *
                                          Matrix.Scale(radius, radius, radius) *
                                          Matrix.Translate(0, -(height * 0.5f), 0) * shape.GlobalPose;
                DrawMeshPart(capsule, capsule.Layers[2], device);

                effect.U.World =  Matrix.Scale(radius, height, radius) * shape.GlobalPose;
                DrawMeshPart(capsule, capsule.Layers[1], device);
            }
            else if (shape is TriangleMeshShape)
            {
                TriangleMesh tmesh = ((TriangleMeshShape)shape).Mesh;
                var mesh = tmesh.GraphicMesh;
                if (mesh != null)
                {
                    effect. U.World = shape.GlobalPose;

                    DrawMesh(mesh, device);                    

                }
            }
        }

        private void DrawMesh(Mesh mesh, GraphicDevice device)
        {
            device.SetVertexBuffer(0, mesh.VertexBuffer, 0);
            device.SetIndexBuffer(mesh.IndexBuffer);

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
        //    Engine.Graphics.DrawIndexedUserPrimitives(IAPrimitive.TriangleList, 0, boxBuilder.Vertices.Length,
        //                            boxBuilder.Indices.Length / 3, boxBuilder.Indices, true, boxBuilder.Vertices));
        //}

        //public override void Draw(object component)
        //{
        //    Draw((Actor)component);
        //}

        public void DrawController(CharacterController characterController)
        {
            var device = Engine.Graphics;
            device.RasterizerStack.Push(rasterizer);
            device.PrimitiveTopology = IAPrimitive.TriangleList;

            if (characterController is BoxController)
            {
                BoxController boxController = (BoxController)characterController;
                effect.U.World = Matrix.Scale(boxController.Extents) * characterController.GlobalPose;
                DrawMesh(box, Engine.Graphics);      
            }
            else
            {
                CapsuleController capsuleController = (CapsuleController)characterController;
                float radius = capsuleController.Radius;
                float height = capsuleController.Height;
                effect.U.World = Matrix.Translate(0, -0.5f, 0) *
                                         Matrix.Scale(radius, radius, radius) *
                                         Matrix.Translate(0, height * 0.5f, 0) * capsuleController.GlobalPose;

                DrawMeshPart(capsule, capsule.Layers[0], Engine.Graphics);

                effect.U.World = Matrix.Translate(0, 0.5f, 0) *
                                          Matrix.Scale(radius, radius, radius) *
                                          Matrix.Translate(0, -(height * 0.5f), 0) * capsuleController.GlobalPose;
                DrawMeshPart(capsule, capsule.Layers[2], Engine.Graphics);

                effect.U.World = Matrix.Scale(radius, height, radius) * capsuleController.GlobalPose;
                DrawMeshPart(capsule, capsule.Layers[1], Engine.Graphics);
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
