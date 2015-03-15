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
        float waveHeight = 0.8f;
        float timeScale = 1f;
        float waveLenght = 0.1f;
        float time;
        Vector3 winDirection;
        Texture2D[] causticsTextures;
    }
}
