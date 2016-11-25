using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Techniques;

namespace Igneel.Techniques
{

    public interface IHitTestRender
    {
        List<HitTestResult> HitTestResults { get; }

        void RenderObjects();

        bool FindResult(int abgr, out HitTestResult result);
    }

}
