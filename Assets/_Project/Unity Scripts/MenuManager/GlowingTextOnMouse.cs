using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlowingTextOnMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // References
    public TextMeshProUGUI text;
    private static Material textBaseMaterial;
    private static Material textHighlightMaterial;
    private bool isInText = false;

    private void Awake()
    {
        // Get a reference to the default base material
        textBaseMaterial = text.fontSharedMaterial;

        // Create new instance of the material assigned to the text object
        // Assumes all text objects will use the same highlight
        textHighlightMaterial = new Material(textBaseMaterial);

        // Set Glow Power on the new material instance
        textHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.0f);
    }

    void Update()
    {
        if (isInText)
        {
            textHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.Abs(Mathf.Sin(Time.unscaledTime * (Mathf.PI*2))));
            text.UpdateMeshPadding();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isInText = true;
        text.fontSharedMaterial = textHighlightMaterial;
        text.UpdateMeshPadding();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontSharedMaterial = textBaseMaterial;
    }
}
