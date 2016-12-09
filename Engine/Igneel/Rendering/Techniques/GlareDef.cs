namespace Igneel.Techniques
{
    //----------------------------------------------------------
    // Star generation

    // Define each line of the star.
    public struct Starline
    {
        public int Passes;
        public float SampleLength;
        public float Attenuation;
        public float Inclination;
    }
    // Simple definition of the star.
    struct Stardef
    {
        public string Name;
        public int StarLines;
        public int Passes;
        public float SampleLength;
        public float Attenuation;
        public float Inclination;
        public bool Rotation;

        public Stardef(string name, int starLine, int passes, float sampleLenght, float attenuation, float inclination, bool rotation)
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
        Disable = 0,
        Cross,
        Crossfilter,
        Snowcross,
        Vertical,      
        Sunnycross,        
    }    

    public class StarDefinition
    {
        internal string name;
        internal Starline[] StarLines;   // [m_nStarLines]
        internal float inclination;
        internal bool rotation;   // Rotation is available from outside ?

        public string Name { get { return name; } }
        public Starline[] Lines { get { return StarLines; } }
        public float Inclination { get { return inclination; } set { inclination = value; } }
        public bool Rotation { get { return rotation; } set { rotation = value; } }

        static StarDefinition[] _starLib = new StarDefinition[6];
        internal static Vector4[] ChromaticAberrationColor = new Vector4[8];
        static Stardef[] _libStarDef = new Stardef[5]
        {
            // star     name           lines   passes  length    attn    rotate          bRotate
            new Stardef("Disable"      ,0       ,0,       0.0f,   0.0f,   0,            false),
            new Stardef( "Cross"       ,4       ,3,      1.0f,   0.85f,   0.0f,         true),
            new Stardef( "CrossFilter" ,4       ,3,      1.0f,   0.95f,   0.0f,         true),
            new Stardef("snowCross"    ,6       ,3,      1.0f,   0.96f, Numerics.ToRadians( 20.0f ), true),
            new Stardef( "Vertical"    ,2       ,3,      1.0f,   0.96f,  0.0f,          false),
        };

        internal StarDefinition() { }

        internal StarDefinition(string name, int starLines, int passes, float sampleLength, float attenuation, float inclination,
                                bool rotation)
        {
            this.name = name;
            this.StarLines = new Starline[starLines];
            this.inclination = inclination;
            this.rotation = rotation;

            float inc = Numerics.ToRadians(360.0f / (float)starLines);
            for (int i = 0; i < starLines; i++)
            {
                this.StarLines[i].Passes = passes;
                this.StarLines[i].SampleLength = sampleLength;
                this.StarLines[i].Attenuation = attenuation;
                this.StarLines[i].Inclination = inc * (float)i;
            }
        }
        internal StarDefinition(Stardef starDef)
            : this(starDef.Name, starDef.StarLines, starDef.Passes, starDef.SampleLength, starDef.Attenuation, starDef.Inclination, starDef.Rotation) { }

        internal void Initialize(string name, int starLines, int passes, float sampleLength, float attenuation, float inclination,
                                bool rotation)
        {
            this.name = name;
            this.StarLines = new Starline[starLines];
            this.inclination = inclination;
            this.rotation = rotation;

            float inc = Numerics.ToRadians(360.0f / (float)starLines);
            for (int i = 0; i < starLines; i++)
            {
                this.StarLines[i].Passes = passes;
                this.StarLines[i].SampleLength = sampleLength;
                this.StarLines[i].Attenuation = attenuation;
                this.StarLines[i].Inclination = inc * (float)i;
            }
        }

        internal void Initialize(Stardef starDef)
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
            this.StarLines = new Starline[8];
            this.rotation = false;

            float inc = Numerics.ToRadians(360.0f / 8.0f);
            for (int i = 0; i < 8; i++)
            {
                StarLines[i].SampleLength = sampleLength;
                StarLines[i].Inclination = inc * (float)i;
                StarLines[i].Passes = 3;
                if (0 == (i % 2))
                    StarLines[i].Attenuation = longAttenuation;    // long                
                else
                    StarLines[i].Attenuation = attenuation;
            }
        }

        internal static void InitializeStaticStarLibs()
        {
            // Create basic form
            for (int i = 0; i < 5; i++)
            {
                _starLib[i] = new StarDefinition(_libStarDef[i]);                
            }
            _starLib[5] = new StarDefinition();
            _starLib[5].Initialize_SunnyCrossFilter();

            ChromaticAberrationColor[0] = new Vector4(0.5f, 0.5f, 0.5f, 0.0f); // w
            ChromaticAberrationColor[1] = new Vector4(0.8f, 0.3f, 0.3f, 0.0f);
            ChromaticAberrationColor[2] = new Vector4(1.0f, 0.2f, 0.2f, 0.0f); // r
            ChromaticAberrationColor[3] = new Vector4(0.5f, 0.2f, 0.6f, 0.0f);
            ChromaticAberrationColor[4] = new Vector4(0.2f, 0.2f, 1.0f, 0.0f); // b
            ChromaticAberrationColor[5] = new Vector4(0.2f, 0.3f, 0.7f, 0.0f);
            ChromaticAberrationColor[6] = new Vector4(0.2f, 0.6f, 0.2f, 0.0f); // g
            ChromaticAberrationColor[7] = new Vector4(0.3f, 0.5f, 0.3f, 0.0f);
        }
       
        /// Access to the star library
        public static StarDefinition GetLib(StarLibType type)
        {
            return _starLib[(int)type];
        }

        public static Vector4 GetChromaticAberrationColor(int index)
        {
            return ChromaticAberrationColor[index];
        }
    }

    //----------------------------------------------------------
    // Clare definition

    public enum GlareLibType
    {
        Disable = 0,
        Camera,
        Natural,
        Cheaplens,        
        FilterCrossscreen,
        FilterCrossscreenSpectral,
        FilterSnowcross,
        FilterSnowcrossSpectral,
        FilterSunnycross,
        FilterSunnycrossSpectral,
        CinecamVerticalslits,
        CinecamHorizontalslits,
        Userdef = -1,
        Default = FilterCrossscreen,
    }
    // Simple glare definition
    struct Glaredef
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

        public Glaredef(string name,
                        float glareLuminance,
                       float bloomLuminance,
                         float ghostLuminance,
                         float ghostDistortion,
                         float starLuminance,
                         StarLibType starType,
                         float starInclination,
                         float chromaticAberration,
                         float afterimageSensitivity,    // Current weight
                         float afterimageRatio,          // Afterimage weight
                         float afterimageLuminance)
        {
            this.Name = name;
            this.GlareLuminance= glareLuminance;
            this.BloomLuminance = bloomLuminance;
            this.GhostLuminance = ghostLuminance;
            this.GhostDistortion = ghostDistortion;
            this.StarLuminance = starLuminance;
            this.StarType = starType;
            this.StarInclination =starInclination;
            this.ChromaticAberration = chromaticAberration;
            this.AfterimageSensitivity = afterimageSensitivity;    // Current weight
            this.AfterimageRatio  = afterimageRatio;          // Afterimage weight
            this.AfterimageLuminance= afterimageLuminance;
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
        internal StarDefinition StarDef;

        public string Name { get { return name; } }
        public StarDefinition StarDefinition { get { return StarDef; } }
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


        static GlareDefinition[] _glareDefLib;
        static Glaredef[] _glareDef = new Glaredef[12]{
                   //  glare name glare   bloom   ghost   distort star    star type  rotate   C.A     current after   ai lum
             new Glaredef("Disable",  0.0f,   0.0f,   0.0f,   0.01f,  0.0f, StarLibType.Disable, 0.0f, 0.5f,   0.00f,  0.00f,  0.0f),   // GLT_DISABLE
             new Glaredef( "Camera", 1.5f,   1.2f,   1.0f,   0.00f,  1.0f,  StarLibType.Cross, 0.0f ,    0.5f,   0.25f,  0.90f,  1.0f),   // GLT_CAMERA
             new Glaredef("Natural Bloom", 1.5f,   1.2f,   0.0f,   0.00f,  0.0f,  StarLibType.Disable, 0.0f,    0.0f,   0.40f,  0.85f,  0.5f), // GLT_NATURAL
             new Glaredef("Cheap Lens Camera",1.25f,  2.0f,   1.5f,   0.05f,  2.0f,StarLibType.Cross, 0.0f,    0.5f,   0.18f,  0.95f,  1.0f),   // GLT_CHEAPLENS    
             new Glaredef("Cross Screen Filter" , 1.0f,   2.0f,   1.7f,   0.00f,  1.5f,StarLibType.Crossfilter , Numerics.ToRadians ( 25.0f ), 0.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_CROSSSCREEN
             new Glaredef("Spectral Cross Filter",1.0f,   2.0f,   1.7f,   0.00f,  1.8f, StarLibType.Crossfilter, Numerics.ToRadians( 70.0f ),    1.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_CROSSSCREEN_SPECTRAL
             new Glaredef("Snow Cross Filter" ,1.0f,   2.0f,   1.7f,   0.00f,  1.5f ,StarLibType.Snowcross, Numerics.ToRadians( 10.0f ),    0.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_SNOWCROSS
             new Glaredef("Spectral Snow Cross", 1.0f,   2.0f,   1.7f,   0.00f,  1.8f, StarLibType.Snowcross, Numerics.ToRadians( 40.0f ),    1.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_SNOWCROSS_SPECTRAL
             new Glaredef("Sunny Cross Filter", 1.0f,   2.0f,   1.7f,   0.00f,  1.5f, StarLibType.Sunnycross,0.0f,    0.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_SUNNYCROSS
             new Glaredef("Spectral Sunny Cross" ,1.0f,   2.0f,   1.7f,   0.00f,  1.8f,   StarLibType.Sunnycross,Numerics.ToRadians( 45.0f ),    1.5f,   0.20f,  0.93f,  1.0f),   // GLT_FILTER_SUNNYCROSS_SPECTRAL
             new Glaredef("Cine Camera Vertical Slits", 1.0f,   2.0f,   1.5f,   0.00f,  1.0f, StarLibType.Vertical, Numerics.ToRadians( 90.0f ),    0.5f,   0.20f,  0.93f,  1.0f),   // GLT_CINECAM_VERTICALSLIT
            new Glaredef("Cine Camera Horizontal Slits",   1.0f,   2.0f,   1.5f,   0.00f,  1.0f,   StarLibType.Vertical,0.0f, 0.5f, 0.20f,  0.93f,  1.0f)   // GLT_CINECAM_HORIZONTALSLIT
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
            StarDef = StarDefinition.GetLib(starType);
        }

        internal GlareDefinition(Glaredef glareDef)
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
            StarDef = StarDefinition.GetLib(starType);
        }

        internal void Initialize(Glaredef glareDef)
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
            _glareDefLib = new GlareDefinition[12];
            for (int i = 0; i < 12; i++)
            {
                _glareDefLib[i] = new GlareDefinition(_glareDef[i]);
            }
        }

        /// Access to the glare library
        public static GlareDefinition GetLib(GlareLibType type)
        {
            if ((int)type >= 0)
                return _glareDefLib[(int)type];
            return null;
        }

    }

}
