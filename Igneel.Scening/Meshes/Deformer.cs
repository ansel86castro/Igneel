using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
    public abstract class Deformer : ResourceAllocator, IDynamic, IAssetProvider
    {
        SceneNode node;

        public event UpdateEventHandler UpdateEvent;

        public SceneNode Node
        {
            get { return node; }
            set { node = value; }
        }

        public virtual void Update(SceneNode sceneNode)
        {
            node = sceneNode;  
        }

        public virtual void Update(float elapsedTime)
        {
            if (UpdateEvent != null)
                UpdateEvent(this, elapsedTime);
        }

        public virtual Asset CreateAsset()
        {
            return Asset.Create(this);
        }

        public virtual void OnAssetDestroyed(AssetReference assetRef) { }
    }   


}
