using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VehicleHitPointSlider : MonoBehaviour
{
    [SerializeField] 
    private Vehicle m_Vehicle;

    [SerializeField] 
    private Image m_FIllImage;

    [SerializeField] 
    private Slider m_Slider;

    private void Start()
    {
        m_Vehicle.HitPointChange += OnHitPointChange;

        m_FIllImage.color = m_Vehicle.Owner.GetComponent<Player>().PlayerColor;

        m_Slider.maxValue = m_Vehicle.MaxHitPoint;
        m_Slider.value = m_Vehicle.HitPoint;
    }

    private void OnDestroy()
    {
        m_Vehicle.HitPointChange -= OnHitPointChange;
    }
    private void OnHitPointChange(int hitPoint)
    {
        m_Slider.value = hitPoint;
    }
}
