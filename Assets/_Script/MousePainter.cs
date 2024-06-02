using UnityEngine;

public class MousePainter : MonoBehaviour{
    public Camera cam;
    [Space]
    public bool mouseSingleClick;
    [Space]
    public Color paintColor;
    
    public float radius = .1f;
    public float strength = 1;
    public float hardness = 1;

    void Update(){

        bool leftClick, rightClick = false;
        leftClick = mouseSingleClick ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);
        if (!leftClick)
        {
            rightClick = Input.GetMouseButtonDown(1) || Input.GetMouseButton(1);
        }

        if (leftClick || rightClick)
        {
            Vector3 position = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f)){
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                transform.position = hit.point;
                Paintable p = hit.collider.GetComponent<Paintable>();
                if(p != null){
                    if(rightClick)
                        PaintManager.Instance.Paint(p, hit.point, radius, hardness, strength, paintColor);
                    if(leftClick)
                        PaintManager.Instance.Erase(p, hit.point, radius);
                }
            }
        }

    }

}