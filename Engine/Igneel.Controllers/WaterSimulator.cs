using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Techniques.NodeTechniques
{
    class WaterSimulator
    {
        float _waveHeight = 0.8f;
        float _timeScale = 1f;
        float _waveLenght = 0.1f;
        float _time;
        Vector3 _winDirection;
        Texture2D[] _causticsTextures;
    }
}
