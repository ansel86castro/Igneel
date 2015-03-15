using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    class MappingInfo { public Object Value; public bool Supported; }

    public struct EffectChangedEventArg
    {
        public Effect Current { get; set; }
        public Effect Previus { get; set; }
    }

    public abstract class Effect : ResourceAllocator
    {
        int counter;              
        static Effect current;
        static bool effectChanged;

        Dictionary<Type, MappingInfo> mappings = new Dictionary<Type, MappingInfo>();        
        Dictionary<string, ShaderVariable> variables = new Dictionary<string, ShaderVariable>();
        dynamic dynamicMap;

        Dictionary<string, int> techniquesLookup = new Dictionary<string, int>();
        EffectPass[][] techniques;
        int currentTech;
        string currentTechName;
        int lastPass;
        GraphicDevice device;

        public event EventHandler<EffectChangedEventArg> EffectHasChanged;

        public Effect(GraphicDevice device =null)
        {
            this.device = device ?? GraphicDeviceFactory.Device;        
            var techs = GetTechniques();
            if (techs != null)
                _SetTechniques(techs);
        }

        public GraphicDevice Device { get { return device; } }

        public static bool CurrentChanged { get { return effectChanged; } }

        public int Technique
        {
            get
            {
                return currentTech;
            }
            set
            {
                currentTech = value;
            }
        }

        public string TechniqueName
        {
            get { return currentTechName; }
            set
            {
                currentTech = techniquesLookup[value];
                currentTechName = value;
            }
        }

        public IReadOnlyDictionary<string, int> Techniques { get { return techniquesLookup; } }

        public dynamic U
        {
            get
            {
                if (dynamicMap == null)
                {
                    DynamicMapper mapper = new DynamicMapper { effect = this, techniques = techniques };
                    dynamicMap = mapper.CreateMapType();                    
                }
                return dynamicMap;
            }
        }

        private static void Using(Effect value)
        {
            effectChanged = current != value;
            if (effectChanged)
            {
                var previus = current;
                current = value;

                var ev = current.EffectHasChanged;
                if (ev != null)
                {
                    ev(current, new EffectChangedEventArg { Current = current, Previus = previus });
                }
            }
        }

        public InputLayout GetInputLayout(int technique, int pass)
        {
            return techniques[technique][pass].Program.InputDefinition;
        }

        public InputLayout GetInputLayout(int pass)
        {
            return techniques[currentTech][pass].Program.InputDefinition;
        }

        public EffectPass[] Passes(string technique)
        {
            this.currentTech = techniquesLookup[technique];
            return techniques[currentTech];
        }

        public EffectPass[] Passes(int technique)
        {
            currentTech = technique;
            return techniques[technique];
        }

        public EffectPass[] Passes()
        {
            return techniques[currentTech];
        }

        public void Apply(EffectPass pass)
        {
            Using(this);

            int passIndex = pass.index;
            if (passIndex > 0)
            {
                EffectPass previusPass = techniques[currentTech][passIndex - 1];
                previusPass.Clear(this, device);
            }

            pass.Apply(currentTech, device);
            lastPass = passIndex;
        }

        public void Apply(int passIndex)
        {
            EffectPass pass = techniques[currentTech][passIndex];
            Apply(pass);
        }

        public void ApplyPasses(Action<Effect, EffectPass> action, int technique = -1)
        {
            technique = technique >= 0 ? technique : currentTech;

            foreach (var pass in techniques[technique])
            {
                Apply(pass);
                action(this, pass);
            }

            EndPasses();
        }

        public void EndPasses()
        {
            var pass = techniques[currentTech][lastPass];
            pass.Clear(this, device);          
        }        

        public T Map<T>(bool fullmap = false) where T : class
        {          
            T value = null;
            MappingInfo map;
            if (mappings.TryGetValue(typeof(T), out map))
            {
                value = map.Supported ? (T)map.Value : null;
            }
            else
            {
                try
                {
                    DynamicMapper mapper = new DynamicMapper { effect = this, techniques = techniques };
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

                mappings.Add(typeof(T), map);
            }

            return value;
        }       
        
        public static Effect GetEffect<T>()
            where T:Effect
        {
            return Service.Require<T>();
        }
      
        internal ShaderVariable GetVariable(string name)
        {
            ShaderVariable v = null;
            this.variables.TryGetValue(name, out v);
            return v;
        }

        internal void SetVariable(string name, ShaderVariable v)
        {
            variables.Add(name, v);           
        }

        /// <summary>
        /// Derived classes must return an array containing the descriptions for the techniques
        /// </summary>
        /// <returns></returns>
        protected abstract TechniqueDesc[] GetTechniques();

        private void _SetTechniques(params TechniqueDesc[] techniques)
        {
            this.techniques = new EffectPass[techniques.Length][];
            for (int i = 0; i < techniques.Length; i++)
            {
                var tech = techniques[i];
                var effectPass = tech.Passes;
                techniquesLookup.Add(tech.Name, i);
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

                this.techniques[i] = passes;
                for (int j = 0; j < this.techniques[i].Length; j++)
                {
                    this.techniques[i][j].index = j;
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
                name = "t" + counter++;
            return new TechniqueDesc(device) { Name = name };
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
    }       
   
}
