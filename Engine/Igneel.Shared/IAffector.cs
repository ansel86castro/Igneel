namespace Igneel
{
    public interface IAffector : IPoseable
    {     
        IAffectable Affectable { get; set; }
    }
}