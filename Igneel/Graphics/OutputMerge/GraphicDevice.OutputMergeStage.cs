using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Igneel.Rendering;

namespace Igneel.Graphics
{
    public struct OMInitialization
    {
        public SwapChain SwapChain;
        public int NbRenderTargets;
        public DepthStencil DepthBuffer;
        public BlendState BlendState;
        public DepthStencilState DepthTestState;
    }

    public abstract partial class GraphicDevice
    {        
        protected struct RenderStackValue
        {
            public RenderTarget target;
            public DepthStencil dephBuffer;
        }
        protected struct RenderStackValues
        {
            public RenderTarget[] targets;
            public DepthStencil dephBuffer;
            public int nbActiveRenderTargets;
        }

        protected RenderTarget _omBackBuffer;
        protected RenderTarget[] _omRenderTargets;
        protected DepthStencil _omDepthStencil;
        private Stack<RenderStackValue> renderTargetStack;
        private Stack<RenderStackValues> renderTargetsStack;
      

        private BlendState _omBlendState;
        private DepthStencilState _omDepthStencilStage;
        protected List<SwapChain> _swapChains = new List<SwapChain>();
        private ReadOnlyCollection<SwapChain> _swapChainsView;
        protected DepthStencil _omBackDepthStencil;
        protected SwapChain _swapChain0;
        int numRenderTargets;

        public RenderTarget BackBuffer { get { return _omBackBuffer; } }

        public DepthStencil BackDepthBuffer { get { return _omBackDepthStencil; } }

        public BlendState Blend
        {
            get { return _omBlendState; }
            set
            {
                if (value == null) throw new ArgumentNullException();

                if (_omBlendState != value)
                {                                        
                    OMSetBlendState(value);
                    _omBlendState = value;
                }
            }
        }

        public DepthStencilState DepthTest
        {
            get { return _omDepthStencilStage; }
            set
            {
                if (value == null) throw new ArgumentNullException();

                if (_omDepthStencilStage != value)
                {                                        
                    OMSetDepthStencilState(value);
                    _omDepthStencilStage = value;
                }
            }
        }

        public DepthStencil DepthBuffer
        {
            get { return _omDepthStencil; }          
        }

        public ReadOnlyCollection<SwapChain> SwapChains
        {
            get
            {
                return _swapChainsView = _swapChainsView ?? _swapChains.AsReadOnly();
            }
        }

        public SwapChain DefaultSwapChain
        {
            get { return _swapChain0; }
        }

        public int NbRenderTargets { get { return numRenderTargets; } }

        #region Protected 

        protected void InitOM()
        {
            OMInitialization init = GetOMInitialization();
            numRenderTargets = init.NbRenderTargets;            
            _omRenderTargets = new RenderTarget[init.NbRenderTargets];            
            if (init.SwapChain != null)
            {
                _swapChains.Add(init.SwapChain);
                _swapChain0 = init.SwapChain;
                _omBackBuffer = init.SwapChain.BackBuffer;
                _omRenderTargets[0] = _omBackBuffer;
            }                        

            _omBlendState = init.BlendState;
            _omDepthStencilStage = init.DepthTestState;

            _omDepthStencil = init.DepthBuffer;
            _omBackDepthStencil = init.DepthBuffer;

            renderTargetStack = new Stack<RenderStackValue>();
            renderTargetsStack = new Stack<RenderStackValues>();

            BlendStack.Push(_omBlendState);
            DepthStencilStack.Push(_omDepthStencilStage);
        }

        protected abstract OMInitialization GetOMInitialization();

        protected abstract void OMSetBlendState(BlendState state);

        protected abstract void OMSetDepthStencilState(DepthStencilState state);      

        protected abstract SwapChain CreateSwapChainImp(SwapChainDesc desc);

        #endregion                

        public abstract RenderTarget CreateRenderTarget(Texture2D texture, int subResource = 0, int count = 1);        

        public abstract DepthStencil CreateDepthStencil(DepthStencilDesc desc);
        
        public abstract BlendState CreateBlendState(BlendDesc desc);

        public abstract DepthStencilState CreateDepthStencilState(DepthStencilStateDesc desc);

        public DepthStencil CreateDepthStencil(int width, int height, Format format, Multisampling multisampling)
        {
            return CreateDepthStencil(new DepthStencilDesc(width, height, format, multisampling, false));
        }

        public SwapChain CreateSwapChain(SwapChainDesc desc)
        {
            var swapChain = CreateSwapChainImp(desc);
            _swapChains.Add(swapChain);
            swapChain.Device = this;

            return swapChain;
        }

        public RenderTarget GetRenderTarget(int slot)
        {
            return _omRenderTargets[slot];
        }

