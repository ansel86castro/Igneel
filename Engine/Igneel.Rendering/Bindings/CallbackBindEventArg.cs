namespace Igneel.Rendering
{
    public struct CallbackBindEventArg<TItem, TMap>
    {
        public TItem Value;
        public TMap Map;

        public CallbackBindEventArg(TItem value, TMap map)
        {          
            this.Value = value;
            this.Map = map;
        }
    }
}