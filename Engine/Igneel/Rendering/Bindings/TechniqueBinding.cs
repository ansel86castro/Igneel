namespace Igneel.Rendering.Bindings
{
    public abstract class TechniqueBinding<T> : RenderBinding<T>, ITechniqueBinding<T>
         where T : FrameTechnique
    {
        T _lastTechnique;

        public T LastBindedTechnique
        {
            get { return _lastTechnique; }
        }

        public override sealed void OnBind(T value)
        {
            if (_lastTechnique != value)
            {
                _lastTechnique = value;
                OnTechBind(value);
            }
        }

        public override void OnUnBind(T value)
        {
            if (value.NbEntries == 0)
            {
                OnTechUnBind(value);
                _lastTechnique = null;
            }
        }

        protected abstract void OnTechBind(T value);

        protected abstract void OnTechUnBind(T value);
    }
}
