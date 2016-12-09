namespace Igneel.Rendering.Presenters
{
    public class MultyViewPresenter:GraphicPresenter
    {
        GraphicPresenter[] _presenters;

        public GraphicPresenter[] Presenters
        {
            get { return _presenters; }
        }             

        public MultyViewPresenter(GraphicPresenter[] presenters)
        {
            this._presenters = presenters;
        }        

        protected override void Render()
        {
            for (int i = 0; i < _presenters.Length; i++)
            {
                 _presenters[i].RenderFrame();                
            }
        }

        public override void Resize(Size size)
        {
            foreach (var item in _presenters)
            {
                item.Resize(size);
            }
        }
    }
}
