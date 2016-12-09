namespace Igneel
{
    public interface IPoseable
    {
        /// <summary>
        /// World Tranfrom of the Object
        /// </summary>
        Matrix GlobalPose { get; }
    }
}