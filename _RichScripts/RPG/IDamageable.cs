namespace Explore
{
    public interface IDamageable
    {
        void TakeDamage(int damageAmount);
        //void TakeDamage(DamagePacket damagePacket, Entity damageSource);
    }
}