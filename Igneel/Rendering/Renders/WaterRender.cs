using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Igneel.Rendering
{
    public enum CausticMergeType { Modulate = 0, Additive = 1, Substractive = 2 }

    //public class WaterRenderTechnique : CallbackGeometryRender
    //{
    //    Matrix textureMtx;
    //    Matrix waterTexMtx;
    //    float waveHeight;
    //    float waveLenght;
    //    float time;
    //    Vector3 winDirection;
    //    Euler wingOrientation;
    //    float waterScale;

    //    [NonSerialized]
    //    EffectHandle[] technique;

    //    [NonSerialized]
    //    EffectHandle hDiffuseFlag;

    //    [NonSerialized]
    //    EffectHandle hNormalFlag;

    //    public WaterRenderTechnique()
    //    {          
    //        technique = new EffectHandle[2];
    //        technique[0] = effect.GetTechique("tech1");
    //        technique[1] = effect.GetTechique("tech1_Shadowed");

    //        hDiffuseFlag = effect.TryGetGlobalSematic(ShaderSemantics.DIFFUSE_MAP_FLAG);
    //        hNormalFlag = effect.TryGetGlobalSematic(ShaderSemantics.NORMAL_MAP_FLAG);
            
    //        textureMtx = new Matrix();
    //        textureMtx.M11 = 0.5f;
    //        textureMtx.M22 = -0.5f;
    //        textureMtx.M33 = 0.5f;
    //        textureMtx.M41 = 0.5f;
    //        textureMtx.M42 = 0.5f;
    //        textureMtx.M43 = 0.5f;
    //        textureMtx.M44 = 1f;
    //        waterScale = 1.0f;
    //    }

    //    protected override ShaderEffect LoadEffect()
    //    {
    //        return ShaderStore.Water;
    //    }

    //    public Water Water
    //    {
    //        get { return (Water)Node; }
    //        set
    //        {
    //            Node = value;
    //            winDirection = value.WinDirection;
    //            wingOrientation = Euler.FromDirection(winDirection);
    //            waveLenght = value.WaveLenght;
    //            waterScale = 1f / waveLenght;
    //            waveHeight = value.WaveHeight;
    //            time = value.Time;
    //            waterTexMtx = Matrix.RotationZ(wingOrientation.Heading);
    //            waterTexMtx *= Matrix.Scaling(waterScale, waterScale, waterScale);
    //            waterTexMtx *= (Matrix.Translation(0, time, 0));
                
    //        }
    //    }

    //    public float WaveLenght
    //    {
    //        get { return waveLenght; }
    //        set
    //        {
    //            waveLenght = value;
    //            waterScale = 1f / waveLenght;
    //            CreateWaterMtx();
    //        }
    //    }

    //    public float WaveHeight
    //    {
    //        get { return waveHeight; }
    //        set { waveHeight = value; }
    //    }
      
    //    public float Time
    //    {
    //        get { return time; }
    //        set { time = value; }
    //    }
     
    //    public Vector3 WinDirection
    //    {
    //        get { return winDirection; }
    //        set
    //        {
    //            winDirection = value;
    //            wingOrientation = Euler.FromDirection(winDirection);
    //            CreateWaterMtx();
    //        }
    //    }

    //    void CreateWaterMtx()
    //    {
    //        waterTexMtx = Matrix.RotationZ(wingOrientation.Heading) * 
    //            Matrix.Scaling(waterScale, waterScale, waterScale);
    //    }

    //    //public override void BeginRender()
    //    //{
    //    //    if (enable && !begin)
    //    //    {
    //    //        //if (GEngine.Shadow.Enable)
    //    //        //{
    //    //        //    int activeShadowMap = 0;
    //    //        //    var activeLights = GLight.ActiveLights;
    //    //        //    for (int i = 0; i < activeLights.Count; i++)
    //    //        //        if (activeLights[i].ShadowMapTechnique != null && activeShadowMap < 2)
    //    //        //            activeShadowMap++;

    //    //        //    if (activeShadowMap == 0)
    //    //        //        effect.Technique = technique[0];
    //    //        //    else
    //    //        //        effect.Technique = technique[1];
    //    //        //}
    //    //        //else
    //    //        if (GEngine.Shadow.Enable)
    //    //        {
    //    //            effect.Technique = technique[1];
    //    //        }
    //    //        else
    //    //            effect.Technique = technique[0];

    //    //        SetupLights();

    //    //        begin = true;
    //    //    }
    //    //}

    //    //public override void Apply()
    //    //{
    //    //    if (renderCallback != null)
    //    //    {
    //    //        bool noBegin = false;
    //    //        if (!begin)
    //    //        {
    //    //            BeginRender();
    //    //            noBegin = true;
    //    //        }

    //    //        Water water = (Water)geometrySource;

    //    //        winDirection = water.WinDirection;
    //    //        wingOrientation = Attitude.FromDirection(winDirection);
    //    //        waveLenght = water.WaveLenght;
    //    //        waterScale = 1f / waveLenght;
    //    //        waveHeight = water.WaveHeight;
    //    //        time = water.Time;
    //    //        waterTexMtx = Matrix.RotationZ(wingOrientation.Heading) *
    //    //                    Matrix.Scaling(waterScale, waterScale, waterScale);
    //    //        waterTexMtx *= Matrix.Translation(0, time, 0);


    //    //        EffectDescription ed = effect.Description;
    //    //        if (water.DiffuseMap != null && ed.DiffuseSamplerRegister >= 0)
    //    //        {
    //    //            GEngine.RenderManager.Samplers[ed.DiffuseSamplerRegister].SetState(water.DiffuseMap, TextureFilter.Linear, TextureAddress.Mirror);
    //    //            if (hDiffuseFlag != null) effect.SetValue(hDiffuseFlag, true);
    //    //        }
    //    //        else if (hDiffuseFlag != null) effect.SetValue(hDiffuseFlag, false);

    //    //        if (water.NormalMap != null && ed.NormalMapRegister >= 0)
    //    //        {
    //    //            GEngine.RenderManager.Samplers[ed.NormalMapRegister].SetState(water.NormalMap, TextureFilter.Linear, TextureAddress.Mirror);
    //    //            if (hNormalFlag != null) effect.SetValue(hNormalFlag, true);
    //    //        }
    //    //        else if (hNormalFlag != null) effect.SetValue(hNormalFlag, false);

    //    //        if (ed.SupportSurfaceInfo)
    //    //            effect.SetSurfaceInfo(water.Surface);

    //    //        if (water.Techniques != null)
    //    //            for (int i = 0; i < water.Techniques.Count; i++)
    //    //            {
    //    //                var tech = water.Techniques[i];
    //    //                tech.SaveState(effect);
    //    //                tech.SetValues(effect);
    //    //            }

    //    //        Matrix vp = view * projection;
    //    //        effect.SetValueByName("projTexM", vp * textureMtx);
    //    //        effect.SetValueByName("waterTexM", waterTexMtx);
    //    //        effect.SetFloat(waveHeight);
    //    //        effect.SetViewProjMatrix(vp);
    //    //        effect.SetEyePosition(eyePosition);
    //    //        effect.SetWorldMatrix(world);

    //    //        Effect _effect = effect.D3DEffect;
    //    //        var device = GEngine.Graphics;

    //    //        bool alpha = device.GetRenderState<bool>(RenderState.AlphaBlendEnable);
    //    //        device.SetRenderState(RenderState.AlphaBlendEnable, false);

    //    //        device.VertexDeclaration = water.VertexDescriptor.VertexDeclaration;

    //    //        int passes = _effect.Begin(0);
    //    //        for (int i = 0; i < passes; i++)
    //    //        {
    //    //            _effect.BeginPass(i);

    //    //            renderCallback();

    //    //            _effect.EndPass();
    //    //        }
    //    //        _effect.End();

    //    //        if (water.Techniques != null)
    //    //            for (int i = 0; i < water.Techniques.Count; i++)
    //    //                water.Techniques[i].RestoreState(effect);

    //    //        device.SetRenderState(RenderState.AlphaBlendEnable, alpha);
    //    //        if (noBegin)
    //    //            EndRender();
    //    //    }
    //    //}       
    //}

    //[Serializable]
    //public class ReflectiveTechnique : SceneNodeTechnique
    //{
    //    [NonSerialized]
    //    RenderTarget2DTex reflectionMap;

    //    [NonSerialized]
    //    RenderTarget2DTex refractionMap;

    //    [NonSerialized]
    //    RenderTarget2DTex mergeSrc;

    //    Matrix reflMatrix;

    //    [NonSerialized]
    //    EffectHandle modulate, additive, substractive;

    //    [NonSerialized]
    //    int reflecRegister;

    //    [NonSerialized]
    //    int refracRegister;
    //    private Format format;
    //    ShaderEffect effect;
    //    public ReflectiveTechnique()
    //        : base(true)
    //    {
    //        format = Format.X8R8G8B8;
    //        if (Engine.Lighting.HDR.Enable)
    //            format = Engine.Lighting.HDR.Render.HRDFormat;

    //        effect = ShaderStore.Water;
    //        modulate = effect.GetTechique("ModulateMerge");
    //        additive = effect.GetTechique("AdditiveMerge");
    //        substractive = effect.GetTechique("SubstractiveMerge");

    //        enable = true;
    //        reflMatrix.M11 = 1;
    //        reflMatrix.M22 = -1;
    //        reflMatrix.M33 = 1;
    //        reflMatrix.M44 = 1;
    //    }

    //    public ReflectiveTechnique(Water obj)
    //        : base(true)
    //    {
    //        format = Format.X8R8G8B8;
    //        if (Engine.Lighting.HDR.Enable )
    //            format = Engine.Lighting.HDR.Render.HRDFormat;

    //        node = obj;
    //        effect = ShaderStore.Water;
    //        modulate = effect.GetTechique("ModulateMerge");
    //        additive = effect.GetTechique("AdditiveMerge");
    //        substractive = effect.GetTechique("SubstractiveMerge");

    //        enable = true;
    //        reflMatrix.M11 = 1;
    //        reflMatrix.M22 = -1;
    //        reflMatrix.M33 = 1;
    //        reflMatrix.M44 = 1;
    //        reflMatrix.M42 = 2 * obj.Height;

    //        //
    //        //reflMatrix.Reflect(new Plane(0, 1, 0, -obj.Height));

    //        Initialize();
    //    }

    //    public override void Initialize()
    //    {
    //        Engine.Lighting.HDR.EnableChanged += new EventHandler(Lighting_HDREnableChanged);

    //        base.Initialize();
    //    }

    //    void Lighting_HDREnableChanged(object sender, EventArgs e)
    //    {
    //        format = Format.X8R8G8B8;
    //        if (Engine.Lighting.HDR.Enable)
    //        {
    //            format = Engine.Lighting.HDR.Render.HRDFormat;
    //        }

    //        OnLostDevice(Engine.Graphics);
    //        OnResetDevice(Engine.Graphics);
    //    }     

    //    protected override void OnLostDevice(Device device)
    //    {
    //        if (reflectionMap != null)
    //            reflectionMap.Dispose();
    //        if (refractionMap != null)
    //            refractionMap.Dispose();
    //        if (mergeSrc != null)
    //            mergeSrc.Dispose();
    //    }

    //    protected override void OnResetDevice(Device device)
    //    {
    //        Surface backBuffer = Engine.Graphics.GetRenderTarget(0);
    //        var sd = backBuffer.Description;
    //        backBuffer.Dispose();

    //        reflectionMap = new RenderTarget2DTex(device, sd.Width, sd.Height, format, Engine.PresentParams.AutoDepthStencilFormat, true);
    //        refractionMap = new RenderTarget2DTex(device, sd.Width, sd.Height, format, Engine.PresentParams.AutoDepthStencilFormat, true);
    //    }

    //    //public override IEngineComponent AttachedComponent
    //    //{
    //    //    get
    //    //    {
    //    //        return attachedCmp;
    //    //    }
    //    //    set
    //    //    {
    //    //        base.AttachedComponent = value;
    //    //        var water = (Water)attachedCmp;

    //    //        reflMatrix.M11 = 1;
    //    //        reflMatrix.M22 = -1;
    //    //        reflMatrix.M33 = 1;
    //    //        reflMatrix.M44 = 1;
    //    //        reflMatrix.M42 = 2 * water.Height;

    //    //        //reflMatrix.Reflect(new Plane(0, 1, 0, -water.Height));
    //    //    }
    //    //}

    //    public Texture ReflectionTexture { get { return reflectionMap.texTarget; } }
       
    //    public Texture RefractionTexture { get { return refractionMap.texTarget; } }
        
    //    public Texture MergeSrc { get { return mergeSrc.texTarget; } }

    //    protected override void Apply()
    //    {
    //        //if (enable && GEngine.Lighting.ReflectionEnable)
    //        //{
    //        //    Water water = (Water)node;
    //        //    reflMatrix.M42 = 2 * water.Height;
    //        //    //reflMatrix.Reflect(new Plane(0, 1, 0, -water.Height));

    //        //    GEngine.Lighting.ReflectionEnable = false;
    //        //    var scene = GEngine.Scene;
    //        //    var camera = scene.ActiveCamera;
    //        //    var device = GEngine.Graphics;
    //        //    var renderMgr = GEngine.RenderManager;              
    //        //    Matrix transInvViewProj = Matrix.Transpose(camera.InvViewProjection);
    //        //    TerrainRenderTechnique terrainRender = null;

    //        //    //if (scene.Terrain != null)
    //        //    //    terrainRender = (TerrainRenderTechnique)GEngine.RenderManager.GetRender(scene.Terrain.GetType());
    //        //    //Vector4 planeCoeff;

    //        //    RenderTarget oldRt = renderMgr.GetRenderTarget(0);

    //        //    #region Render Refraction

    //        //    //planeCoeff = new Vector4(0, -1, 0, water.Height);
    //        //    //planeCoeff = Vector4.Transform(planeCoeff, transInvViewProj);

    //        //    //if (terrainRender != null)
    //        //    //{
    //        //    //    terrainRender.ClipPlane = planeCoeff;
    //        //    //    terrainRender.ClipEnable = true;
    //        //    //}

    //        //    #region Caustic
    //        //    var caustics = water.Caustics;
    //        //    if (caustics != null && caustics.Length > 0)
    //        //    {
    //        //        if (mergeSrc == null || mergeSrc.Disposed)
    //        //        {
    //        //            mergeSrc = new RenderTarget2DTex(device, reflectionMap.Width, reflectionMap.Height, refractionMap.Format, refractionMap.DepthStencilFormat, true);
    //        //        }

    //        //        renderMgr.SetRenderTarget2D(0, mergeSrc);

    //        //        //device.SetClipPlane(0, new Plane(planeCoeff));
    //        //        device.SetRenderState(RenderState.ClipPlaneEnable, 1);

    //        //        device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, 0, 1, 0);

    //        //        scene.OnRender(x => x != water && x.CastRefraction);

    //        //        device.SetRenderState(RenderState.ClipPlaneEnable, 0);

    //        //        renderMgr.SetRenderTarget2D(0, refractionMap);

    //        //        switch (water.CausticMerge)
    //        //        {
    //        //            case CausticMergeType.Modulate:
    //        //                effect.Technique = modulate;
    //        //                break;
    //        //            case CausticMergeType.Additive:
    //        //                effect.Technique = additive;
    //        //                break;
    //        //            case CausticMergeType.Substractive:
    //        //                effect.Technique = substractive;
    //        //                break;
    //        //        }

    //        //        renderMgr.SetTexture(0, mergeSrc);
    //        //        renderMgr.Samplers[1].SetState(caustics[water.CurrentCaustic], TextureFilter.Linear, TextureAddress.Mirror);

    //        //        effect.SetValue("custicTexM", water.CausticTransformMatrix);

    //        //        device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, 0, 1, 0);
    //        //        var render = GEngine.RenderManager.QuadRender;
    //        //        render.Effect = effect;
    //        //        render.DrawToTexture(refractionMap.Width, refractionMap.Height);
    //        //    }
    //        //    #endregion

    //        //    else
    //        //    {
    //        //        renderMgr.SetRenderTarget2D(0, refractionMap);

    //        //        device.SetClipPlane(0, new Plane(planeCoeff));
    //        //        device.SetRenderState(RenderState.ClipPlaneEnable, 1);

    //        //        device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, 0, 1, 0);
    //        //        scene.OnRender(x => x != water && x.CastRefraction);

    //        //        device.SetRenderState(RenderState.ClipPlaneEnable, 0);
    //        //    }

    //        //    if (terrainRender != null)
    //        //        terrainRender.ClipEnable = false;

    //        //    #endregion

    //        //    #region Render Reflection

    //        //    var cull = device.GetRenderState(RenderState.CullMode);
    //        //    device.SetRenderState(RenderState.CullMode, Cull.None);
    //        //    //var world = scene.WorldMatrix;

    //        //    if (camera.Position.Y > water.Height)
    //        //    {
    //        //        reflMatrix.M42 = 2 * water.Height;
    //        //        scene.WorldMatrix = reflMatrix;
    //        //        planeCoeff = new Vector4(0, -1, 0, water.Height);
    //        //    }
    //        //    else
    //        //        planeCoeff = new Vector4(0, 1, 0, -water.Height);

    //        //    planeCoeff = Vector4.Transform(planeCoeff, transInvViewProj);
    //        //    if (terrainRender != null)
    //        //    {
    //        //        terrainRender.ClipPlane = planeCoeff;
    //        //        terrainRender.ClipEnable = true;
    //        //    }

    //        //    renderMgr.SetRenderTarget2D(0, reflectionMap);

    //        //    device.SetClipPlane(0, new Plane(planeCoeff));
    //        //    device.SetRenderState(RenderState.ClipPlaneEnable, 1);

    //        //    device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, 0, 1, 0);

    //        //    scene.OnRender(camera, x => x != water && x.CastReflection);

    //        //    scene.WorldMatrix = world;

    //        //    device.SetRenderState(RenderState.CullMode, cull);
    //        //    device.SetRenderState(RenderState.ClipPlaneEnable, 0);

    //        //    if (terrainRender != null)
    //        //        terrainRender.ClipEnable = false;

    //        //    #endregion

    //        //    renderMgr.SetRenderTarget(0, oldRt);
    //        //    GEngine.Lighting.ReflectionEnable = true;
    //        //}
    //    }       

    //    public override void SaveState(ShaderEffect effect)
    //    {
    //        reflecRegister = -1;
    //        refracRegister = -1;
    //        var ed = effect.Description;
    //        if (ed.ReflectionMapRegister >= 0 && ed.RefractionMapRegister >= 0)
    //        {
    //            var renderMgr = Engine.RenderManager;
    //            reflecRegister = ed.ReflectionMapRegister;
    //            refracRegister = ed.RefractionMapRegister;

    //            renderMgr.SaveSamplerState(reflecRegister);
    //            renderMgr.SaveSamplerState(refracRegister);
    //        }
    //    }

    //    public override void RestoreState(ShaderEffect effect)
    //    {
    //        if (refracRegister >= 0 && reflecRegister >= 0)
    //        {
    //            Engine.RenderManager.RestoreSamplerState(reflecRegister, true);
    //            Engine.RenderManager.RestoreSamplerState(refracRegister, true);
    //        }
    //    }

    //    public override void SetValues(ShaderEffect effect)
    //    {
    //        var ed = effect.Description;
    //        if (reflecRegister >= 0 && refracRegister >= 0)
    //        {
    //            var renderMgr = Engine.RenderManager;
    //            if (Engine.Lighting.Reflection.Enable)
    //            {
    //                renderMgr.Samplers[reflecRegister].SetState(reflectionMap.texTarget, TextureFilter.Linear, TextureAddress.Clamp);
    //                renderMgr.Samplers[refracRegister].SetState(refractionMap.texTarget, TextureFilter.Linear, TextureAddress.Clamp);
    //                //renderMgr.SetTexture(reflecRegister, reflectionMap);
    //                //renderMgr.SetTexture(refracRegister, refractionMap);
    //            }
    //            else
    //            {
    //                renderMgr.SetTexture(ed.ReflectionMapRegister, (BaseTexture)null);
    //                renderMgr.SetTexture(ed.RefractionMapRegister, (BaseTexture)null);
    //            }
    //        }
    //    }

    //    public override void CommitChanges()
    //    {
    //        var obj = (Water)node;
    //        reflMatrix.M11 = 1;
    //        reflMatrix.M22 = -1;
    //        reflMatrix.M33 = 1;
    //        reflMatrix.M44 = 1;
    //        reflMatrix.M42 = 2 * obj.Height;

    //        base.CommitChanges();

    //        //reflMatrix.Reflect(new Plane(0, 1, 0, -obj.Height));
    //    }

    //    protected override void OnDispose()
    //    {
    //        if (reflectionMap != null)
    //            reflectionMap.Dispose();
    //        if (refractionMap != null)
    //            refractionMap.Dispose();
    //        if (mergeSrc != null)
    //            mergeSrc.Dispose();

    //        base.OnDispose();
    //    }
    //}
}
