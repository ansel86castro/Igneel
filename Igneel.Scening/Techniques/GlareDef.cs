using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Igneel.Rendering
{
    //----------------------------------------------------------
    // Star generation

    // Define each line of the star.
    public struct STARLINE
    {
        public int Passes;
        public float SampleLength;
        public float Attenuation;
        public float Inclination;
    }
    // Simple definition of the star.
    struct STARDEF
    {
        public string Name;
        public int StarLines;
        public int Passes;
        public float SampleLength;
        public float Attenuation;
        public float Inclination;
        public bool Rotation;

        public STARDEF(string name, int starLine, int passes, float sampleLenght, float attenuation, float inclination, bool rotation)
        {
            this.Name = name;
            this.StarLines=starLine;
            this.Passes = passes;
            this.SampleLength = sampleLenght;
            this.Attenuation = attenuation;
            this.Inclination = inclination;
            this.Rotation = rotation;
        }
    }

    // Simple definition of the sunny cross filter
    struct StarDefSunnyCross
    {
        public string StarName;
        public float SampleLength;
        public float Attenuation;
        public float Inclination;
    }

    // Star form library
    public enum StarLibType
    {
        DISABLE = 0,
        CROSS,
        CROSSFILTER,
        SNOWCROSS,
        VERTICAL,      
        SUNNYCROSS,        
    }    

    public class StarDefinition
    {
        internal string name;
        internal STARLINE[] starLines;   // [m_nStarLines]
        internal float inclination;
        internal bool rotation;   // Rotation is available from outside ?

        public string Name { get { return name; } }
        public STARLINE[] Lines { get { return starLines; } }
        public float Inclination { get { return inclination; } set { inclination = value; } }
        public bool Rotation { get { return rotation; } set { rotation = value; } }

        static StarDefinition[] starLib = new StarDefinition[6];
        internal static Vector4[] chromaticAberrationColor = new Vector4[8];
        static STARDEF[] LibStarDef = new STARDEF[5]
        {
            // star     name           lines   passes  length    attn    rotate          bRotate
            new STARDEF("Disable"      ,0       ,0,       0.0f,   0.0f,   0,            false),
            new STARDEF( "Cross"       ,4       ,3,      1.0f,   0.85f,   0.0f,         true),
            new STARDEF( "CrossFilter" ,4       ,3,      1.0f,   0.95f,   0.0f,         true),
            new STARDEF("snowCross"    ,6       ,3,      1.0f,   0.96f, Numerics.ToRadians( 20.0f ), true),
            new STARDEF( "Vertical"    ,2       ,3,      1.0f,   0.96f,  0.0f,          false),
        };

        internal StarDefinition() { }

        internal StarDefinition(string name, int starLines, int passes, float sampleLength, float attenuation, float inclination,
                                bool rotation)
        {
            this.name = name;
            this.starLines = new STARLINE[starLines];
            this.inclination = inclination;
            this.rotation = rotation;

            float inc = Numerics.ToRadians(360.0f / (float)starLines);
            for (int i = 0; i < starLines; i++)
            {
                this.starLines[i].Passes = passes;
                this.starLines[i].SampleLength = sampleLength;
                this.starLines[i].Attenuation = attenuation;
                this.starLines[i].Inclination = inc * (float)i;
            }
        }
        internal StarDefinition(STARDEF starDef)
            : this(starDef.Name, starDef.StarLines, starDef.Passes, starDef.SampleLength, starDef.Attenuation, starDef.Inclination, starDef.Rotation) { }

        internal void Initialize(string name, int starLines, int passes, float sampleLength, float attenuation, float inclination,
                                bool rotation)
        {
            this.name = name;
            this.starLines = new STARLINE[starLines];
            this.inclination = inclination;
            this.rotation = rotation;

            float inc = Numerics.ToRadians(360.0f / (float)starLines);
            for (int i = 0; i < starLines; i++)
            {
                this.starLines[i].Passes = passes;
                this.starLines[i].SampleLength = sampleLength;
                this.starLines[i].Attenuation = attenuation;
                this.starLines[i].Inclination = inc * (float)i;
            }
        }

        internal void Initialize(STARDEF starDef)
        {
            Initialize(starDef.Name, starDef.StarLines, starDef.Passes, starDef.SampleLength, starDef.Attenuation, starDef.Inclination, starDef.Rotation);
        }

        internal void Initialize_SunnyCrossFilter(string name = "SunnyCross",
                                                float sampleLength = 1.0f,
                                                float attenuation = 0.88f,
                                                float longAttenuation = 0.95f,
                                                float inclination = 0)
        {
            this.name = name;
            this.inclination = inclination;
            this.starLines = new STARLINE[8];
            this.rotation = false;

            float inc = Numerics.ToRadians(360.0f / 8.0f);
            for (int i = 0; i < 8; i++)
            {
                starLines[i].SampleLength = sampleLength;
                starLines[i].Inclination = inc * (float)i;
                starLines[i].Passes = 3;
                if (0 == (i % 2))
                    starLines[i].Attenuation = longAttenuation;    // long                
                else
                    starLines[i].Attenuation = attenuation;
            }
        }

        internal static void InitializeStaticStarLibs()
        {
            // Create basic form
            for (int i = 0; i < 5; i++)
            {
                starLib[i] = new StarDefinition(LibStarDef[i]);                
            }
            starLib[5] = new StarDefinition();
            starLib[5].Initialize_SunnyCrossFilter();

            chromaticAberrationColor[0] = new Vector4(0.5f, 0.5f, 0.5f, 0.0f); // w
            chromaticAberrationColor[1] = new Vector4(0.8f, 0.3f, 0.3f, 0.0f);
            chromaticAberrationColor[2] = new Vector4(1.0f, 0.2f, 0.2f, 0.0f); // r
            chromaticAberrationColor[3] = new Vector4(0.5f, 0.2f, 0.6f, 0.0f);
            chromaticAberrationColor[4] = new Vector4(0.2f, 0.2f, 1.0f, 0.0f); // b
            chromaticAberrationColor[5] = new Vector4(0.2f, 0.3f, 0.7f, 0.0f);
            chromaticAberrationColor[6] = new Vector4(0.2f, 0.6f, 0.2f, 0.0f); // g
            chromaticAberrationColor[7] = new Vector4(0.3f, 0.5f, 0.3f, 0.0f);
        }
       
        /// Access to the star library
        public static StarDefinition GetLib(StarLibType type)
        {
            return starLib[(int)type];
        }

        public static Vector4 GetChromaticAberrationColor(int index)
        {
            return chromaticAberrationColor[index];
        }
    }

    //----------------------------------------------------------
    // Clare definition

    public enum GlareLibType
    {
        DISABLE = 0,
        CAMERA,
        NATURAL,
        CHEAPLENS,        
        FILTER_CROSSSCREEN,
        FILTER_CROSSSCREEN_SPECTRAL,
        FILTER_SNOWCROSS,
        FILTER_SNOWCROSS_SPECTRAL,
        FILTER_SUNNYCROSS,
        FILTER_SUNNYCROSS_SPECTRAL,
        CINECAM_VERTICALSLITS,
        CINECAM_HORIZONTALSLITS,
        USERDEF = -1,
        DEFAULT = FILTER_CROSSSCREEN,
    }
    // Simple glare definition
    struct GLAREDEF
    {
        public string Name;
        public float GlareLuminance;
        public float BloomLuminance;
        public float GhostLuminance;
        public float GhostDistortion;
        public float StarLuminance;
        public StarLibType StarType;
        public float StarInclination;
        public float ChromaticAberration;
        public float AfterimageSensitivity;    // Current weight
        public float AfterimageRatio;          // Afterimage weight
        public float AfterimageLuminance;

        public GLAREDEF(string Name,
                        float GlareLuminance,
                       float BloomLuminance,
                         float GhostLuminance,
                         float GhostDistortion,
                         float StarLuminance,
                         StarLibType StarType,
                         float StarInclination,
                         float ChromaticAberration,
                         float AfterimageSensitivity,    // Current weight
                         float AfterimageRatio,          // Afterimage weight
                         float AfterimageLuminance)
        {
            this.Name = Name;
            this.GlareLuminance= GlareLuminance;
            this.BloomLuminance = BloomLuminance;
            this.GhostLuminance = GhostLuminance;
            this.GhostDistortion = GhostDistortion;
            this.StarLuminance = StarLuminance;
            this.StarType = StarType;
            this.StarInclination =StarInclination;
            this.ChromaticAberration = ChromaticAberration;
            this.AfterimageSensitivity = AfterimageSensitivity;    // Current weight
            this.AfterimageRatio  = AfterimageRatio;          // Afterimage weight
            this.AfterimageLuminance= AfterimageLuminance;
        }
    }

    public class GlareDefinition
    {
        internal string name;
        internal float glareLuminance;     // Total glare intensity (not effect to "after image")
        internal float bloomLuminance;
        internal float ghostLuminance;
        internal float ghostDistortion;
        internal float starLuminance;
        internal float starInclination;
        internal float chromaticAberration;
        internal float afterimageSensitivity;  // Current weight
        internal float afterimageRatio;        // Afterimage weight
        internal float afterimageLuminance;
        internal StarDefinition starDef;

        public string Name { get { return name; } }
        public StarDefinition StarDefinition { get { return starDef; } }
        public float GlareLuminance { get { return glareLuminance; } set { glareLuminance = value; } }
        public float BloomLuminance { get { return bloomLuminance; } set { bloomLuminance = value; } }
        public float GhostLuminance { get { return ghostLuminance; } set { ghostLuminance = value; } }
        public float GhostDistortion { get { return ghostDistortion; } set { ghostDistortion = value; } }
        public float StarLuminance { get { return starLuminance; } set { starLuminance = value; } }
        public float StarInclination { get { return starInclination; } set { starInclination = value; } }
        public float ChromaticAberration { get { return chromaticAberration; } set { chromaticAberration = value; } }
        public float AfterimageSensitivity { get { return afterimageSensitivity; } set { afterimageSensitivity = value; } }
        public float AfterimageRatio { get { return afterimageRatio; } set { afterimageRatio = value; } }
        public float AfterimageLuminance { get { return afterimageLuminance; } set { afterimageLuminance = value; } }


        static GlareDefinition[] glareDefLib;
        static GLAREDEF[] glareDef = new GLAREDEF[12]{
                   //  glare name glare   bloom   ghost   distort star    star type  rotate   C.A     current after   ai lum
             new GLAREDEF("Disable",  0.0f,   0.0f,   0.0f,   0.01f,  0.0f, StarLibType.DISABLE, 0.0f, 0.5f,   0.00f,  0.00f,  0.0f),   // GLT_DISABLE
             new GLAREDEF( "Camera", 1.5f,   1.2f,   1.0f,   0.00f,  1.0f,  StarLibType.CROSS, 0.0f ,    0.5f,   0.25f,  0.90f,  1.0f),   // GLT_CAMERA
             new GLAREDEF("Natural Bloom", 1.5f,   1.2f,   0.0f,   0.00f,  0.0f,  StarLibType.DISABLE, 0.0f,    0.0f,   0.40f,  0.85f,  0.5f), // GLT_NATURAL
             new GLAREDEF("Cheap Lens Camera",1.25f,  2.0f,   1.5f,   0.05f,  2.0f,StarLibType.CROSS, 0.0f,    0.5f,   0.18f,  0.95f,  1.0f),   // GLT_CHEAPLENS    
             new GLAREDEF("Cross Screen Filter" , 1.0f,   2.0f,   1.7f,   0.00f,  1.5f,StarLibType.CROSSFILTER , Numerics.ToRadians ( 25.0f ), 0.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_CROSSSCREEN
             new GLAREDEF("Spectral Cross Filter",1.0f,   2.0f,   1.7f,   0.00f,  1.8f, StarLibType.CROSSFILTER, Numerics.ToRadians( 70.0f ),    1.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_CROSSSCREEN_SPECTRAL
             new GLAREDEF("Snow Cross Filter" ,1.0f,   2.0f,   1.7f,   0.00f,  1.5f ,StarLibType.SNOWCROSS, Numerics.ToRadians( 10.0f ),    0.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_SNOWCROSS
             new GLAREDEF("Spectral Snow Cross", 1.0f,   2.0f,   1.7f,   0.00f,  1.8f, StarLibType.SNOWCROSS, Numerics.ToRadians( 40.0f ),    1.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_SNOWCROSS_SPECTRAL
             new GLAREDEF("Sunny Cross Filter", 1.0f,   2.0f,   1.7f,   0.00f,  1.5f, StarLibType.SUNNYCROSS,0.0f,    0.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_SUNNYCROSS
             new GLAREDEF("Spectral Sunny Cross" ,1.0f,   2.0f,   1.7f,   0.00f,  1.8f,   StarLibType.SUNNYCROSS,Numerics.ToRadians( 45.0f ),    1.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_SUNNYCROSS_SPECTRAL
             new GLAREDEF("Cine Camera Vertical Slits", 1.0f,   2.0f,   1.5f,   0.00f,  1.0f, StarLibType.VERTICAL, Numerics.ToRadians( 90.0f ),    0.5f,   0.20f,  0.93f,  1.0f),   // GLT_CINECAM_VERTICALSLIT
            new GLAREDEF("Cine Camera Horizontal Slits",   1.0f,   2.0f,   1.5f,   0.00f,  1.0f,   StarLibType.VERTICAL,0.0f, 0.5f, 0.20f,  0.93f,  1.0f)   // GLT_CINECAM_HORIZONTALSLIT
        };


        static GlareDefinition()
        {
            InitializeStaticGlareLibs();
        }

        internal GlareDefinition(string name,
                                float glareLuminance,
                                float bloomLuminance,
                                float ghostLuminance,
                                float ghostDistortion,
                                float starLuminance,
                                StarLibType starType,
                                float starInclination,
                                float chromaticAberration,
                                float afterimageSensitivity,    // Current weight
                                float afterimageRatio,          // After Image weight
                                float afterimageLuminance) 
        {
            this.name = name;
            this.glareLuminance = glareLuminance;
            this.bloomLuminance = bloomLuminance;
            this.ghostLuminance = ghostLuminance;
            this.ghostDistortion = ghostDistortion;
            this.starLuminance = starLuminance;
            this.starInclination = starInclination;
            this.chromaticAberration = chromaticAberration;
            this.afterimageSensitivity = afterimageSensitivity;
            this.afterimageRatio = afterimageRatio;
            this.afterimageLuminance = afterimageLuminance;
            starDef = StarDefinition.GetLib(starType);
        }

        internal GlareDefinition(GLAREDEF glareDef)
            : this(glareDef.Name, glareDef.GlareLuminance, glareDef.BloomLuminance, glareDef.GhostLuminance, glareDef.GhostDistortion, glareDef.StarLuminance, glareDef.StarType, glareDef.StarInclination, glareDef.ChromaticAberration, glareDef.AfterimageSensitivity, glareDef.AfterimageRatio, glareDef.AfterimageLuminance) { }

        internal void Initialize(string name,
                                float glareLuminance,
                                float bloomLuminance,
                                float ghostLuminance,
                                float ghostDistortion,
                                float starLuminance,
                                StarLibType starType,
                                float starInclination,
                                float chromaticAberration,
                                float afterimageSensitivity,    // Current weight
                                float afterimageRatio,          // After Image weight
                                float afterimageLuminance)
        {
            this.name = name;
            this.glareLuminance = glareLuminance;
            this.bloomLuminance = bloomLuminance;
            this.ghostLuminance = ghostLuminance;
            this.ghostDistortion = ghostDistortion;
            this.starLuminance = starLuminance;
            this.starInclination = starInclination;
            this.chromaticAberration = chromaticAberration;
            this.afterimageSensitivity = afterimageSensitivity;
            this.afterimageRatio = afterimageRatio;
            this.afterimageLuminance = afterimageLuminance;
            starDef = StarDefinition.GetLib(starType);
        }

        internal void Initialize(GLAREDEF glareDef)
        {
            Initialize(glareDef.Name,
                       glareDef.GlareLuminance,
                       glareDef.BloomLuminance,
                       glareDef.GhostLuminance,
                       glareDef.GhostDistortion,
                       glareDef.StarLuminance,
                       glareDef.StarType,
                       glareDef.StarInclination,
                       glareDef.ChromaticAberration,
                       glareDef.AfterimageSensitivity,
                       glareDef.AfterimageRatio,
                       glareDef.AfterimageLuminance);
        }

        public static void InitializeStaticGlareLibs()
        {
            StarDefinition.InitializeStaticStarLibs();
            glareDefLib = new GlareDefinition[12];
            for (int i = 0; i < 12; i++)
            {
                glareDefLib[i] = new GlareDefinition(glareDef[i]);
            }
        }

        /// Access to the glare library
        public static GlareDefinition GetLib(GlareLibType type)
        {
            if ((int)type >= 0)
                return glareDefLib[(int)type];
            return null;
        }

    }

}
