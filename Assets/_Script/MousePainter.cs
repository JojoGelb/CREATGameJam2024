using UnityEngine;

public class MousePainter : MonoBehaviour{
    private Camera cam;
    [Space]
    public bool mouseSingleClick;
    [Space]
    public Color paintColor;

    public bool ActiveRightClick;
    public bool ActiveLeftClick;

    public float radius = .1f;
    public float strength = 1;
    public float hardness = 1;

    private void Start()
    {
        cam = Camera.main;
    }

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
                Debug.Log("Mouse 2D: " + hit.textureCoord2);
                Paintable p = hit.collider.GetComponent<Paintable>();
                if(p != null){
                    if(rightClick && ActiveRightClick)
                        PaintManager.Instance.Paint(p, hit.point, radius, hardness, strength, paintColor);
                    if(leftClick && ActiveLeftClick)
                        PaintManager.Instance.Erase(p, hit.point, radius);
                }
            }
        }

    }

}
