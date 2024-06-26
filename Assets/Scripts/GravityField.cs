using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    public float radius = 5f;
    public float tolerance = 0.1f;
    [Header("Warning Indicator")]
    public SpriteRenderer warningSprite;
    [Range(0.5f, 1)] public float warningStartRadiusScale = 0.7f;
    public Gradient warningGradient;
    public float warningAlphaScale = 0.2f;
    [Header("Gravity VFX")]
    public float gravityVfxDuration = 4f;
    public float gravityVfxInterval = 4f;
    public Transform gravityVFX;
    public Color gravityVfxInitialColor = new Color(255, 255, 255, 51);

    private void Awake()
    {
        transform.localScale = Vector3.one * radius * 2f;    
    }

    public bool IsInField(Planet planet)
    {
        var planetRadius = planet.GetData().radius;
        var planetPos = planet.transform.position;

        return (Vector3.Distance(planetPos, transform.position) + planetRadius <= radius + tolerance);
    }

    private float maxPlanetDistance = 0f;
    public void SetDistanceFromCenter(Planet planet)
    {
        var planetPos = planet.transform.position;
        var distance = Vector3.Distance(planetPos, transform.position);
        distance += planet.GetData().radius;
        maxPlanetDistance = Mathf.Max(maxPlanetDistance, distance);
    }

    private void Update()
    {
        var t = Mathf.InverseLerp(radius * warningStartRadiusScale, radius, maxPlanetDistance);

        var color = warningGradient.Evaluate(t);
        color.a *= warningAlphaScale;
        warningSprite.color = color;

        maxPlanetDistance = 0f;


        float x = Time.time % gravityVfxInterval;
        x = Mathf.Lerp(1, 0, x / gravityVfxDuration);

        gravityVFX.localScale = Vector3.one * x;

        var newColor = gravityVfxInitialColor;
        newColor.a *= x;
        gravityVFX.GetComponent<SpriteRenderer>().color = newColor;
    }

    private void OnValidate() => Awake();
}
