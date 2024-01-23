using Acoolaum.Game.Level;

namespace Acoolaum.Game.Hero
{
    public interface IHeroMovementStrategy : ITick
    {
        void OnStart();
        void OnGround(float groundPosition);
        void OnAir(float tickTime);
        void OnFinish();
    }
}