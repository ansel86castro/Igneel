using System;
using System.Collections.Generic;
using System.Linq;

namespace Igneel.Rendering
{
    public class RenderManager    
    {
        private static Stack<TechniqueRegistry> _activeTechStack = new Stack<TechniqueRegistry>(10);
        private static Stack<TechniqueRegistry> _entries = new Stack<TechniqueRegistry>(10);       

        #region Renders

        /// <summary>
        /// Register a render of the component type T for the provided technique type
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="techniqueType">Technique type</param>
        /// <param name="render">Render instance</param>
        /// <remarks>
        /// A render and technique are not the same thing.A render is used for render the component geometry,
        /// and a technique is and algorithm for obtain a specifique effect. A technique will use several renders depending 
        /// on the presents components types in the scene. Also the renders and the techniques inheritates form IRenderTechnique
        /// the diference is that the renders inherit form IGeometryRender and the techniques even from IAttachableTechnique or IRenderTechnique
        /// </remarks>       
        public static void RegisterRender<TComp, TTech>(Render render)
            where TComp : class
            where TTech : Technique
        {
            var entries = TrCache<TTech>.RenderRegistries;
            var entry = entries.Find(x => x is RenderRegistry<TComp>);
            if (entry == null)
            {
                entry = new RenderRegistry<TComp>();
                entries.Add(entry);
            }
            entry.Render = render;
        }

        public static void RegisterRender<TComp, TTech, TRender>()
            where TComp : class
            where TTech : Technique
            where TRender : Render
        {
            var entries = TrCache<TTech>.RenderRegistries;
            var entry = entries.Find(x => x is RenderRegistry<TComp, TRender>);
            if (entry == null)
            {
                entry = new RenderRegistry<TComp, TRender>();
                entries.Add(entry);
            }

            entry.Render = null;
            entry.IsLazy = true;
        }

        public static Render RemoveRender<TComp, TTech>()
            where TTech : Technique
            where TComp : class
        {
            var entries = TrCache<TTech>.RenderRegistries;
            Render render = null;
            int index = entries.FindIndex(x => x is RenderRegistry<TComp>);
            if (index >= 0)
            {
                render = entries[index].Render;
                entries.RemoveAt(index);
            }

            return render;
        }

        public static void ClearRenders<TTech>()
            where TTech : Technique
        {
            TrCache<TTech>.RenderRegistries.Clear();
        }


        public static void ClearRenderStack<TComp>()
        {
            RenderStack<TComp>.Renders.Clear();
        }

        /// <summary>
        /// return the top register render for the type T of component
        /// </summary>
        /// <typeparam name="TComp"></typeparam>
        /// <returns></returns>
        public static Render GetRender<TComp>()
        {
            var stack = RenderStack<TComp>.Renders;
            return stack.Count > 0 ? stack.Peek() : null;
        }

        /// <summary>
        /// Push a render on the top of the render stack for the type T of component
        /// </summary>
        /// <typeparam name="TComp">Component Type</typeparam>
        /// <param name="render">the Render</param>
        public static void PushRender<TComp>(Render render)
        {
            var stack = RenderStack<TComp>.Renders;
            stack.Push(render);
        }

        /// <summary>
        /// remove the top render for the render stack of the type T of component
        /// </summary>
        /// <typeparam name="TComp">Component Type</typeparam>
        /// <returns>The top Render</returns>
        public static  Render PopRender<TComp>()
        {
            var stack = RenderStack<TComp>.Renders;
            return stack.Pop();
        }

        #endregion

        #region Techniques

        /// <summary>
        /// Gets the current render technique
        /// </summary>
        public static Technique ActiveTechnique
        {
            get { return _activeTechStack.Count == 0 ? null : _activeTechStack.Peek().Technique; }
        }

        /// <summary>
        /// Push a technique on top of the techniques stack
        /// </summary>
        /// <typeparam name="TTech"></typeparam>
        /// <param name="technique"></param>
        public static void PushTechnique<TTech>(Technique technique) where TTech : Technique
        {
            if (technique == null) throw new ArgumentNullException("technique");

            var entry = TrCache<TTech>.Registry;
            entry.technique = technique;
            _activeTechStack.Push(entry);

            foreach (var item in entry.Renders)
            {
                item.PushRender();
            }
        }

        public static void PushTechnique<TTech>(TTech technique)
            where TTech : Technique
        {
            if (technique == null) throw new ArgumentNullException("technique");

            var entry = TrCache<TTech>.Registry;
            entry.technique = technique;
            _activeTechStack.Push(entry);

            foreach (var item in entry.Renders)
            {
                item.PushRender();
            }
        }

        /// <summary>
        /// Updates the the render stack for the provided component type
        /// </summary>
        /// <typeparam name="TComp"></typeparam>
        public static void UpdateRenderStacks<TComp>()
              where TComp : class
        {
            foreach (var entry in _activeTechStack.Reverse())
            {
                foreach (var item in entry.Renders)
                {
                    if (item is RenderRegistry<TComp>)
                    {
                        item.PushRender();
                    }
                }
            }
        }

