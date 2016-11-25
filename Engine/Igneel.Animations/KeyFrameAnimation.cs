using Igneel.Assets;
using Igneel.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Assets.StorageConverters;

namespace Igneel.Animations
{
    public enum PlayingState
    {
        Stoped = 0,
        Playing = 1,
    };    

    
    //[ResourceActivator(typeof(KeyFrameAnimation.Activator))]
    [Asset("ANIMATION")]
    public partial class KeyFrameAnimation : Resource
    {           
        List<KeyFrameCursor> _cursors = new List<KeyFrameCursor>(1);
        ObservedDictionary<string, CurvesContainer> _nodes;        
        float _lastKeyValue = -1;

        public event Action<KeyFrameAnimation> AnimationStepEnd;

        public KeyFrameAnimation(string name)
            : base(name, null)
        {
            //Creates the Defaul cursor
            _cursors.Add(KeyFrameCursor.Create(this));

            _nodes = new ObservedDictionary<string, CurvesContainer>(
                itemAdded: x =>
                {
                    x.Animation = this;
                },
                itemRemoved: x =>
                {
                    if (x.Animation == this) x.Animation = null;
                },
                keySelector: x => x.Name);
        }
    
      
        public ObservedDictionary<string, CurvesContainer> Nodes { get { return _nodes; } }

        public float LastKeyValue
        {
            get { return _lastKeyValue < 0 ? _lastKeyValue = _GetMaxKeyValue() : _lastKeyValue; }
            protected set { _lastKeyValue = value; }
        }

        public bool HasEnded(int cursorIndex)
        {
            return GetCursor(cursorIndex).Time == LastKeyValue;
        }

        public KeyFrameCursor GetCursor(int cursorIndex) { return _cursors[cursorIndex]; }

        public int NbCursors { get { return _cursors.Count; } }

        public KeyFrameCursor CreateCursor()
        {
            var cursor = new KeyFrameCursor(this);
            _cursors.Add(cursor);
            return cursor;
        }

        public void Update(float elapsedTime, float blendWeight = 1.0f, bool blended = false)
        {
            Update(elapsedTime, _cursors[0], blendWeight, blended);
        }        
       
        public void Update(float elapsedTime, int stateIndex , float blendWeight = 1.0f, bool blended = false)
        {
            var state = _cursors[stateIndex];
            Update(elapsedTime, _cursors[stateIndex], blendWeight, blended);            
        }

        public void Update(float elapsedTime, KeyFrameCursor state, float blendWeight = 1.0f, bool blended = false)
        {
            state.ValidateTime();
            Sample(state.Time, blendWeight, blended);
            state.UpdateTime(elapsedTime);
        }

        public void Sample(float time, float blendWeight = 1.0f, bool blended = false)
        {
            foreach (var node in _nodes)
            {
                node.Sample(time, blendWeight, blended);
            }

            _OnAnimationStepEnd();
        }

        protected float _GetMaxKeyValue()
        {
            float lastKeyValue = _nodes[0].LastKeyValue;
            foreach (var node in _nodes)
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
   
        //[Serializable]
        //protected abstract class Activator : IResourceActivator
        //{
        //    List<Cursor> _states;            
        //    string _name;
        //    float _lastKey;

        //    public virtual void Initialize(IAssetProvider provider)
        //    {
        //        KeyFrameAnimation anim = (KeyFrameAnimation)provider;
        //        _name = anim._name;
        //        _states = anim._cursors;
        //        _lastKey = anim.LastKeyValue;
        //    }

        //    public IAssetProvider OnCreateResource()
        //    {
        //        KeyFrameAnimation  animation = new KeyFrameAnimation(_name);
        //        animation.LastKeyValue = _lastKey;
        //        animation._cursors = _states;
        //        foreach (var item in _states)
        //        {
        //            item.Animation = animation;
        //        }

        //        return animation;
        //    }            
        //}

        //public void OnRemoveFromScene(AnimationManager manager)
        //{
        //   manager.Animations.Remove(this);          
        //}

        //public void OnAddToScene(AnimationManager manager)
        //{           
        //    if (name == null)
        //        name = "Animation" + manager.Animations.Count;
        //    manager.Animations.Add(this);
        //}

        protected override void OnDispose(bool disposing)
        {
           
        }
    }
}
