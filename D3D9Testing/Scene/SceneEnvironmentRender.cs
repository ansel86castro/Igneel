using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel;
using System.Drawing;
using System.Windows.Forms;
using Igneel.Rendering;
using Igneel.Graphics;
using System.Runtime.InteropServices;
using Igneel.Components;
using Igneel.Rendering.Effects;
using Igneel.Rendering.Bindings;

namespace Igneel.Design
{
    public class DesignEnvironment : GraphicObject<DesignEnvironment>, IDeferreable
    {
        internal VertexPositionColor[] lines;
        internal VertexPositionColor[] axis;       
        float scale = 10;
        float size = 1000;
        GraphicBuffer vb;
        
        public interface Map
        {
            Matrix World { get; set; }
        }

        Map map;

        public DesignEnvironment()                        
        {          
            InitializeEnviroment3D();

            SetRender<SceneTechnique, RenderMeshColorEffect>((component, render) =>
            {
                var device = Engine.Graphics;

                var effect = render.Effect;
                if (map == null)
                    map = effect.Map<Map>();
                map.World = Matrix.Identity;

                device.PrimitiveTopology = IAPrimitive.LineList;
                device.SetVertexBuffer(0, vb, 0);

                foreach (var pass in effect.Passes(0))
                {
                    effect.Apply(pass);
                    device.Draw(axis.Length + lines.Length, 0);
                }
                effect.EndPasses();       

            }).BindWith(new CameraBinding());
            
            SetNullRender<DepthSceneRender>();

        }

        public float Scale { get { return scale; } set { scale = value; } }

        public float Size { get { return size; } set { size = value; } }      

        private void InitializeEnviroment3D()
        {

            if (axis == null)
                axis = new VertexPositionColor[6];

            //X axis
            axis[0] = new VertexPositionColor(Vector3.UnitX * size, Color.Red);
            axis[1] = new VertexPositionColor(Vector3.UnitX * -size, Color.Red);
            //Y axis
            axis[2] = new VertexPositionColor(Vector3.UnitY * size, Color.Green);
            axis[3] = new VertexPositionColor(Vector3.UnitY * -size, Color.Green);
            //Z axis
            axis[4] = new VertexPositionColor(Vector3.UnitZ * size, Color.Blue);
            axis[5] = new VertexPositionColor(Vector3.UnitZ * -size, Color.Blue);

            float left = -size;
            float right = size;
            float top = size;
            float bottom = -size;

            int sizeH = (int)(2 * size / scale);
            int sizeV = sizeH;


            Vector4 color = (Vector4)Color.DarkGray;

            lines = new VertexPositionColor[(sizeH + sizeV) * 2 + 4];

            int k = 0;
            //horizontal Lines
            for (int i = 0; i <= sizeH; i++)
            {
                float z = top - scale * i;
                lines[k++] = new VertexPositionColor(new Vector3(left, 0, z), color);
                lines[k++] = new VertexPositionColor(new Vector3(right, 0, z), color);
            }
            //Vertical Lines            
            for (int i = 0; i <= sizeV; i++)
            {
                float x = left + scale * i;
                lines[k++] = new VertexPositionColor(new Vector3(x, 0, top), color);
                lines[k++] = new VertexPositionColor(new Vector3(x, 0, bottom), color);
            }

            VertexPositionColor[] vbData = new VertexPositionColor[axis.Length + lines.Length];
            Array.ConstrainedCopy(lines, 0, vbData, 0, lines.Length);
            Array.ConstrainedCopy(axis, 0, vbData, lines.Length, axis.Length);

            int vertexSize = Marshal.SizeOf(typeof(VertexPositionColor));
            vb = Engine.Graphics.CreateVertexBuffer(vbData.Length * vertexSize, vertexSize, vbData, usage: ResourceUsage.Default, cpuAcces: CpuAccessFlags.ReadWrite);
           
           //unsafe{
           //    VertexPositionColor* pter = (VertexPositionColor*)vb.Map(MapType.ReadWrite);               
           //    for (int i = 0; i < vbData.Length; i++)
           //    {
           //        var pos = pter[i].Position;
           //        pter[i].Color = new Color4(1, 1, 1, 1);
           //    }
           //    vb.Unmap();
           // }

            //vb.Write(axis,0, false);
            //vb.Write(lines, vertexSize * axis.Length, false);
        }       

        public void CommitChanges()
        {
            InitializeEnviroment3D();
        }                  

        public override Assets.Asset CreateAsset()
        {
            return null;
        }
      
    }

    //public class DesignEnvironmentRender : Render<RenderMeshIdEffect<VertexPositionColor>, DesignEnvironment>
    //{
    //    public override void Draw(DesignEnvironment component)
    //    {
    //        var device = Engine.Graphics;

    //        var effect = Effect;
    //        effect.Constants.World = Matrix.Identity;           

    //        device.IAPrimitiveTopology = IAPrimitive.LineList;
    //        device.IASetVertexBuffer(0, component.VertexBuffer, 0);

    //        foreach (var pass in effect.Passes(0))
    //        {
    //            effect.Apply(pass);
    //            device.Draw(component.VertexCount, 0);
    //        }
    //        effect.EndPasses();          
    //    }
    //}

    //[Registrator]
    //public class DesignEnvironmentRegistrator : Registrator<DesignEnvironment>
    //{

    //    public override void RegisterRenders()
    //    {
    //        Register<SceneTechnique, DesignEnvironmentRender>(() => new DesignEnvironmentRender()
    //             .SetBinding(new CameraBinding())
    //             .SetBinding(new SceneNodeWorldBinding()));

    //        RegisterNullRender<DepthSceneRender>();
    //    }
    //}
}
