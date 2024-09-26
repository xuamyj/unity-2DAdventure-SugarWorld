using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    // boilerplate 1
    public static UIHandler instance { get; private set; }

    private VisualElement m_Healthbar;

    // boilerplate 2
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();

        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");

        SetHealthValue(1.0f);
    }

    public void SetHealthValue(float betwZeroAndOne)
    {
        m_Healthbar.style.width = Length.Percent(betwZeroAndOne * 100.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
