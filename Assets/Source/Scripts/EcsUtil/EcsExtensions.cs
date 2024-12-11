using Leopotam.EcsLite;

namespace Source.Scripts.EcsUtil
{
    public static class EcsExtensions
    {
        public static ref T GetOrCreateRef<T>(this EcsPool<T> pool,int ent) where T:struct
        {
            return ref pool.Has(ent) ? ref pool.Get(ent) : ref pool.Add(ent);
        }
    }
}