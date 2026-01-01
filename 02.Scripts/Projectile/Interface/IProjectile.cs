public interface IProjectile
{
    void Fire(ProjectileContext ctx); // 생성 직후, 소유자가 호출
}
