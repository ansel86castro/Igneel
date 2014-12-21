using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Igneel.Graphics;

namespace Igneel.Components
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct Particle
    {
        public Vector3 GlobalPosition;
        public Vector3 GlobalVelocity;
        public float Mass;
        public float InvMass;
        public float RotationAngle;
        public float AngularVelocity;

        public float Size;
        public uint Color;
        public float Life;
        public float Alpha;
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ParticleVertex
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;

        [VertexElement(IASemantic.TextureCoordinate, 0)]
        public Vector2 TexCoord;

        [VertexElement(IASemantic.TextureCoordinate, 1)]
        public Vector3 PositionPtc;

        [VertexElement(IASemantic.TextureCoordinate, 2)]
        public float Alpha;

        [VertexElement(IASemantic.TextureCoordinate, 3)]
        public float Size;

        [VertexElement(IASemantic.Color)]
        public uint Color;

        public ParticleVertex(Vector3 position = default(Vector3), Vector2 texCoord = default(Vector2))
        {
            Position = position;
            TexCoord = texCoord;
            PositionPtc = new Vector3();
            Alpha = 0;
            Color = 0xFFFFFFFF;
            Size = 0;
        }

        public ParticleVertex(float x, float y, float z, float u, float v)
            :this(new Vector3(x,y,z), new Vector2(u,v))
        {
            
        }
    }

    public enum ParticleBlending { None, Additive, Substractive, Modulative, Transparent }

    public class ParticleSystem : GraphicObject<ParticleSystem>, INameable,IDynamic
    {
        private bool generateParticles;
        private int nbAliveParticles;
        private Vector3 globalForce;
        Particle[] particlesBuffer;
        Matrix globalPose;
        private string name;
        ParticleEmitter emitter;
        ParticleSystem next;
        float particleLifeSpand;
        int particlesPerFrame;
        int offset;
        private float particleSize;
        private float particleFadeIn;
        private float particleFadeOut;
        private float torque;

        GraphicBuffer vb;
        GraphicBuffer ib;
        ParticleVertex[] quads;
        private int nbQuads;
        VertexDescriptor vd;
        MeshMaterial material;

        private ParticleBlending blendingType;
        protected BlendOperation blendOp;
        protected Blend srcBlend;
        protected Blend destBlend;
        protected bool blendEnable;
        private float time;        

        public ParticleSystem(string name ,int nbParticles)
        {
            this.name = name;
            this.particlesBuffer = new Particle[nbParticles];

            InitParticles();
        }

        public ParticleSystem(string name, float particleLifeSpand, int particlesPerFrame , int fps)            
        {
            this.name = name;
            this.particleLifeSpand = particleLifeSpand;
            this.particlesPerFrame = particlesPerFrame;

            int particlesPerSecond = particlesPerFrame * fps;

            this.particlesBuffer = new Particle[(int)(particlesPerSecond * particleLifeSpand) + 2 * particlesPerFrame];

            InitParticles();
        }

        public string Name { get { return name; } set { name = value; } }

        public bool GenerateParticles { get { return generateParticles; } set { generateParticles = value; } }

        public int NbAliveParticles { get { return nbAliveParticles; } set { nbAliveParticles = value; } }

        public Vector3 GlobalForce { get { return globalForce; } set { globalForce = value; } }

        public float Torque { get { return torque; } set { torque = value; } }

        public float ParticleSize { get { return particleSize; } set { particleSize = value; } }

        public float ParticleFadeIn { get { return particleFadeIn; } set { particleFadeIn = value; } }

        public float ParticleFadeOut { get { return particleFadeOut; } set { particleFadeOut = value; } }

        public float ParticleLifeSpand { get { return particleLifeSpand; } set { particleLifeSpand = value; } }

        public ParticleEmitter Emitter { get { return emitter; } set { emitter = value; } }

        public ParticleSystem NextSystem { get { return next; } set { next = value; } }

        public MeshMaterial Material { get { return material; } set { material = value; } }

        public BlendOperation BlendOperation { get { return blendOp; } set { blendOp = value; } }

        public Blend SourceBlend { get { return srcBlend; } set { srcBlend = value; } }

        public Blend DestinationBlend { get { return destBlend; } set { destBlend = value; } }

        public bool BlendEnable { get { return blendEnable; } set { blendEnable = value; } }

        public GraphicBuffer QuadVertexBuffer { get { return vb; } }

        public GraphicBuffer Indices { get { return ib; } }

        public VertexDescriptor VertDescriptor { get { return vd; } }

        public ParticleBlending Blending
        {
            get { return blendingType; }
            set
            {
                blendingType = value;
                switch (blendingType)
                {
                    case ParticleBlending.None:
                        blendEnable = false;
                        srcBlend = Blend.One;
                        destBlend = Blend.Zero;
                        blendOp = BlendOperation.Add;
                        break;
                    case ParticleBlending.Additive:
                        blendOp = BlendOperation.Add;
                        srcBlend = Blend.SourceAlpha;
                        destBlend = Blend.One;
                        blendEnable = true;
                        break;
                    case ParticleBlending.Substractive:
                        blendOp = BlendOperation.ReverseSubtract;
                        srcBlend = Blend.SourceAlpha;
                        destBlend = Blend.One;
                        blendEnable = true;
                        break;
                    case ParticleBlending.Modulative:
                        blendOp = BlendOperation.Add;
                        srcBlend = Blend.Zero;
                        destBlend = Blend.SourceColor;
                        blendEnable = true;
                        break;
                    case ParticleBlending.Transparent:
                        blendEnable = true;
                        blendOp = BlendOperation.Add;
                        srcBlend = Blend.SourceAlpha;
                        destBlend = Blend.InverseSourceAlpha;
                        break;
                }
            }
        }

        public int NbQuads { get { return nbQuads; } set { nbQuads = value; } }

        private void InitParticles()
        {
            IsTransparent = true;
            nbQuads = particlesBuffer.Length;

            for (int i = 0; i < particlesBuffer.Length; i++)
            {
                particlesBuffer[i].Life = -1;
                particlesBuffer[i].InvMass = 1f;
                particlesBuffer[i].Mass = 1f;
                particlesBuffer[i].Color = 0xFFFFFFFF;
                particlesBuffer[i].Alpha = 0;
            }

            float size = 1;
            quads = new ParticleVertex[nbQuads * 4];
            vd = VertexDescriptor.GetDescriptor<ParticleVertex>();
            short[] indices = new short[nbQuads * 6]; ;

            for (int i = 0; i < nbQuads; i++)
            {
                // P0----P1
                // |   / |
                // | /   |
                // P3----P2
                quads[4 * i] = new ParticleVertex(-0.5f * size, 0.5f * size, 0, 0, 0);      //P0
                quads[4 * i + 1] = new ParticleVertex(0.5f * size, 0.5f * size, 0, 1, 0);   //P1
                quads[4 * i + 2] = new ParticleVertex(-0.5f * size, -0.5f * size, 0, 0, 1); //P3
                quads[4 * i + 3] = new ParticleVertex(0.5f * size, -0.5f * size, 0, 1, 1);  //P2

                indices[6 * i] = (short)(4 * i);
                indices[6 * i + 1] = (short)(4 * i + 1);
                indices[6 * i + 2] = (short)(4 * i + 2);

                indices[6 * i + 3] = (short)(4 * i + 2);
                indices[6 * i + 4] = (short)(4 * i + 1);
                indices[6 * i + 5] = (short)(4 * i + 3);
            }

            vb = Engine.Graphics.CreateVertexBuffer(ResourceUsage.Dynamic, CpuAccessFlags.Write, data:quads);
            ib =  Engine.Graphics.CreateIndexBuffer(data: indices);            
        }

        public override void OnPoseUpdated(SceneNode node)
        {
            this.globalPose = node.GlobalPose;
        }

        public unsafe void Update(float elapsedTime)
        {
            time += elapsedTime;
            Particle* ptc;
            Matrix pose = globalPose;

            fixed (Particle* pParticle = particlesBuffer)
            {               
                #region UpdateParticles                
            
                if (nbAliveParticles > 0)
                {
                    for (int i = 0, len = particlesBuffer.Length; i < len; i++)
                    {
                        ptc = pParticle + i;
                        if (ptc->Life >= 0)
                        {                            
                            if (ptc->Life > 1)
                            {
                                //kill the particle
                                ptc->Life = -1;
                                ptc->Size = 0;
                                ptc->Alpha = 0;

                                nbAliveParticles--;

                                //emit next particle in the chain
                                if (next != null)
                                    next.Emit(ptc);
                            }
                            else
                            {
                                //if particle is alive

                                Vector3 acceleration = globalForce * (1f / ptc->InvMass);

                                ptc->GlobalVelocity += acceleration * elapsedTime;
                                ptc->AngularVelocity += torque * elapsedTime;

                                ptc->GlobalPosition += ptc->GlobalVelocity * elapsedTime;
                                ptc->RotationAngle += ptc->AngularVelocity * elapsedTime;

                                if (ptc->Life <= particleFadeIn)
                                    ptc->Alpha = Numerics.Saturate(ptc->Life / particleFadeIn);
                                else if (ptc->Life >= particleFadeOut)
                                    ptc->Alpha = Numerics.Saturate((ptc->Life - particleFadeOut) / (1f - particleFadeOut));
                                else
                                    ptc->Alpha = 1f;

                                ptc->Life += particleLifeSpand > 0 ? elapsedTime / particleLifeSpand : 0;
                                ptc->Size = particleSize;

                            }
                        }
                    }
                }

                #endregion

                #region EmitParticles

                if (emitter != null && generateParticles && particlesPerFrame > 0)
                {
                    for (int i = 0; i < particlesPerFrame; i++)
                    {
                        ptc = pParticle + offset + i;
                        ptc->GlobalPosition = new Vector3();
                        ptc->GlobalVelocity = new Vector3();

                        emitter.Emit(ptc);                        

                        Vector3.Transform(ref ptc->GlobalPosition, ref pose, out ptc->GlobalPosition);
                        Vector3.TransformNormal(ref ptc->GlobalVelocity, ref pose, out ptc->GlobalVelocity);
                    }

                    offset = (offset + particlesPerFrame) % particlesBuffer.Length;
                    nbAliveParticles += particlesPerFrame;
                }

                #endregion
            }

            if (next != null)
                next.Update(elapsedTime);            
        }

        public unsafe void Emit(Particle* ptc)
        {
            emitter.Emit(ptc);

            ptc->GlobalPosition = Vector3.Transform(ptc->GlobalPosition, globalPose);
            ptc->GlobalVelocity = Vector3.TransformNormal(ptc->GlobalVelocity, globalPose);

            particlesBuffer[offset] = *ptc;

            offset = (offset + 1) % particlesBuffer.Length;
            nbAliveParticles++;
        }

        public unsafe int UpdateBuffer()
        {
            int quadIndex = 0;

            fixed (ParticleVertex* pQuads = quads)
            {
                fixed (Particle* pParticles = particlesBuffer)
                {
                    for (int i = 0, len = particlesBuffer.Length; i < len; i++)
                    {
                        int k = 4 * quadIndex++;
                        Particle* ptc = pParticles + i;
                        for (int j = 0; j < 4; j++)
                        {
                            ParticleVertex* vertex = pQuads + k + j;
                            vertex->PositionPtc = ptc->GlobalPosition;
                            vertex->Alpha = ptc->Alpha;
                            vertex->Color = ptc->Color;
                            vertex->Size = ptc->Size;
                        }
                    }
                }

                vb.Write(quads);                
            }
            
            return quadIndex;
        }        
    }
}
