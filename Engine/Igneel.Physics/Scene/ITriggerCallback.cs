using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public interface ITriggerReport
    {
        void OnTrigger(ActorShape triggerShape, ActorShape otherShape, ShapeFlag status);
    }
}
