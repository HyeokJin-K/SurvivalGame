using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class FocusCutScene : MonoBehaviour
{
    [Header("카메라 참조")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera tutorialFocusCamera;
    
    [Header("타겟 설정")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform tutorialTarget; // 게임 시작시 보여줄 타겟
    
    [Header("튜토리얼 설정")]
    [SerializeField] private float focusDuration = 2f;
    [SerializeField] private bool autoStartOnBegin = true;
    
    private bool isInTutorialMode = false;

    private void Start()
    {
        // 초기 카메라 설정
        playerCamera.Target.TrackingTarget = player;
        playerCamera.Target.LookAtTarget = player;
        playerCamera.Priority.Value = 10;
        
        tutorialFocusCamera.Priority.Value = 9;
        
        // 게임 시작시 자동으로 튜토리얼 연출 시작
        if (autoStartOnBegin && tutorialTarget != null)
        {
            StartCoroutine(TutorialFocusSequence(tutorialTarget, focusDuration));
        }
    }

    /// <summary>
    /// 튜토리얼 타겟에 포커스
    /// </summary>
    public void FocusOnTutorialTarget(Transform target)
    {
        if (!isInTutorialMode)
        {
            StartCoroutine(TutorialFocusSequence(target, focusDuration));
        }
    }

    /// <summary>
    /// 커스텀 시간으로 포커스
    /// </summary>
    public void FocusOnTutorialTarget(Transform target, float customDuration)
    {
        if (!isInTutorialMode)
        {
            StartCoroutine(TutorialFocusSequence(target, customDuration));
        }
    }

    private IEnumerator TutorialFocusSequence(Transform target, float duration)
    {
        isInTutorialMode = true;
        
        Debug.Log("튜토리얼 시작: 타겟으로 이동");
        
        // 포커스 카메라 타겟 설정
        tutorialFocusCamera.Target.TrackingTarget = target;
        tutorialFocusCamera.Target.LookAtTarget = target;
        
        // 포커스 카메라 활성화
        tutorialFocusCamera.Priority.Value = 11;
        playerCamera.Priority.Value = 10;
        
        // 블렌드 시간 대기
        yield return new WaitForSeconds(1.5f);
        
        Debug.Log("타겟을 비추는 중...");
        
        // 타겟을 비추는 시간
        yield return new WaitForSeconds(duration);
        
        Debug.Log("플레이어 카메라로 복귀");
        
        // 플레이어 카메라로 복귀
        playerCamera.Priority.Value = 11;
        tutorialFocusCamera.Priority.Value = 9;
        
        // 블렌드 시간 대기
        yield return new WaitForSeconds(1.5f);
        
        Debug.Log("튜토리얼 종료");
        
        isInTutorialMode = false;
    }

}
