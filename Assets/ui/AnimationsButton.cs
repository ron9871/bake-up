using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimationsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button button;
    [SerializeField] float scaleIncrease = 1.1f;  // ����� ������ ������ ��� ����� ����
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        // ����� ��� ����� ������ �� ������
        originalScale = button.transform.localScale;

        // ����� ����� ������ ����� �� ������
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        // ����� �� ������ ���� ����� ������
        if (button.interactable)
        {
            // ���� ������ ��� ������ ������
        }
    }

    // ������� ������ ���� ������ ����
    void OnButtonClick()
    {
        Debug.Log("Button clicked!");
    }

    // ������� ������ ���� ����� ���� �� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ����� ��� ����� �� ������
        button.transform.localScale = originalScale * scaleIncrease;
    }

    // ������� ������ ���� ����� ���� �������
    public void OnPointerExit(PointerEventData eventData)
    {
        // ����� ��� ����� �� ������ ���� ����� ������
        button.transform.localScale = originalScale;
    }
}
