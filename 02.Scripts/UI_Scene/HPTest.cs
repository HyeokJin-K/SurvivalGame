using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPTest : MonoBehaviour
{
    [SerializeField] private TMP_InputField curhp;
    [SerializeField] private TMP_InputField maxhp;
    
    public void OnClick()
    {
        HUDManager.Instance.HealthBarUpdate(float.Parse(curhp.text), float.Parse(maxhp.text));
    }
}
