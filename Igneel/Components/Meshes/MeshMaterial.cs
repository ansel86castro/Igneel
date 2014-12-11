using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Igneel.Graphics;
using System.Runtime.Serialization;
using System.ComponentModel;
using Igneel.Design.UITypeEditors;
using System.Drawing.Design;
using Igneel.Design;
using Igneel.Assets;
using Igneel.Services;
using Igneel.Rendering.Effects;
using Igneel.Rendering;

namespace Igneel.Components
{
    [Serializable]
    [TypeConverter(typeof(DesignTypeConverter))]
    [StructLayout(LayoutKind.Sequential)]
    public struct LayerSurface
    {
        public Color4 Color;

        public float SpecularIntensity;

        public float EmisiveIntensity;

        public float Reflectivity;

        public float Refractitity;

        public float SpecularPower;

        public static LayerSurface Default
        {
            get
            {
                return new LayerSurface()
                {
                    Color = new Color4(1, 1, 1, 1),
                    SpecularIntensity = 1,
                    EmisiveIntensity = 0,
                    Reflectivity = 0,
                    Refractitity = 0,
                    SpecularPower = 4,
                };
            }
        }
    }

    public abstract class SurfaceMaterial : IAssetProvider
    {       
        private LayerSurface surface = LayerSurface.Default;
        private bool containsAlpha;  

        [Browsable(false)]
        public LayerSurface Surface
        {
            get { return surface; }
            set { surface = value; }
        }

        [Category("Diffuse")]
        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIColorTypeEditor), typeof(UITypeEditor))]
        public Vector3 Diffuse
        {
            get
            {
                unsafe
                {
                    var diffuse = surface.Color;
                    return *(Vector3*)&diffuse;
                }
            }
            set 
            {
                unsafe
                {
                    var color = surface.Color;
                    *(Vector3*)&color = value;
                    surface.Color = color;
                }
            }
        }

        [Category("Specular")]
        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIColorTypeEditor), typeof(UITypeEditor))]
        public float SpecularIntensity 
        {
            get
            {
               return surface.SpecularIntensity;
            }
            set
            {
                surface.SpecularIntensity = value;
            }
        }

        [Category("Transparencey")]
        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float Alpha
        {
            get { return surface.Color.A; }
            set
            {
                surface.Color.A = value;
                if (value != 1)
                    containsAlpha = true;
            }
        }

        [Category("Scattering")]
        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float Reflectivity { get { return surface.Reflectivity; } set { surface.Reflectivity = value; } }

        [Category("Scattering")]
        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float Refractitity { get { return surface.Refractitity; } set { surface.Refractitity = value; } }

        [Category("Specular")]
        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]
        public float SpecularPower { get { return surface.SpecularPower; } set { surface.SpecularPower = value; } }

        [Category("Emissive")]
        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIColorTypeEditor), typeof(UITypeEditor))]
        public float EmisiveIntensity { get { return surface.EmisiveIntensity; } set { surface.EmisiveIntensity = value; } }

        public bool ContainsTrasparency { get { return containsAlpha; } protected set { containsAlpha = value; } }      

        public abstract Asset CreateAsset();               
    }

    [TypeConverter(typeof(DesignTypeConverter))]
    public class MeshMaterial : SurfaceMaterial, INameable
    {                       
        private string name;       
          
        private Texture2D diffuseMap;
        private Texture2D specularMap;
        private Texture2D normalMap;           

        public MeshMaterial(string name) 
        { 
            this.name = name;         
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }

        public MeshMaterial():this(null)
        {

        }
      
        [AssetMember]
        [Category("Material")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [AssetMember(StoreAs = StoreType.Reference)]
        [Category("Diffuse")]
        public Texture2D DiffuseMap
        {
            get { return diffuseMap; }
            set 
            {
                diffuseMap = value;
            }
        }

        [AssetMember(StoreAs = StoreType.Reference)]
        [Category("Specular")]
        public Texture2D SpecularMap
        {
            get { return specularMap; }
            set { specularMap = value; }
        }

        [AssetMember(StoreAs = StoreType.Reference)]
        [Category("Normal")]
        public Texture2D NormalMap
        {
            get { return normalMap; }
            set { normalMap = value; }
        }               
      
        public static MeshMaterial CreateDefaultMaterial(string name)
        {
            return new MeshMaterial(name);
        }

        public override Asset CreateAsset()
        {
            return new AutoAsset(this);
        }             

        public override string ToString()
        {
            return name ?? base.ToString();
        }      

        public bool CheckForTransparency()
        {            
            if (Alpha < 1) 
                return ContainsTrasparency = true;
            else if (diffuseMap == null)
                return ContainsTrasparency = false;

            Engine.Lock();
            try
            {
                TransparencyChecker checker = Service.Require<TransparencyChecker>();
                ContainsTrasparency = checker.IsTransparent(diffuseMap);
            }
            finally
            {
                Engine.Unlock();
            }

            return ContainsTrasparency;

            //containsAlpha = Alpha < 1;
            //if (!containsAlpha && diffuseMap != null)
            //    containsAlpha = diffuseMap.ContainsTrasparency();

            //if (containsAlpha && renderLayerIndex == -1)
            //    renderLayerIndex = Scene.TransparentLayer;

            //return containsAlpha;
        }      
    }

    public class TransparencyChecker
    {
        RenderTexture2D renderTexture;       
        Sprite sprite;      
        Effect effect;
        SamplerState samState;
        
        public TransparencyChecker()
        {
            renderTexture = new RenderTexture2D(1, 1, Format.R8G8B8A8_UNORM, Format.UNKNOWN , Multisampling.Disable, true);            

            effect = Effect.GetEffect<CheckTransparenceEffect>();
            sprite = Service.Require<Sprite>();

            samState = Engine.Graphics.CreateSamplerState(new SamplerDesc
            {
                Filter = Filter.MinMagMipPoint,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
            });
        }

        public bool IsTransparent(Texture2D texture)
        {
            var device = Engine.Graphics;
           
            var oldvp = device.RSViewPort;
            device.RSViewPort = new ViewPort(0, 0, 1, 1);

            device.OMSaveRenderTarget();
            renderTexture.SetTarget();            

            device.PSStage.SetResource(0, texture);
            device.PSStage.SetSampler(0, samState);
            
            sprite.Begin();

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color4.Black, 1, 0);

            sprite.SetFullScreenTransform(effect);
            sprite.DrawQuad(effect);
            
            sprite.End();            

            device.OMRestoreRenderTarget();
            device.RSViewPort = oldvp;

            var rec = renderTexture.Texture.Map(0, MapType.Read);
            var containsTransparency = Marshal.ReadByte(rec.DataPointer);
            renderTexture.Texture.UnMap(0);
            
            return containsTransparency > 0;            

        }
    }
}
