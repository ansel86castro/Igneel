namespace Igneel.SceneManagement
{
    //[ResourceActivator(typeof(SceneManager.Activator))]
    //public class SceneManager:ResourceAllocator
    //{
    //    Igneel.Collections.ObservedDictionary<string, Scene> _scenes;
    //    Igneel.Collections.ObservedCollection<SceneState> _sceneStates;
    //    SceneState _currentState;
    //    static Scene _currentScene;

    //    public SceneManager()
    //    {
    //        _sceneStates = new Igneel.Collections.ObservedCollection<SceneState>(x =>
    //            {
    //                if (_currentState == null)
    //                {
    //                    _currentState = x;
    //                    x.SetActive();
    //                }
    //            }, null);

    //        _scenes = new ObservedDictionary<string, Scene>(x =>
    //        {
    //            if (Engine.Scene == null)
    //                Engine.Scene = x;
    //        }, x =>
    //            {
    //                List<SceneState> states = new List<SceneState>();
    //                foreach (var item in _sceneStates)
    //                {
    //                    if (item.Scene == x)
    //                        states.Add(item);
    //                    else
    //                    {
    //                        item.RemoveStates(x);
    //                    }
    //                }

    //                foreach (var item in states)
    //                    _sceneStates.Remove(item);
    //            },
    //            x => x.Name);
    //    }

    //    [AssetMember(storeAs: StoreType.Reference)]
    //    public SceneState ActiveSceneState
    //    {
    //        get { return _currentState; }
    //        set
    //        {
    //            _currentState = value;
    //            if (_currentState != null)
    //                _currentState.SetActive();
    //        }
    //    }

    //    [AssetMember(typeof(CollectionStoreConverter<Scene>))]
    //    public ObservedDictionary<string, Scene> Scenes { get { return _scenes; } }

    //    [AssetMember(typeof(CollectionStoreConverter<SceneState>))]
    //    public Igneel.Collections.ObservedCollection<SceneState> SceneStates { get { return _sceneStates; } }

    //    public static Scene Scene
    //    {
    //        get { return _currentScene; }
    //        set { _currentScene = value; }
    //    }

    //    public void UpdateSceneState()
    //    {
    //        if (_currentState == null && _sceneStates.Count > 0)
    //        {
    //            _currentState = _sceneStates[0];
    //            _currentState.SetActive();
    //        }

    //        if (_currentState != null)
    //        {
    //            foreach (var item in _currentState.States)
    //            {
    //                if (item.Trigger.IsActive())
    //                {
    //                    _currentState = item;
    //                    _currentState.SetActive();
    //                    break;
    //                }
    //            }
    //        }
    //    }
      
    //    protected override void OnDispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            //foreach (var item in scenes)
    //            //{
    //            //    item.Dispose();
    //            //}
    //            foreach (var item in _sceneStates)
    //            {
    //                item.Dispose();
    //            }
    //        }
    //    }
  
    //    public Asset CreateAsset()
    //    {
    //        return Asset.Create(this);
    //    }

    //    [Serializable]
    //    public class Activator:IResourceActivator
    //    {
    //        public void Initialize(IAssetProvider provider)
    //        {
               
    //        }

    //        public IAssetProvider OnCreateResource()
    //        {
    //            return Engine.SceneManager;
    //        }
    //    }
    //}
}
