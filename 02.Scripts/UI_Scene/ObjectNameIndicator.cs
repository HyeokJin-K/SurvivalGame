using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectNameIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private Image crosshair;

    [SerializeField] private float detectDistance = 3f;
    [SerializeField] private Color normalColor = Color.gray;
    [SerializeField] private Color activeColor = Color.yellow;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        nameUI.gameObject.SetActive(false);
        crosshair.color = normalColor;
    }

    private void Update()
    {
        DetectObject();
    }

    void DetectObject()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit[] hits = Physics.RaycastAll(ray, detectDistance, ~0, QueryTriggerInteraction.Ignore);

        RaycastHit? best = null;
        float bestDist = float.MaxValue;

        // 이름이 "T_"로 시작하는 것만 통과하며 가장 가까운 것 선택
        for (int i = 0; i < hits.Length; i++)
        {
            var h = hits[i];
            string n = h.collider.gameObject.name;
            if (n.Length >= 2 && n[0] == 'T' && n[1] == '_')
            {
                if (h.distance < bestDist)
                {
                    best = h;
                    bestDist = h.distance;
                }
            }
        }

        if (best.HasValue)
        {
            Show(best.Value.collider.gameObject.name);
        }
        else
        {
            Hide();
        }
    }

    void Show(string objName)
    {
        nameUI.text = objName;
        nameUI.gameObject.SetActive(true);
        crosshair.color = activeColor;
    }

    void Hide()
    {
        nameUI.gameObject.SetActive(false);
        crosshair.color = normalColor;
    }
}
