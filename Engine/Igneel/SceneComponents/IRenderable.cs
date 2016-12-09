namespace Igneel.SceneComponents
{   
   
    public interface IDrawable
    {
        int RenderId { get; set; }

        void Draw();
    }
}
