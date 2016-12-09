using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Igneel.Assets;

namespace Igneel.Rendering
{
    class MappingInfo { public Object Value; public bool Supported; }

    public struct EffectChangedEventArg
    {
        public Effect Current { get; set; }
        public Effect Previus { get; set; }
    }

    public abstract class Effect : Resource
    {
        int _counter;              
        static Effect _current;
        static bool _effectChanged;

        Dictionary<Type, MappingInfo> _mappinGS = new Dictionary<Type, MappingInfo>();        
        Dictionary<string, ShaderVariable> _variables = new Dictionary<string, ShaderVariable>();
        dynamic _dynamicMap;

        Dictionary<string, int> _techniquesLookup = new Dictionary<string, int>();
        EffectPass[][] _techniques;
        int _currentTech;
        string _currentTechName;
        int _lastPass;
        GraphicDevice _device;

        public event EventHandler<EffectChangedEventArg> EffectHasChanged;

        public Effect(GraphicDevice device)
        {
            this._device = device;        
            var techs = GetTechniques();
            if (techs != null)
                _SetTechniques(techs);
        }

        public GraphicDevice Device { get { return _device; } }

        public static bool CurrentChanged { get { return _effectChanged; } }

        public int Technique
        {
            get
            {
                return _currentTech;
            }
            set
            {
                _currentTech = value;
            }
        }

        public string TechniqueName
        {
            get { return _currentTechName; }
            set
            {
                _currentTech = _techniquesLookup[value];
                _currentTechName = value;
            }
        }

        public IReadOnlyDictionary<string, int> Techniques { get { return _techniquesLookup; } }

        public dynamic Input
        {
            get
            {
                if (_dynamicMap == null)
                {
                    DynamicMapper mapper = new DynamicMapper { Effect = this, Techniques = _techniques };
                    _dynamicMap = mapper.CreateMapType();                    
                }
                return _dynamicMap;
            }
        }

        private static void Using(Effect value)
        {
            _effectChanged = _current != value;
            if (_effectChanged)
            {
                var previus = _current;
                _current = value;

                var ev = _current.EffectHasChanged;
                if (ev != null)
                {
                    ev(_current, new EffectChangedEventArg { Current = _current, Previus = previus });
                }
            }
        }

        public InputLayout GetInputLayout(int technique, int pass)
        {
            return _techniques[technique][pass].Program.InputDefinition;
        }

        public InputLayout GetInputLayout(int pass)
        {
            return _techniques[_currentTech][pass].Program.InputDefinition;
        }

        public EffectPass[] Passes(string technique)
        {
            this._currentTech = _techniquesLookup[technique];
            return _techniques[_currentTech];
        }

        public EffectPass[] Passes(int technique)
        {
            _currentTech = technique;
            return _techniques[technique];
        }

        public EffectPass[] Passes()
        {
            return _techniques[_currentTech];
        }

        public void Apply(EffectPass pass)
        {
            Using(this);

            int passIndex = pass.Index;
            if (passIndex > 0)
            {
                EffectPass previusPass = _techniques[_currentTech][passIndex - 1];
                previusPass.Clear(this, _device);
            }

            pass.Apply(_currentTech, _device);
            _lastPass = passIndex;
        }

        public void Apply(int passIndex)
        {
            EffectPass pass = _techniques[_currentTech][passIndex];
            Apply(pass);
        }

        public void ApplyPasses(Action<Effect, EffectPass> action, int technique = -1)
        {
            technique = technique >= 0 ? technique : _currentTech;

            foreach (var pass in _techniques[technique])
            {
                Apply(pass);
                action(this, pass);
            }

            EndPasses();
        }

        public void EndPasses()
        {
            var pass = _techniques[_currentTech][_lastPass];
            pass.Clear(this, _device);          
        }        

        public T Map<T>(bool fullmap = false) where T : class
        {          
            T value = null;
            MappingInfo map;
            if (_mappinGS.TryGetValue(typeof(T), out map))
            {
                value = map.Supported ? (T)map.Value : null;
            }
            else
            {
                try
                {
                    DynamicMapper mapper = new DynamicMapper { Effect = this, Techniques = _techniques };
                    value = (T)mapper.CreateMapType(typeof(T), fullmap);
                    if (value != null)
                        map = new MappingInfo { Value = value, Supported = true };
                    else
                        map = new MappingInfo { Supported = false };
                }
                catch (InvalidMappingException)
                {
                    map = new MappingInfo { Supported = false };
                }

                _mappinGS.Add(typeof(T), map);
            }

            return value;
        }

        public static Effect GetEffect<T>(GraphicDevice device)
            where T : Effect
        {
            var effect = Service.Get<T>();
            if (effect == null)
            {
                effect = (T)Activator.CreateInstance(typeof(T), device);
                Service.Set(effect);
            }
            return effect;
        }
      
        internal ShaderVariable GetVariable(string name)
        {
            ShaderVariable v = null;
            this._variables.TryGetValue(name, out v);
            return v;
        }

        internal void SetVariable(string name, ShaderVariable v)
        {
            _variables.Add(name, v);           
        }

        /// <summary>
        /// Derived classes must return an array containing the descriptions for the techniques
        /// </summary>
        /// <returns></returns>
        protected abstract TechniqueDesc[] GetTechniques();

        private void _SetTechniques(params TechniqueDesc[] techniques)
        {
            this._techniques = new EffectPass[techniques.Length][];
            for (int i = 0; i < techniques.Length; i++)
            {
                var tech = techniques[i];
                var effectPass = tech.Passes;
                _techniquesLookup.Add(tech.Name, i);
                var passes = new EffectPass[effectPass.Count];
                for (int j = 0; j < passes.Length; j++)
                {
                    passes[j] = new EffectPass(GraphicDeviceFactory.Device.CreateProgram(effectPass[j].Program))
                    {
                        BlendState = effectPass[j].BlendState,
                        RState = effectPass[j].RState,
                        ZBufferState = effectPass[j].ZBufferState
                    };
                }

                this._techniques[i] = passes;
                for (int j = 0; j < this._techniques[i].Length; j++)
                {
                    this._techniques[i][j].Index = j;
                }
            }
        }

        //protected TechniqueDesc Tech(string name, params EffectPass[] effectPass)
        //{
        //    return new TechniqueDesc { Name = name, Passes = effectPass };
        //}
        protected TechniqueDesc Tech(string name = null)
        {
            if (name == null)
                name = "t" + _counter++;
            return new TechniqueDesc(_device) { Name = name };
        }

        //protected TechniqueDesc Tech<TVert>(string name = null) where TVert : struct
        //{
        //    if (name == null)
        //        name = "t" + counter++;
        //    return new TechniqueDesc<TVert>(device) { Name = name };
        //}                        

        public virtual void OnRender(Render render) { }

        public virtual void OnRenderCreated(Render render) { }

        public static void SetBinding<TEffect, TItem>(IRenderBinding<TItem> value)
            where TEffect :Effect
        {
            Binding<TItem, TEffect>.Value = value;
        }

        public static IRenderBinding<TItem> GetBinding<TEffect, TItem>(IRenderBinding<TItem> value)
            where TEffect : Effect
        {
            return Binding<TItem, TEffect>.Value;
        }

        public static void Bind<TEffect, TItem>(TItem item)
            where TEffect : Effect
        {
            var value = Binding<TItem, TEffect>.Value;
            if (value != null)
                value.Bind(item);
        }

        public static void UnBind<TEffect, TItem>(TItem item)
            where TEffect : Effect
        {
            var value = Binding<TItem, TEffect>.Value;
            if (value != null)
                value.UnBind(item);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }       
   
}
