using UnityEngine;
using UnityEngine.UI;

public class DragModel : MonoBehaviour
{
    public Slider poslid;
    public Transform model;

    void Update()
    {
        model.position = new Vector3(poslid.value, model.position.y, model.position.z);
    }
}