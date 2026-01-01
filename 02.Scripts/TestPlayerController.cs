using System;
using System.Collections;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public float moveSpeed = 4f;
    public bool isHit = false;
    public bool isDead = false;
    public Material[] Materials;

    private void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 dir = new Vector3(h, 0, v).normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHP -= damage;
        StartCoroutine(HitRoutine());
        if (currentHP <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }

    IEnumerator HitRoutine()
    {
        if(isHit) yield break;

        isHit = true;
        this.GetComponent<MeshRenderer>().material = Materials[1];
        yield return new WaitForSeconds(0.05f);
        isHit = false;
        this.GetComponent<MeshRenderer>().material = Materials[0];
    }
}
