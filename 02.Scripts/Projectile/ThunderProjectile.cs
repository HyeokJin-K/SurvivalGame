using System;
using System.Collections;
using UnityEngine;

public class ThunderProjectile : ProjectileBase
{
    [SerializeField] private GameObject[] thunderParticles;
    // ThunderProjectile은 빈 게임 오브젝트(Root)가 가짐
    // 파티클 두 개를 자식 오브젝트로 둠

    private Vector3 firstPos, lastPos;   // 마법 실행 시 플레이어의 초기 위치
    
    private float _castTime = 2f;
    private bool isCast;

    void Start()
    {
        AllOff();

        firstPos = target.transform.position;
    }
    
    protected override void OnFire()
    {
        if(!isCast)
            StopCoroutine(CastRoutine());
    }

    private void AllOff()
    {
        for (int i = 0; i < thunderParticles.Length; i++)
        {
            thunderParticles[i].SetActive(false);
        }
    }

    IEnumerator CastRoutine()
    {
        isCast = true;
        
        thunderParticles[0].transform.position = firstPos;
        thunderParticles[0].SetActive(true);
        
        lastPos = thunderParticles[0].transform.position;
        yield return new WaitForSeconds(_castTime);
        
        thunderParticles[1].transform.position = lastPos;
        thunderParticles[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        isCast = false;
    }

    void OnCast()
    {
        // 마법 준비 과정
        // 마법진 파티클 깔아두기
        
    }

    void OnAttack()
    {
        // 공격 마법 실행
        // 마법진 중앙으로 번개가 내리침
    }
}
