using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Graphics;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Reflection;
 
using System.ComponentModel;
using Igneel.Design.UITypeEditors;
using System.Drawing.Design;
using Igneel.Design;

namespace Igneel.Components
{
    //[Serializable]
    //public abstract class Terrain : SceneNode<Terrain>, ITerrain
    //{
    //    internal TextureHandle<Texture>[] layers;
    //    internal TextureHandle<Texture>[] blendLayers;                
    //    internal LayerSurface surface;
    //    internal float xSize;
    //    internal float zSize;      
    //    internal float minHeight;
    //    internal float maxHeight;        
    //    [NonSerialized]internal VertexDescriptor vd;    
    //    internal Matrix invWorldMtx = Matrix.Identity;                     
    //    internal bool valid;      
    //    private bool isAccelerated;
    //    Camera renderingCamera;
    //    Action renderCallback;     

    //    public Terrain()
    //    {
    //        scale = new Vector3(1, 1, 1);
    //        surface = new LayerSurface { Diffuse = Float3.One, Specular = Float3.Cero, SpecularPower = 1, Reflectivity = 0, Refractitity = 0, Alpha = 1 };         
    //        invWorldMtx = Matrix.Invert(GlobalPose);

    //        renderCallback = new Action(RenderGeometry);
    //    }             

    //    #region Properties      

    //    public Camera RenderingCamera { get { return renderingCamera; } }

    //    public bool Valid { get { return valid; } set { valid = value; } }

    //    [TypeConverter(typeof(DesignTypeConverter))]
    //    [Editor(typeof(UITextureArrayTypeEditor), typeof(UITypeEditor))]
    //    public TextureHandle<Texture>[] Layers
    //    {
    //        get { return layers; }
    //        set { layers = value; }
    //    }

    //    [TypeConverter(typeof(DesignTypeConverter))]
    //    [Editor(typeof(UITextureArrayTypeEditor), typeof(UITypeEditor))]
    //    public TextureHandle<Texture>[] BlendLayers
    //    {
    //        get { return blendLayers; }
    //        set { blendLayers = value; }
    //    }      
              
    //    public LayerSurface Surface
    //    {
    //        get { return surface; }
    //        set { surface = value; }
    //    }      

   
    //    public bool IsAccelerated
    //    {
    //        get { return isAccelerated; }
    //        set { isAccelerated = value; }
    //    }
     
    //    [Browsable(false)]
    //    public VertexDescriptor VertexDescriptor
    //    {
    //        get { return vd; }
    //    }            

     
    //    [Browsable(false)]
    //    public Matrix InvWolrdMatrix { get { return invWorldMtx; } }           
             
    //    #endregion      

    //    public abstract void Initialize();

    //    public abstract void AdjustHeights(float minHeight, float maxHeight);        

    //    public abstract float HeightOfTerrain(Vector3 position);

    //    public abstract float HeightAboveTerrain(Vector3 position);

    //    public abstract bool InLineOfSight(Vector3 position1, Vector3 position2);

    //    public abstract Euler GetSlope(Vector3 position, float heading);
          
    //    public abstract void RenderGeometry();                          

    //    public override void CommitChanges()
    //    {
    //        base.CommitChanges();
    //        invWorldMtx = Matrix.Invert(GlobalPose);
    //    }

    //    public override void Dispose()
    //    {
    //        if (!Disposed)
    //        {
    //            if (vd != null)
    //                vd.Dispose();

    //            if (layers != null)
    //                foreach (var layer in layers)
    //                    layer.Dispose();

    //            if (blendLayers != null)
    //                foreach (var layer in blendLayers)
    //                    layer.Dispose();

    //            base.Dispose();             

    //        }
    //    }

    //    public void SaveContent(Uri location)
    //    {
    //        if (layers != null)
    //            foreach (var layer in layers)
    //            {
    //                layer.SaveToUri(location, ImageFileFormat.Dds);
    //            }
    //        if (blendLayers != null)
    //            foreach (var item in blendLayers)
    //            {
    //                item.SaveToUri(location, ImageFileFormat.Dds);
    //            }
    //    }         

    //    //public HitResult GetIntercetedObject(Ray ray, float maxRange)
    //    //{
    //    //    return HitResult.Fail;
    //    //}
      
    //}
}
