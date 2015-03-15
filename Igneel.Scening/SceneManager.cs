using Igneel.Assets;
using Igneel.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Igneel.Scenering
{
    [ProviderActivator(typeof(SceneManager.Activator))]
    public class SceneManager:ResourceAllocator,IAssetProvider
    {
        Igneel.Collections.ObservedDictionary<string, Scene> scenes;
        Igneel.Collections.ObservedCollection<SceneState> sceneStates;
        SceneState currentState;
        static Scene currentScene;

        public SceneManager()
        {
            sceneStates = new Igneel.Collections.ObservedCollection<SceneState>(x =>
                {
                    if (currentState == null)
                    {
                        currentState = x;
                        x.SetActive();
                    }
                }, null);

            scenes = new ObservedDictionary<string, Scene>(x =>
            {
                if (SceneManager.Scene == null)
                    SceneManager.Scene = x;
            }, x =>
                {
                    List<SceneState> states = new List<SceneState>();
                    foreach (var item in sceneStates)
                    {
                        if (item.Scene == x)
                            states.Add(item);
                        else
                        {
                            item.RemoveStates(x);
                        }
                    }

                    foreach (var item in states)
                        sceneStates.Remove(item);
                },
                x => x.Name);
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public SceneState ActiveSceneState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                if (currentState != null)
                    currentState.SetActive();
            }
        }

        [AssetMember(typeof(CollectionStoreConverter<Scene>))]
        public ObservedDictionary<string, Scene> Scenes { get { return scenes; } }

        [AssetMember(typeof(CollectionStoreConverter<SceneState>))]
        public Igneel.Collections.ObservedCollection<SceneState> SceneStates { get { return sceneStates; } }

        public static Scene Scene
        {
            get { return currentScene; }
            set { currentScene = value; }
        }

        public void UpdateSceneState()
        {
            if (currentState == null && sceneStates.Count > 0)
            {
                currentState = sceneStates[0];
                currentState.SetActive();
            }

            if (currentState != null)
            {
                foreach (var item in currentState.States)
                {
                    if (item.Trigger.IsActive())
                    {
                        currentState = item;
                        currentState.SetActive();
                        break;
                    }
                }
            }
        }
      
        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                //foreach (var item in scenes)
                //{
                //    item.Dispose();
                //}
                foreach (var item in sceneStates)
                {
                    item.Dispose();
                }
            }
        }
  
        public Asset CreateAsset()
        {
            return Asset.Create(this);
        }

        [Serializable]
        public class Activator:IProviderActivator
        {
            public void Initialize(IAssetProvider provider)
            {
               
            }

            public IAssetProvider CreateInstance()
            {
                return Engine.SceneManager;
            }
        }
    }
}
