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
using Igneel;
using Igneel.Effects;
using Igneel.Techniques;
using Igneel.Rendering.Bindings;
using Igneel.SceneComponents;

namespace Igneel.Design
{
    public class DesignEnvironment : GraphicObject<DesignEnvironment>, IDeferreable
    {
        internal VertexPositionColor[] Lines;
        internal VertexPositionColor[] Axis;       
        float _scale = 10;
        float _size = 1000;
        GraphicBuffer _vb;
        
        public interface IMap
        {
            Matrix World { get; set; }
        }

        IMap _map;

        public DesignEnvironment()                        
        {          
            InitializeEnviroment3D();

            SetRender<DefaultTechnique, RenderMeshColorEffect>((component, render) =>
            {
                var device = GraphicDeviceFactory.Device;

                var effect = render.Effect;
                if (_map == null)
                    _map = effect.Map<IMap>();
                _map.World = Matrix.Identity;

                device.PrimitiveTopology = IAPrimitive.LineList;
                device.SetVertexBuffer(0, _vb, 0);

                foreach (var pass in effect.Passes(0))
                {
                    effect.Apply(pass);
                    device.Draw(Axis.Length + Lines.Length, 0);
                }
                effect.EndPasses();       

            }).BindWith(new CameraBinding());
            
            SetNullRender<DepthSceneRender>();

        }

        public float Scale { get { return _scale; } set { _scale = value; } }

        public float Size { get { return _size; } set { _size = value; } }      

        private void InitializeEnviroment3D()
        {

            if (Axis == null)
                Axis = new VertexPositionColor[6];

            //X axis
            Axis[0] = new VertexPositionColor(Vector3.UnitX * _size, new Color4(Color.Red.ToArgb()));
            Axis[1] = new VertexPositionColor(Vector3.UnitX * -_size, new Color4(Color.Red.ToArgb()));
            //Y axis
            Axis[2] = new VertexPositionColor(Vector3.UnitY * _size, new Color4(Color.Green.ToArgb()));
            Axis[3] = new VertexPositionColor(Vector3.UnitY * -_size, new Color4(Color.Green.ToArgb()));
            //Z axis
            Axis[4] = new VertexPositionColor(Vector3.UnitZ * _size, new Color4(Color.Blue.ToArgb()));
            Axis[5] = new VertexPositionColor(Vector3.UnitZ * -_size, new Color4(Color.Blue.ToArgb()));

            float left = -_size;
            float right = _size;
            float top = _size;
            float bottom = -_size;

            int sizeH = (int)(2 * _size / _scale);
            int sizeV = sizeH;


            Vector4 color = new Vector4(Color.DarkGray.ToArgb());

            Lines = new VertexPositionColor[(sizeH + sizeV) * 2 + 4];

            int k = 0;
            //horizontal Lines
            for (int i = 0; i <= sizeH; i++)
            {
                float z = top - _scale * i;
                Lines[k++] = new VertexPositionColor(new Vector3(left, 0, z), color);
                Lines[k++] = new VertexPositionColor(new Vector3(right, 0, z), color);
            }
            //Vertical Lines            
            for (int i = 0; i <= sizeV; i++)
            {
                float x = left + _scale * i;
                Lines[k++] = new VertexPositionColor(new Vector3(x, 0, top), color);
                Lines[k++] = new VertexPositionColor(new Vector3(x, 0, bottom), color);
            }

            VertexPositionColor[] vbData = new VertexPositionColor[Axis.Length + Lines.Length];
            Array.ConstrainedCopy(Lines, 0, vbData, 0, Lines.Length);
            Array.ConstrainedCopy(Axis, 0, vbData, Lines.Length, Axis.Length);

            int vertexSize = Marshal.SizeOf(typeof(VertexPositionColor));
            _vb = GraphicDeviceFactory.Device.CreateVertexBuffer(vbData.Length * vertexSize, vertexSize, vbData, usage: ResourceUsage.Default, cpuAcces: CpuAccessFlags.ReadWrite);
           
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
           
    }

    //public class DesignEnvironmentRender : Render<RenderMeshIdEffect<VertexPositionColor>, DesignEnvironment>
    //{
    //    public override void Draw(DesignEnvironment component)
    //    {
    //        var device = GraphicDeviceFactory.Device;

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
