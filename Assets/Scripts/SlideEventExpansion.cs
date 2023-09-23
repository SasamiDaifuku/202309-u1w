using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderEventExpansion : MonoBehaviour, IPointerUpHandler
{
    private Slider _slider;

    private readonly Subject<float> _changeValueSubject = new Subject<float>();
 
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
 
    public IObservable<float> OnChangeValue
    {
        get { return _changeValueSubject; }
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        _changeValueSubject.OnNext(_slider.value);
    }
}
