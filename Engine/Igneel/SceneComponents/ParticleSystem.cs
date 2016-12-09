using System;
using System.Runtime.InteropServices;
using Igneel.Graphics;
using Igneel.Components;
using Igneel.Components.Particles;
using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{
   
    public enum ParticleBlending { None, Additive, Substractive, Modulative, Transparent }

    public class ParticleSystem : GraphicObject<ParticleSystem>, INameable,IDynamic
    {
        private bool _generateParticles;
        private int _nbAliveParticles;
        private Vector3 _globalForce;
        Particle[] _particlesBuffer;
        Matrix _globalPose;
        private string _name;
        ParticleEmitter _emitter;
        ParticleSystem _next;
        float _particleLifeSpand;
        int _particlesPerFrame;
        int _offset;
        private float _particleSize;
        private float _particleFadeIn;
        private float _particleFadeOut;
        private float _torque;

        GraphicBuffer _vb;
        GraphicBuffer _ib;
        ParticleVertex[] _quads;
        private int _nbQuads;
        VertexDescriptor _vd;
        BasicMaterial _material;

        private ParticleBlending _blendingType;
        protected BlendOperation BlendOp;
        protected Blend SrcBlend;
        protected Blend DestBlend;
        protected bool blendEnable;
        private float _time;        

        public ParticleSystem(string name ,int nbParticles)
        {
            this._name = name;
            this._particlesBuffer = new Particle[nbParticles];

            InitParticles();
        }

        public ParticleSystem(string name, float particleLifeSpand, int particlesPerFrame , int fps)            
        {
            this._name = name;
            this._particleLifeSpand = particleLifeSpand;
            this._particlesPerFrame = particlesPerFrame;

            int particlesPerSecond = particlesPerFrame * fps;

            this._particlesBuffer = new Particle[(int)(particlesPerSecond * particleLifeSpand) + 2 * particlesPerFrame];

            InitParticles();
        }

        public string Name { get { return _name; } set { _name = value; } }

        public bool GenerateParticles { get { return _generateParticles; } set { _generateParticles = value; } }

        public int NbAliveParticles { get { return _nbAliveParticles; } set { _nbAliveParticles = value; } }

        public Vector3 GlobalForce { get { return _globalForce; } set { _globalForce = value; } }

        public float Torque { get { return _torque; } set { _torque = value; } }

        public float ParticleSize { get { return _particleSize; } set { _particleSize = value; } }

        public float ParticleFadeIn { get { return _particleFadeIn; } set { _particleFadeIn = value; } }

        public float ParticleFadeOut { get { return _particleFadeOut; } set { _particleFadeOut = value; } }

        public float ParticleLifeSpand { get { return _particleLifeSpand; } set { _particleLifeSpand = value; } }

        public ParticleEmitter Emitter { get { return _emitter; } set { _emitter = value; } }

        public ParticleSystem NextSystem { get { return _next; } set { _next = value; } }

        public BasicMaterial Material { get { return _material; } set { _material = value; } }

        public BlendOperation BlendOperation { get { return BlendOp; } set { BlendOp = value; } }

        public Blend SourceBlend { get { return SrcBlend; } set { SrcBlend = value; } }

        public Blend DestinationBlend { get { return DestBlend; } set { DestBlend = value; } }

        public bool BlendEnable { get { return blendEnable; } set { blendEnable = value; } }

        public GraphicBuffer QuadVertexBuffer { get { return _vb; } }

        public GraphicBuffer Indices { get { return _ib; } }

        public VertexDescriptor VertDescriptor { get { return _vd; } }

        public ParticleBlending Blending
        {
            get { return _blendingType; }
            set
            {
                _blendingType = value;
                switch (_blendingType)
                {
                    case ParticleBlending.None:
                        blendEnable = false;
                        SrcBlend = Blend.One;
                        DestBlend = Blend.Zero;
                        BlendOp = BlendOperation.Add;
                        break;
                    case ParticleBlending.Additive:
                        BlendOp = BlendOperation.Add;
                        SrcBlend = Blend.SourceAlpha;
                        DestBlend = Blend.One;
                        blendEnable = true;
                        break;
                    case ParticleBlending.Substractive:
                        BlendOp = BlendOperation.ReverseSubtract;
                        SrcBlend = Blend.SourceAlpha;
                        DestBlend = Blend.One;
                        blendEnable = true;
                        break;
                    case ParticleBlending.Modulative:
                        BlendOp = BlendOperation.Add;
                        SrcBlend = Blend.Zero;
                        DestBlend = Blend.SourceColor;
                        blendEnable = true;
                        break;
                    case ParticleBlending.Transparent:
                        blendEnable = true;
                        BlendOp = BlendOperation.Add;
                        SrcBlend = Blend.SourceAlpha;
                        DestBlend = Blend.InverseSourceAlpha;
                        break;
                }
            }
        }

        public int NbQuads { get { return _nbQuads; } set { _nbQuads = value; } }

        private void InitParticles()
        {
            IsTransparent = true;
            _nbQuads = _particlesBuffer.Length;

            for (int i = 0; i < _particlesBuffer.Length; i++)
            {
                _particlesBuffer[i].Life = -1;
                _particlesBuffer[i].InvMass = 1f;
                _particlesBuffer[i].Mass = 1f;
                _particlesBuffer[i].Color = 0xFFFFFFFF;
                _particlesBuffer[i].Alpha = 0;
            }

            float size = 1;
            _quads = new ParticleVertex[_nbQuads * 4];
            _vd = VertexDescriptor.GetDescriptor<ParticleVertex>();
            short[] indices = new short[_nbQuads * 6]; ;

            for (int i = 0; i < _nbQuads; i++)
            {
                // P0----P1
                // |   / |
                // | /   |
                // P3----P2
                _quads[4 * i] = new ParticleVertex(-0.5f * size, 0.5f * size, 0, 0, 0);      //P0
                _quads[4 * i + 1] = new ParticleVertex(0.5f * size, 0.5f * size, 0, 1, 0);   //P1
                _quads[4 * i + 2] = new ParticleVertex(-0.5f * size, -0.5f * size, 0, 0, 1); //P3
                _quads[4 * i + 3] = new ParticleVertex(0.5f * size, -0.5f * size, 0, 1, 1);  //P2

                indices[6 * i] = (short)(4 * i);
                indices[6 * i + 1] = (short)(4 * i + 1);
                indices[6 * i + 2] = (short)(4 * i + 2);

                indices[6 * i + 3] = (short)(4 * i + 2);
                indices[6 * i + 4] = (short)(4 * i + 1);
                indices[6 * i + 5] = (short)(4 * i + 3);
            }

            _vb = GraphicDeviceFactory.Device.CreateVertexBuffer(ResourceUsage.Dynamic, CpuAccessFlags.Write, data:_quads);
            _ib =  GraphicDeviceFactory.Device.CreateIndexBuffer(data: indices);            
        }

        public override void OnPoseUpdated(Frame node)
        {
            this._globalPose = node.GlobalPose;
        }

        public unsafe void Update(float elapsedTime)
        {
            _time += elapsedTime;
            Particle* ptc;
            Matrix pose = _globalPose;

            fixed (Particle* pParticle = _particlesBuffer)
            {               
                #region UpdateParticles                
            
                if (_nbAliveParticles > 0)
                {
                    for (int i = 0, len = _particlesBuffer.Length; i < len; i++)
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

                                _nbAliveParticles--;

                                //emit next particle in the chain
                                if (_next != null)
                                    _next.Emit(ptc);
                            }
                            else
                            {
                                //if particle is alive

                                Vector3 acceleration = _globalForce * (1f / ptc->InvMass);

                                ptc->GlobalVelocity += acceleration * elapsedTime;
                                ptc->AngularVelocity += _torque * elapsedTime;

                                ptc->GlobalPosition += ptc->GlobalVelocity * elapsedTime;
                                ptc->RotationAngle += ptc->AngularVelocity * elapsedTime;

                                if (ptc->Life <= _particleFadeIn)
                                    ptc->Alpha = Numerics.Saturate(ptc->Life / _particleFadeIn);
                                else if (ptc->Life >= _particleFadeOut)
                                    ptc->Alpha = Numerics.Saturate((ptc->Life - _particleFadeOut) / (1f - _particleFadeOut));
                                else
                                    ptc->Alpha = 1f;

                                ptc->Life += _particleLifeSpand > 0 ? elapsedTime / _particleLifeSpand : 0;
                                ptc->Size = _particleSize;

                            }
                        }
                    }
                }

                #endregion

                #region EmitParticles

                if (_emitter != null && _generateParticles && _particlesPerFrame > 0)
                {
                    for (int i = 0; i < _particlesPerFrame; i++)
                    {
                        ptc = pParticle + _offset + i;
                        ptc->GlobalPosition = new Vector3();
                        ptc->GlobalVelocity = new Vector3();

                        _emitter.Emit(ptc);                        

                        Vector3.Transform(ref ptc->GlobalPosition, ref pose, out ptc->GlobalPosition);
                        Vector3.TransformNormal(ref ptc->GlobalVelocity, ref pose, out ptc->GlobalVelocity);
                    }

                    _offset = (_offset + _particlesPerFrame) % _particlesBuffer.Length;
                    _nbAliveParticles += _particlesPerFrame;
                }

                #endregion
            }

            if (_next != null)
                _next.Update(elapsedTime);            
        }

        public unsafe void Emit(Particle* ptc)
        {
            _emitter.Emit(ptc);

            ptc->GlobalPosition = Vector3.Transform(ptc->GlobalPosition, _globalPose);
            ptc->GlobalVelocity = Vector3.TransformNormal(ptc->GlobalVelocity, _globalPose);

            _particlesBuffer[_offset] = *ptc;

            _offset = (_offset + 1) % _particlesBuffer.Length;
            _nbAliveParticles++;
        }

        public unsafe int UpdateBuffer()
        {
            int quadIndex = 0;

            fixed (ParticleVertex* pQuads = _quads)
            {
                fixed (Particle* pParticles = _particlesBuffer)
                {
                    for (int i = 0, len = _particlesBuffer.Length; i < len; i++)
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

                _vb.Write(_quads);                
            }
            
            return quadIndex;
        }        
    }
}
