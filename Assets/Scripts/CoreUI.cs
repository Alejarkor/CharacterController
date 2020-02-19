using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoreUI : MonoBehaviour
{
    [SerializeField] private Image _ring;
    [SerializeField] private Image _ringGrey;
    [SerializeField] private Image _icon;

    public float ringFill;
    public float ringGreyFill;
    public float iconFill;

    // Update is called once per frame
    void Update()
    {
        _ring.fillAmount = ringFill;
        _ringGrey.fillAmount = ringGreyFill;
        _icon.fillAmount = iconFill;
    }
}
