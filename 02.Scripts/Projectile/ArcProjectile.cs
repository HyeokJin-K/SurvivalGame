using UnityEngine;

public class ArcProjectile : ProjectileBase
{
    [SerializeField] private float arcHeight = 2.0f;

    protected override void OnFire()
    {
        if (!fired)
        {
            fired = true;
            currLifeTime = 0f;

            rb.useGravity = true;
            rb.angularDamping = 0f;
            
            Vector3 origin = transform.position;
            Vector3 dest = target ? target.position : (origin + transform.forward * stats.speed);
            float desiredApex = Mathf.Max(arcHeight, (dest.y - origin.y) + 0.1f);

            Vector3 v0 = CalculateBallisticVelocity(origin, dest, desiredApex);
            rb.linearVelocity = v0;

            if (rb.linearVelocity.sqrMagnitude > 0.0001f)
                transform.rotation = Quaternion.LookRotation(rb.linearVelocity, Vector3.up);

            return;
        }

        currLifeTime += Time.fixedDeltaTime;
        if (stats && currLifeTime >= stats.lifeTime)
        {
            Despawn();
            return;
        }

        if (rb.linearVelocity.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity, Vector3.up);
    }

    private Vector3 CalculateBallisticVelocity(Vector3 origin, Vector3 target, float apexHeight)
    {
        float g = Mathf.Abs(Physics.gravity.y);
        Vector3 displacementXZ = new Vector3(target.x - origin.x, 0f, target.z - origin.z);
        float Vy = Mathf.Sqrt(2f * g * apexHeight);
        float tUp = Vy / g;
        float apexY = origin.y + apexHeight;
        float drop = Mathf.Max(0f, apexY - target.y);
        float tDown = Mathf.Sqrt(2f * drop / g);
        float T = tUp + tDown;
        Vector3 Vxz = displacementXZ / Mathf.Max(T, 0.0001f);
        return Vxz + Vector3.up * Vy;
    }
}