using System;
using System.Runtime.Serialization;

namespace Igneel.Assets
{
    [Serializable]
    public abstract class SerializableBase
    {
        [OnSerializing]
        private void _OnSerializing(StreamingContext context)
        {
            OnSerializing(context);
        }

        [OnSerialized]
        private void _OnSerialized(StreamingContext context)
        {
            OnSerialized(context);
        }

        [OnDeserializing]
        private void _OnDeserializing(StreamingContext context)
        {
            OnDeserializing(context);
        }

        [OnDeserialized]
        private void _OnDeserialized(StreamingContext context)
        {
            OnDeserialized(context);
        }

        protected virtual void OnSerializing(StreamingContext context) { }

        protected virtual void OnSerialized(StreamingContext context) { }

        protected virtual void OnDeserializing(StreamingContext context) { }

        protected virtual void OnDeserialized(StreamingContext context) { }

    }
}
