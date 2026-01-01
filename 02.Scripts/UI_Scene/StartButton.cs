using UnityEngine;

public class StartButton : MonoBehaviour
{
    public FadeOut panelFader;

    public void OnClickStartButton()
    {
        panelFader.StartFadeOut(() =>
        {
            LoadingSceneManager.LoadScene("Main2");
        });
    }

    public void OnClickEndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 플레이 중지
#else
        Application.Quit(); // 빌드된 앱 종료(모바일/PC)
#endif
    }
}
