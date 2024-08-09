using UnityEngine;

public class DroneCreator : MonoBehaviour
{
    public GameObject droneTemplate; 
    public int numDrones = 4;

    void Start(){
        for (int i = 0; i < numDrones; i++){
            Vector3 dronePos = (numDrones % 2 != 0)  ? new Vector3( Mathf.Cos(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (2 * numDrones))), 0.5f, Mathf.Sin(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (2 * numDrones))) ) : new Vector3( Mathf.Cos(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (numDrones))), 0.5f, Mathf.Sin(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (numDrones))) );
            GameObject newDrone = Instantiate(droneTemplate, new Vector3(0,0,0), Quaternion.identity);
            // Get TextMeshPro component from child object and set text
            newDrone.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = i.ToString();
            // Do rainbow color effect with index and NumDrones
            newDrone.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color((float)i/numDrones, 1, 1 - (float)i/numDrones);
            newDrone.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().color = new Color((float)i/numDrones, 1, 1 - (float)i/numDrones);
            //newDrone.transform.GetChild(1).GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
            newDrone.transform.GetChild(1).GetComponent<Transform>().position = dronePos;
        }


        Destroy(droneTemplate);
        Destroy(gameObject); 
    }
}