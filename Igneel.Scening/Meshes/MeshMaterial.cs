using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Igneel.Graphics;
using System.Runtime.Serialization;
using System.ComponentModel;
using Igneel.Assets;
using Igneel.Rendering;
using Igneel.Scenering.Effects;
using Igneel.Rendering.Bindings;

namespace Igneel.Scenering
{
   
    public abstract class SurfaceMaterial : IAssetProvider
    {       
        private LayerSurface surface = LayerSurface.Default;
        private bool containsAlpha;  

       
        public LayerSurface Surface
        {
            get { return surface; }
            set { surface = value; }
        }

        
        public Color3 Diffuse
        {
            get
            {
               return (Color3)surface.Color;
            }
            set 
            {
                surface.Color = new Color4(value, surface.Color.A);                
            }
        }

                     
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

       
        
        public float Reflectivity { get { return surface.Reflectivity; } set { surface.Reflectivity = value; } }       
        
        public float Refractitity { get { return surface.Refractitity; } set { surface.Refractitity = value; } }
               
        public float SpecularPower { get { return surface.SpecularPower; } set { surface.SpecularPower = value; } }        
               
        public float EmisiveIntensity { get { return surface.EmisiveIntensity; } set { surface.EmisiveIntensity = value; } }

        public bool ContainsTrasparency { get { return containsAlpha; } protected set { containsAlpha = value; } }

        public abstract Asset CreateAsset();               
    }

   
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
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [AssetMember(StoreAs = StoreType.Reference)]        
        public Texture2D DiffuseMap
        {
            get { return diffuseMap; }
            set 
            {
                diffuseMap = value;
            }
        }

        [AssetMember(StoreAs = StoreType.Reference)]       
        public Texture2D SpecularMap
        {
            get { return specularMap; }
            set { specularMap = value; }
        }

        [AssetMember(StoreAs = StoreType.Reference)]        
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

            samState = GraphicDeviceFactory.Device.CreateSamplerState(new SamplerDesc
            {
                Filter = Filter.MinMagMipPoint,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
            });
        }

        public bool IsTransparent(Texture2D texture)
        {
            var device = GraphicDeviceFactory.Device;
           
            var oldvp = device.ViewPort;
            device.ViewPort = new ViewPort(0, 0, 1, 1);

            device.SaveRenderTarget();
            renderTexture.SetTarget();            

            device.PS.SetResource(0, texture);
            device.PS.SetSampler(0, samState);
            
            sprite.Begin();

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color4.Black, 1, 0);

            sprite.SetFullScreenTransform(effect);
            sprite.DrawQuad(effect);
            
            sprite.End();            

            device.RestoreRenderTarget();
            device.ViewPort = oldvp;

            var rec = renderTexture.Texture.Map(0, MapType.Read);
            var containsTransparency = ClrRuntime.Runtime.GetValue<byte>(rec.DataPointer);
            renderTexture.Texture.UnMap(0);
            
            return containsTransparency > 0;            

        }
    }
}
