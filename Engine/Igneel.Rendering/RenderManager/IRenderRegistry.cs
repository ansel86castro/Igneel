namespace Igneel.Rendering
{
    interface IRenderRegistry
    {
        Render Render { get; set; }
        void PushRender();
        void PopRender();
        bool IsLazy { get; set; }

    }
}