using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIHitPointSlider : MonoBehaviour
{
    [SerializeField] private Destructible m_Destructible;

    [SerializeField] private Image m_FIllImage;

    [SerializeField] private Slider m_Slider;

    private void Start()
    {
        m_Destructible.HitPointChange += OnHitPointChange;

        m_Slider.maxValue = m_Destructible.MaxHitPoint;
        m_Slider.value = m_Destructible.HitPoint;
    }

    private void OnDestroy()
    {
        m_Destructible.HitPointChange -= OnHitPointChange;
    }
    private void OnHitPointChange(int hitPoint)
    {
        m_Slider.value = hitPoint;
    }
}
