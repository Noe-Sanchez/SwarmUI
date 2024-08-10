using UnityEngine;

public class TextFollow : MonoBehaviour
{
    public Transform parent; 

    void Update(){ 
        // Set the position of the text RectTransform to the position of the parent object
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetPositionAndRotation(parent.position + new Vector3(0, 0.2f, 0), parent.rotation); 
    }
}
