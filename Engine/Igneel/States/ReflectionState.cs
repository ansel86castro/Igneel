namespace Igneel.States
{
    public class ReflectionState:EnabilitableState
    {
        public ReflectionState()
        {
            EnvironmentMapSize = 128;
            EnvironmentMapZn = 1;
            EnvironmentMapZf = 1000;
        }

        
        public int EnvironmentMapSize { get; set; }

        
        public float EnvironmentMapZn { get; set; }

        
        public float EnvironmentMapZf { get; set; }

        
        public bool BdrEnable { get; set; }

        public bool UseDefaultTechnique { get; set; }
    }
}
