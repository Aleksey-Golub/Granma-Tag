namespace Assets.CodeBase.Infrastructure
{
    public interface IPauseManager : IPauseable
    {
        void Add(IPauseable pauseable);
        void Remove(IPauseable pauseable);
    }
}