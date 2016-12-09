using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{   
    public enum FillMode:int
    {
        Wireframe = 2,
        Solid = 3        
    }

    public enum CullMode:int
    {                
        None = 1,
        Front = 2,
        Back = 3
    }

    public enum Blend:int
    {
        Zero = 1,
        One = 2,
        SourceColor = 3,        
        InverseSourceColor = 4,
        SourceAlpha = 5,
        InverseSourceAlpha = 6,
        DestinationAlpha = 7,
        InverseDestinationAlpha = 8,
        DestinationColor = 9,
        InverseDestinationColor = 10,
        SourceAlphaSaturated = 11,                
        BlendFactor = 14,    
        InverseBlendFactor = 15,
        SourceColor1 = 16,
        InverseSourceColor1 = 17,
        SourceColor1Alpha = 18,          
        InverseSourceColor1Alpha = 19
    }

    public enum BlendOperation:int
    {
        Add = 1,
        Subtract = 2,
        ReverseSubtract = 3,
        Minimum = 4,        
        Maximum = 5        
    } 

}
