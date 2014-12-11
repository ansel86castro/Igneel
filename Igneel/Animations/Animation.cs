using Igneel.Assets;
using Igneel.Collections;
using Igneel.Components;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    public enum PlayingState
    {
        Stoped = 0,
        Playing = 1,
    };    

    [TypeConverter(typeof(Igneel.Design.DesignTypeConverter))]
    [ProviderActivator(typeof(Animation.Activator))]
    public partial class Animation : INameable, IAssetProvider,ISceneElement
    {       
        string name;
        AnimationManager manager;
        List<Cursor> cursors = new List<Cursor>(1);
        ObservedDictionary<string, AnimationNode> nodes;        
        float lastKeyValue = -1;

        public event Action<Animation> AnimationStepEnd;

        public Animation(string name)
        {                                    
            Cursor.Create(this);

           nodes = new ObservedDictionary<string, AnimationNode>(
               itemAdded: x =>
               {
                   x.Animation = this;
               },
               itemRemoved: x =>
               {
                   if (x.Animation == this) x.Animation = null;
               },
               keySelector: x => x.Name);

            var noti = Service.Get<INotificationService>();
            if (noti != null)
                noti.OnObjectCreated(this);
        }

        public AnimationManager Manager
        {
            get { return manager; }        
        }

        [AssetMember(typeof(CollectionStoreConverter<AnimationNode>))]
        public ObservedDictionary<string, AnimationNode> Nodes { get { return nodes; } }        

        [AssetMember]
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    manager.Animations.ChangeKey(name, value);
                }
                name = value;
            }
        }        

        public float LastKeyValue { get { return lastKeyValue < 0 ? lastKeyValue = _GetMaxKeyValue() : lastKeyValue; } protected set { lastKeyValue = value; } }

        public bool IsAnimationFinalized(int stateIndex) { return GetCursor(stateIndex).Time == LastKeyValue; }      

        public Cursor GetCursor(int index) { return cursors[index]; }

        public int NbCursors { get { return cursors.Count; } }

        public Cursor CreateCursor()
        {
            return new Cursor(this);
        }

        public void Update(float elapsedTime, Cursor state = null, float blendWeight = 1.0f, bool blended = false)
        {
            if (state == null)
                state = cursors[0];

            state.ValidateTime();
            Sample(state.Time, blendWeight, blended);
            state.UpdateTime(elapsedTime);
        }
       
        public void Update(float elapsedTime, int stateIndex , float blendWeight = 1.0f, bool blended = false)
        {
            var state = cursors[stateIndex];
            state.ValidateTime();
            Sample(state.Time, blendWeight, blended);
            state.UpdateTime(elapsedTime);
        }

        public void Sample(float time, float blendWeight = 1.0f, bool blended = false)
        {
            foreach (var node in nodes)
            {
                node.Sample(time, blendWeight, blended);
            }

            _OnAnimationStepEnd();
        }

        protected float _GetMaxKeyValue()
        {
            float lastKeyValue = nodes[0].LastKeyValue;
            foreach (var node in nodes)
            {
                lastKeyValue = Math.Max(lastKeyValue, node.LastKeyValue);
            }

            return lastKeyValue;
        }

        private void _OnAnimationStepEnd()
        {
            if (AnimationStepEnd != null)
                AnimationStepEnd(this);
        }        

        public Asset CreateAsset()
        {
            return Asset.Create(this, name);
        }

        public override string ToString()
        {
            return name ?? base.ToString();
        }            

        [Serializable]
        protected abstract class Activator : IProviderActivator
        {
            List<Cursor> states;            
            string name;
            float lastKey;

            public virtual void Initialize(IAssetProvider provider)
            {
                Animation anim = (Animation)provider;
                name = anim.name;
                states = anim.cursors;
                lastKey = anim.LastKeyValue;
            }

            public IAssetProvider CreateInstance()
            {
                Animation  animation = new Animation(name);
                animation.LastKeyValue = lastKey;
                animation.cursors = states;
                foreach (var item in states)
                {
                    item.Animation = animation;
                }

                return animation;
            }            
        }

        public void OnRemoveFromScene(Scene scene)
        {
            this.manager.Animations.Remove(this);
            this.manager = null;
        }

        public void OnAddToScene(Scene scene)
        {
            this.manager = scene.AnimManager;
            if (name == null)
                name = "Animation" + scene.AnimManager.Animations.Count;
            this.manager.Animations.Add(this);
        }
    }
}
