using Igneel.Assets;
using Igneel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    public class AnimationManager
    {               
        ObservedDictionary<string, Animation> animations;
        static Dictionary<object, IAnimContext> contexts = new Dictionary<object, IAnimContext>();

        static AnimationManager()
        {
                        
        }

        public AnimationManager()
        {
            animations = new ObservedDictionary<string, Animation>(null,
               null, x => x.Name ?? (x.Name = "Animation" + animations.Count));
        }
        
        [AssetMember(typeof(CollectionStoreConverter<Animation>))]
        public ObservedDictionary<string, Animation> Animations { get { return animations; } }

        public static IAnimContext GetContext<T>(object target)
        {
            IAnimContext context;
            if (!contexts.TryGetValue(target, out context))
            {
                var fac = Service.GetFactory<IAnimContext<T>>();
                if (fac == null) throw new InvalidOperationException("no factory is provided");

                context = fac.CreateInstance();
                context.Target = target;
                contexts.Add(target, context);
            }
            return context;            
        }

        public static void RemoveContext(object target)
        {
            contexts.Remove(target);
        }

        public static bool ContainsContext(object target)
        {
            return contexts.ContainsKey(target);
        }

        public static void SetContext(IAnimContext context)
        {
            contexts[context.Target] = context;
        }
        
       
    }
}
