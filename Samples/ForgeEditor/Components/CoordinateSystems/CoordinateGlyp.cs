using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ForgeEditor.Components.CoordinateSystems;
using ForgeEditor.Effects;
using Igneel;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using Igneel.SceneComponents;
using Igneel.Techniques;

namespace ForgeEditor.Components
{
    public class CoordinateGlyp : DecalGlyp
    {
        public const int None = 0;
        public const int X = 1;
        public const int Y = 2;
        public const int Z = 3;
        public const int XY = 4;
        public const int XZ = 5;
        public const int YZ = 6;      

       private float alpha =1;

       public float Alpha
       {
           get { return alpha; }
           set { alpha = value; }
       }

       float arrowRadius = 2f;

       public float ArrowRadius
       {
           get { return arrowRadius; }
           set { arrowRadius = value; }
       }
       float arrowHeight = 1.5f;

       public float ArrowHeight
       {
           get { return arrowHeight; }
           set { arrowHeight = value; }
       }
       float trunkHeight = 4f;

       public float TrunkHeight
       {
           get { return trunkHeight; }
           set { trunkHeight = value; }
       }
       float width = 3;

       public float Width
       {
           get { return width; }
           set { width = value; }
       }
       float tick = 0.1f;

       public float Tick
       {
           get { return tick; }
           set { tick = value; }
       }

        public CoordinateGlyp()
        {
            
        }      

        public CoordinateGlyp(Igneel.Rectangle rect)
            :base(rect)
        {
            
        }

        public CoordinateGlyp Initialize()
        {
            var graphics = Engine.Graphics;
            GlypComponent[] components;
            if (EnablePlanes)
            {
                components = new GlypComponent[6]
                {
                    new GlypComponent{ Id = X},
                    new GlypComponent{ Id = Y},
                    new GlypComponent{ Id = Z},
                    new GlypComponent{ Id = XY},
                    new GlypComponent{ Id = XZ},
                    new GlypComponent{ Id = YZ}
                };
            }
            else
            {
                components = new GlypComponent[3]
                {
                    new GlypComponent{ Id = X},
                    new GlypComponent{ Id = Y},
                    new GlypComponent{ Id = Z}                 
                };
            }
            Components = components;           
         

            var arrow = new ConeBuilder(16, 16, arrowRadius, arrowHeight);

            //var trunk2 = trunk; // new CylindreBuilder(16, 16, 1, 10, false);
            //cylyndre = new Component { Color = new Color4(1, 1, 1, 1), Axix = AxisName.None };
            //VertexPositionColor[] data2 = new VertexPositionColor[trunk2.Vertices.Length];
            //var transform2 = Matrix.RotationZ(-Numerics.PIover2) * Matrix.Translate(0.5f * trunk2.height, 0, 0);
            //for (int i = 0; i < data2.Length; i++)
            //{
            //    data2[i] = new VertexPositionColor(Vector3.TransformCoordinates(trunk2.Vertices[i].Position, transform2), new Color4(1, 1, 0, 0));
            //}
            //cylyndre.VertexBuffer = graphics.CreateVertexBuffer(data: data2);
            //cylyndre.IndexBufffer = graphics.CreateIndexBuffer(data: trunk2.Indices);

            // X Axis            
            var xAxis = components[0];

            VertexPositionColor[] data = new VertexPositionColor[arrow.Vertices.Length];
            Matrix transform = Matrix.RotationZ(-Numerics.PIover2) * Matrix.Translate(0.5f * arrow.height + trunkHeight, 0, 0);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new VertexPositionColor(Vector3.TransformCoordinates(arrow.Vertices[i].Position, transform), new Color4(alpha,1,0,0));
            }
            xAxis.VertexBuffer = graphics.CreateVertexBuffer(data: data);
            xAxis.IndexBufffer = graphics.CreateIndexBuffer(data: arrow.Indices);

            //Y Axis            
            var yAxis = components[1];
            transform = Matrix.Translate(0, 0.5f * arrow.height + trunkHeight, 0);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new VertexPositionColor(Vector3.TransformCoordinates(arrow.Vertices[i].Position, transform), new Color4(alpha, 0, 1, 0));
            }

            yAxis.VertexBuffer = graphics.CreateVertexBuffer(data: data);
            yAxis.IndexBufffer = graphics.CreateIndexBuffer(data: arrow.Indices);

            // Z axis
            var zAxis = components[2];

            transform = Matrix.RotationX(Numerics.PIover2) * Matrix.Translate(0, 0, 0.5f * arrow.height + trunkHeight);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new VertexPositionColor(Vector3.TransformCoordinates(arrow.Vertices[i].Position, transform), new Color4(alpha, 0, 0, 1));
            }

            zAxis.VertexBuffer = graphics.CreateVertexBuffer(data: data);
            zAxis.IndexBufffer = graphics.CreateIndexBuffer(data: arrow.Indices);

            if (EnablePlanes)
            {
                //XY
                BoxBuilder box = new BoxBuilder(width, width, tick);
                var xyAxis = components[3];

                data = new VertexPositionColor[box.Vertices.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = new VertexPositionColor(box.Vertices[i].Position, new Color4(alpha, 1, 0, 0));
                }
                xyAxis.VertexBuffer = graphics.CreateVertexBuffer(data: data);
                xyAxis.IndexBufffer = graphics.CreateIndexBuffer(data: box.Indices);

                //XZ
                box = new BoxBuilder(width, tick, width);
                var xzAxis = components[4];

                data = new VertexPositionColor[box.Vertices.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = new VertexPositionColor(box.Vertices[i].Position, new Color4(alpha, 0, 0, 1));
                }
                xzAxis.VertexBuffer = graphics.CreateVertexBuffer(data: data);
                xzAxis.IndexBufffer = graphics.CreateIndexBuffer(data: box.Indices);

                //YZ
                box = new BoxBuilder(tick, width, width);
                var yzAxis = components[5];

                data = new VertexPositionColor[box.Vertices.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = new VertexPositionColor(box.Vertices[i].Position, new Color4(alpha, 0, 1, 0));
                }
                yzAxis.VertexBuffer = graphics.CreateVertexBuffer(data: data);
                yzAxis.IndexBufffer = graphics.CreateIndexBuffer(data: box.Indices);
            }

            return this;
        }


        //device.SetVertexBuffer(0, cylyndre.VertexBuffer, 0);
        //device.SetIndexBuffer(cylyndre.IndexBufffer);

        //int indexCount3 = (int)(cylyndre.IndexBufffer.SizeInBytes / cylyndre.IndexBufffer.Stride);

        //effect.OnRender(render);
        //foreach (var pass in effect.Passes(0))
        //{
        //    effect.Apply(pass);
        //    device.DrawIndexed(indexCount3, 0, 0);
        //}
        //effect.EndPasses();

        public bool EnablePlanes { get; set; }
    }
}