        public void GetRenderTargets(int startSlot, RenderTarget[] targets)
        {
            for (int i = 0 ; i < targets.Length; i++)
            {
                targets[i] = _omRenderTargets[startSlot + i];
            }
        }

        public void SetRenderTargets(int numTargets, RenderTarget[] renderTargets, DepthStencil dephtStencil)
        {          
            OMSetRenderTargetsImp(numTargets, renderTargets, dephtStencil);
            if (renderTargets == null)
            {
                Array.Clear(_omRenderTargets, 0, numTargets);                
            }
            else
            {
                ClrPlatform.Crl.CopyMemory(renderTargets, _omRenderTargets, numTargets);                
            }

            this._omDepthStencil = dephtStencil;
        }

        public void SetRenderTargets(int numTargets, RenderTarget[] renderTargets)
        {           
            OMSetRenderTargetsImp(numTargets, renderTargets, _omDepthStencil);

            if (renderTargets == null)
            {
                Array.Clear(_omRenderTargets, 0, numTargets);
            }
            else
            {
                ClrPlatform.Crl.CopyMemory(renderTargets, _omRenderTargets, numTargets);
            }
        }

        public void SetRenderTarget(RenderTarget renderTarget, DepthStencil dephtStencil)
        {           
            OMSetRenderTargetImpl(renderTarget, dephtStencil);

            _omRenderTargets[0] = renderTarget;
            _omDepthStencil = dephtStencil;
        }      

        public void SetRenderTarget(RenderTarget renderTarget)
        {            
            OMSetRenderTargetImpl(renderTarget, _omDepthStencil);
            _omRenderTargets[0] = renderTarget;
        }

        protected abstract void OMSetRenderTargetsImp(int numTargets, RenderTarget[] renderTargets, DepthStencil dephtStencil);

        protected abstract void OMSetRenderTargetImpl(RenderTarget renderTarget, DepthStencil dephtStencil);

        //public void OMPushRenderTarget(int slot, RenderTarget renderTarget, DepthStencil dephtStencil = null)
        //{
        //    RenderStackValue newValue = new RenderStackValue { target = renderTarget, dephBuffer = dephtStencil };
        //    renderTargetStack[slot].Push(newValue);

        //    OMSetRenderTarget(slot, renderTarget, dephtStencil ?? this._omDepthStencil);
        //}

        //public void OMPopRenderTarget(int slot)
        //{
        //    var value = renderTargetStack[slot].Pop();
        //    var old = renderTargetStack[slot].Peek();

        //    OMSetRenderTarget(slot, old.target, old.dephBuffer ?? _omDepthStencil);
        //}

        //public RenderStackValue OMGetRenderStackValue(int slot)
        //{
        //    return new RenderStackValue{ target = _omRenderTargets[slot], dephBuffer = _omDepthStencil };
        //}

        //public void OMSetRenderStackValue(int slot  , RenderStackValue v)
        //{
        //    OMSetRenderTarget(slot, v.target, v.dephBuffer);
        //}

        public void SaveRenderTarget()
        {
            var v = new RenderStackValue { target = _omRenderTargets[0], dephBuffer = _omDepthStencil };
            renderTargetStack.Push(v);
        }

        //public void OmSaveRenderTargets()
        //{
        //    RenderStackValues v = new RenderStackValues();
        //    for (v.nbActiveRenderTargets = 0; v.nbActiveRenderTargets < _omRenderTargets.Length; v.nbActiveRenderTargets++)
        //    {
        //        if (_omRenderTargets[v.nbActiveRenderTargets] == null)
        //            break;
        //    }
        //    v.targets = _omRenderTargets;
        //    v.dephBuffer = _omDepthStencil;
        //    renderTargetsStack.Push(v);
        //}

        public void RestoreRenderTarget()
        {
            if (renderTargetStack.Count > 0)
            {
                var v = renderTargetStack.Pop();
                SetRenderTarget(v.target, v.dephBuffer);
            }
        }

        //public void OMRestoreRenderTargets()
        //{
        //    if (renderTargetsStack.Count > 0)
        //    {
        //        var v = renderTargetsStack.Pop();
        //        OMSetRenderTargets(v.nbActiveRenderTargets, v.targets, v.dephBuffer);
        //    }
        //}


        protected virtual void DisposeOM()
        {
            foreach (var item in _swapChains)
            {
                item.Dispose();
            }

            if(_omDepthStencil!=null)
                _omDepthStencil.Dispose();

            if (_omBlendState != null)
                _omBlendState.Dispose();
            if (_omDepthStencilStage != null)
                _omDepthStencilStage.Dispose();
        }

        internal void RemoveSwapChain(SwapChain swapChain)
        {
            _swapChains.Remove(swapChain);
        }
    }

}
