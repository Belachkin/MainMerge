using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenURLButtonComponent : MonoBehaviour
{
    [SerializeField] string _url;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(_url));
    }
}
