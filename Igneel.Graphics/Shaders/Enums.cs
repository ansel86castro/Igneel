using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Flags]
    public enum ShaderFlags
    {
        AvoidFlowControl = 0x200,
        Debug = 1,
        EnableBackwardsCompatibility = 0x1000,
        EnableIEEEStrictness = 0x2000,
        EnableStrictness = 0x800,
        ForceSoftwarePixelShader = 0x80,
        ForceSoftwareVertexShader = 0x40,
        None = 0,
        NoPreshader = 0x100,
        OptimizationLevel0 = 0x4000,
        OptimizationLevel1 = 0,
        OptimizationLevel2 = 0xc000,
        OptimizationLevel3 = 0x8000,
        PackMatrixColumnMajor = 0x10,
        PackMatrixRowMajor = 8,
        PartialPrecision = 0x20,
        PreferFlowControl = 0x400,
        SkipOptimization = 4,
        SkipValidation = 2,
        WarningsAreErrors = 0x40000
    }


}
