using UnityEngine;

public class DraggableCube : MonoBehaviour {

    public float hoverHeight = 0.5f;
    private Vector3 offset;

    void OnMouseOver(){
        if (Input.GetMouseButtonDown(0))
            offset = transform.position - GetMouseWorldPos();
        if(Input.GetMouseButtonDown(1))
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 15, 0);
    }

    void OnMouseDrag(){
        transform.position = GetMouseWorldPos() + offset;
        transform.position = new Vector3(transform.position.x, hoverHeight, transform.position.z); 
    }

    Vector3 GetMouseWorldPos(){
        var screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
