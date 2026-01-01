using UnityEngine;
/// <summary>
/// Projectile 능력치 설정
/// </summary>
public enum ProjectileMoveType { Straight, Guided, Arc }

[CreateAssetMenu(menuName = "Projectile/Projectile Stats", fileName = "ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    public ProjectileMoveType moveType = ProjectileMoveType.Straight;
    
    public int damage = 5;
    public float speed = 12f;
    public float lifeTime = 5f;
    public float knockback = 0f;
    public int pierce = 0;             // 0 = 즉시 소멸
    
    public bool useGravity = false;
    public float gravityScale = 1f;
    
    public float turnRateDegPerSec = 360f;
    
    public string poolKey = "Projectile";  // 팩토리 반환용 키
}