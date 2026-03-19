using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeaconLight : MonoBehaviour {
    public Color beaconColor = Color.yellow;
    public float height = 50f;
    private LineRenderer line;

    void Start() {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true; 
        line.positionCount = 2;
        line.startWidth = 0.2f;
        line.endWidth = 0.05f;
        line.material = new Material(Shader.Find("Unlit/Color"));
        line.material.color = beaconColor;
    }

    void Update() {
        Vector3 basePos = transform.position;
        Vector3 topPos = basePos + Vector3.up * height;

        line.SetPosition(0, basePos);
        line.SetPosition(1, topPos);
    }
}