using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class ImageFillSetter : MonoBehaviour
{
    private Image _image;

    public FloatValue fillAmount;


    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        _image.fillAmount = fillAmount;
    }
}