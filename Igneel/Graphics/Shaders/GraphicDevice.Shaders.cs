using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public struct ShadingInitialization
    {
        public ShaderStage VS { get; set; }
        public ShaderStage PS { get; set; }
        public ShaderStage GS { get; set; }
        public ShaderStage HS { get; set; }
        public ShaderStage DS { get; set; }
        public ShaderStage CS { get; set; }
    }

    public abstract partial class GraphicDevice
    {              
        private ShaderProgram program;        
        private ShaderStage vs ;
        private ShaderStage ps;
        private ShaderStage gs;
        private ShaderStage hs;
        private ShaderStage ds;
        private ShaderStage cs;

        static class StateStack<T>
        {
            public static Stack<T> states = new Stack<T>();
            internal static Action<T> setter;

            public static void Push(T state)
            {
                states.Push(state);
                setter(state);
            }

            public static T Pop()
            {
                var s = states.Pop();
                setter(states.Peek());
                return s;
            }

        }       

        public ShaderProgram Program 
        { 
            get { return program; }
            set
            {
                if (program != value)
                {
                    SetProgram(value);
                    program = value;
                    IAInputLayout = value.InputDefinition;
                }
            }
        }

        public ShaderStage VSStage { get { return vs; } }

        public ShaderStage PSStage { get { return ps; } }

        public ShaderStage GSStage { get { return gs; } }

        public ShaderStage HSStage { get { return hs; } }

        public ShaderStage DSStage { get { return ds; } }

        public ShaderStage CSStage { get { return cs; } }

        //public T GetShaderStage<T>()
        //    where T:ShaderStage
        //{
        //    return ShaderStages<T>.Stage;
        //}

        //protected static void RegisterStage<T>(T stage)
        //    where T:ShaderStage
        //{
        //    ShaderStages<T>.Stage = stage;
        //}

        protected void InitShading()
        {            
            var ini = GetShadingInitialization();
            vs = ini.VS;
            ps = ini.PS;
            gs = ini.GS;
            hs = ini.HS;
            ds = ini.DS;
            cs = ini.CS;
        }

        protected abstract ShadingInitialization GetShadingInitialization();        

        public abstract ShaderProgram CreateProgram(ShaderProgramDesc desc);              

        protected abstract void SetProgram(ShaderProgram program);

       //public abstract ShaderBuffer CreateShaderBuffer(BufferDesc desc);

        #region States

        public void PushGraphicState<T>(T state)            
        {
            StateStack<T>.Push(state);
        }

        public T PopGraphicState<T>()
        {
            return StateStack<T>.Pop();
        }

        #endregion           
    }    
   

}
