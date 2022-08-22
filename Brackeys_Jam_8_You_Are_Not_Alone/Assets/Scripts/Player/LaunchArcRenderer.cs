using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaunchArcRenderer : MonoBehaviour
{

    LineRenderer lr;

    public float velocity;
    public float angle;
    public int resolution = 10;

    //gravity on the y axis
    float g;
    float radianAngle;



    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    private void OnValidate()
    {
        //check lr is not null and game is playing
        if(lr != null && Application.isPlaying)
        {
            RenderArc();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RenderArc();
    }

    //Populates the line renderer with appropriate settings
    //LineRenderer is a series of points, draw lines between them
    public void RenderArc()
    {
        
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
    }

    //Calculate array of vector 3 positions for throwing arc
    Vector3[] CalculateArcArray()
    {
        //Create array with size of the vertex count
        Vector3[] arrayPositions = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for(int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arrayPositions[i] = CalculateArcPoint(t, maxDistance);
        }

        return arrayPositions;
    }

    //Calc height and distance of vertices
    Vector3 CalculateArcPoint(float t,  float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }
    
}
