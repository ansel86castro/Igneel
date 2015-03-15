using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Scenering
{
    public interface ISceneStateTrigger:IAssetProvider
    {
        bool IsActive();
    }

    public class SceneState: ResourceAllocator, IAssetProvider
    {
        Scene scene;
        ISceneStateTrigger trigger;
        List<SceneState> states = new List<SceneState>();

        public SceneState() { }

        public SceneState(Scene scene, ISceneStateTrigger trigger)
        {
            this.scene = scene;
            this.trigger = trigger;
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public Scene Scene { get { return scene; } set { scene = value; } }
        
        [AssetMember(storeAs: StoreType.Reference)]
        public ISceneStateTrigger Trigger { get { return trigger; } set { trigger = value; } }
        
        [AssetMember(typeof(CollectionStoreConverter<SceneState>))]
        public List<SceneState> States { get { return states; } }

        public void SetActive()
        {
            SceneManager.Scene = scene;
        }

        public bool RemoveStates(Scene scene)
        {
            List<SceneState>removeStates=new List<SceneState>();
            bool result = false;
            foreach (var item in states)
            {
                if (item.scene == scene)
                {
                    result = true;
                    removeStates.Add(item);
                }
                else
                {
                   var r= item.RemoveStates(scene);
                   if (!result)
                       result = r;
                }
            }

            foreach (var item in removeStates)
            {
                states.Remove(item);
            }
            return result;
        }      

        public Asset CreateAsset()
        {
            return Asset.Create(this);
        }
    }
       
}
