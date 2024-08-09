using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public GameObject prefabToSpawn;

    void Start(){
        for (int i = -5; i < 5; i++){
            for (int j = -5; j < 5; j++){
                GameObject newObject = Instantiate(prefabToSpawn, new Vector3((float)i+0.5f, 0, (float)j+0.5f), Quaternion.identity); 
                newObject.transform.localScale = new Vector3(0.1f, 1f, 0.1f);
                if ((i+((j % 2 == 0 ) ? 1 : 0)) % 2 == 0){
                    newObject.GetComponent<MeshRenderer>().material.color = new Color(0.75f, 0.75f, 0.75f);
                }else{
                    newObject.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
                }
                // Add as child of the object containing this script
                newObject.transform.parent = transform;
            }
        }
        // Destroy the prefab
        Destroy(prefabToSpawn);
        // Destroy the object containing this script
        //Destroy(gameObject); 
    }
}
