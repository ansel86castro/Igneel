namespace Igneel.Rendering
{
    public interface ITechniqueBinding<T> :IRenderBinding<T>    
    {
        T LastBindedTechnique { get; }
    }

 

}
