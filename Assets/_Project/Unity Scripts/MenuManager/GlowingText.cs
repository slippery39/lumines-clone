using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlowingText : MonoBehaviour
{
    // Start is called before the first frame update
    // References
    public TextMeshProUGUI text;
    private static Material textBaseMaterial;
    private static Material textHighlightMaterial;

    private void Awake()
    {
        // Get a reference to the default base material
        textBaseMaterial = text.fontSharedMaterial;

        // Create new instance of the material assigned to the text object
        // Assumes all text objects will use the same highlight
        textHighlightMaterial = new Material(textBaseMaterial);

        // Set Glow Power on the new material instance
        textHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.0f);
        text.fontSharedMaterial= textHighlightMaterial;
    }

    private void Update()
    {
        textHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.Abs(Mathf.Sin(Time.unscaledTime * Mathf.PI)));
        text.UpdateMeshPadding();
    }

}
