namespace AliceHook.Models.Abstract
{
    public interface ICloneable<out T>
    {
        T Clone();
    }
}