        /// <summary>
        /// Push a technique on top of the techniques stack. It uses the methods of Singleton for creating the 
        /// technique instance. If the technique can't be created the method will launch an exeption.
        /// </summary>
        /// <typeparam name="TTech"></typeparam>
        public static void PushTechnique<TTech>() where TTech : Technique
        {
            var entry = TrCache<TTech>.Registry;
            if (entry.technique == null || entry.technique.Disposed)
                entry.technique = Service.Require<TTech>();

            _activeTechStack.Push(entry);
            foreach (var item in entry.Renders)
            {
                item.PushRender();
            }
        }

        /// <summary>
        /// Push a technique on top of the techniques stack. The technique must have a defaut constructor
        /// this method is obsolete an will be removed in future releases.
        /// </summary>
        /// <typeparam name="TTech"></typeparam>
        /// 
        [Obsolete]
        public static void PushRequiredTechnique<TTech>() where TTech : Technique, new()
        {
            Technique.Require<TTech>();
            PushTechnique<TTech>();
        }

        /// <summary>
        /// Remove a technique fron the top of the render stack.
        /// It returns the techniques register.
        /// </summary>
        /// <typeparam name="TTech"></typeparam>
        /// <returns></returns>
        private static TechniqueRegistry _PopTechnique()
        {
            if (_activeTechStack.Count > 0)
            {
                var entry = _activeTechStack.Pop();
                foreach (var item in entry.Renders)
                {
                    item.PopRender();
                }
                return entry;
            }
            return null;
        }

        public static void PopTechnique()
        {
            if (_activeTechStack.Count > 0)
            {
                var entry = _activeTechStack.Pop();
                foreach (var item in entry.Renders)
                {
                    item.PopRender();
                }
            }
        }

        /// <summary>
        /// remove a techinques and all its renders
        /// </summary>
        /// <typeparam name="TTech"></typeparam>
        /// <returns></returns>
        public static TechniqueRegistry RemoveTechnique<TTech>() where TTech : Technique
        {
            if (_activeTechStack.Count > 0)
            {
                var entry = _activeTechStack.Peek();
                var techniqueType = entry.Technique.GetType();

                var type = typeof(TTech);

                if (techniqueType == type)
                {
                    return _PopTechnique();
                }
                else
                {
                    while (_activeTechStack.Count > 0)
                    {
                        entry = _activeTechStack.Peek();
                        techniqueType = entry.Technique.GetType();
                        if (techniqueType == type)
                            break;
                        else
                        {
                            _entries.Push(entry);

                            _activeTechStack.Pop();
                            foreach (var item in entry.Renders)
                            {
                                item.PopRender();
                            }
                        }
                    }

                    var popEntry = _PopTechnique();

                    while (_entries.Count > 0)
                    {
                        entry = _entries.Pop();
                        _activeTechStack.Push(entry);
                        foreach (var item in entry.Renders)
                        {
                            item.PushRender();
                        }
                    }

                    return popEntry;
                }
            }

            return null;
        }

        public static void InsertTechnique<TTech>(Predicate<Type> selectionSlotFunc, Technique technique = null)
            where TTech : Technique
        {
            if (_activeTechStack.Count == 0)
            {
                if (technique != null)
                    PushTechnique<TTech>(technique);
                else
                    PushTechnique<TTech>();
            }
            else
            {
                TechniqueRegistry entry;
                while (_activeTechStack.Count > 0)
                {
                    entry = _activeTechStack.Peek();
                    if (selectionSlotFunc(entry.Technique.GetType()))
                        break;
                    else
                    {
                        _entries.Push(entry);
                        _activeTechStack.Pop();
                        foreach (var item in entry.Renders)
                            item.PopRender();
                    }
                }

                if (technique != null)
                    PushTechnique<TTech>(technique);
                else
                    PushTechnique<TTech>();

                while (_entries.Count > 0)
                {
                    entry = _entries.Pop();
                    _activeTechStack.Push(entry);
                    foreach (var item in entry.Renders)
                        item.PushRender();
                }
            }
        }

        public static bool IsTechniqueActive(Type techniqueType)
        {
            return _activeTechStack.Count > 0 && _activeTechStack.Peek().Technique.GetType() == techniqueType;
        }

        public static bool IsTechniqueActive<T>() where T : Technique
        {
            return ActiveTechnique.GetType() == typeof(T);
        }

        /// <summary>
        /// apply the current render technique
        /// </summary>
        public static void ApplyTechnique()
        {
            if (_activeTechStack.Count > 0)
            {
                var tech = _activeTechStack.Peek().technique;
                if (tech != null && tech.Enable)
                    tech.Apply();
            }

        }

        public static void ApplyTechnique<TTech>() where TTech : Technique
        {
            PushTechnique<TTech>();

            var tech = _activeTechStack.Peek().technique;
            if (tech.Enable)
                tech.Apply();

            PopTechnique();
        }

        public static void ApplyTechnique<TTech>(TTech technique) where TTech : Technique
        {
            if (technique.Enable)
            {
                PushTechnique(technique);

                technique.Apply();

                PopTechnique();
            }
        }

        //public void GetRenders(Action<Render> callback)
        //{
        //    var registry = _activeTechStack.Peek();
        //    foreach (var render in registry.Renders)
        //    {
                
        //    }
        //}

        #endregion
    }
}
