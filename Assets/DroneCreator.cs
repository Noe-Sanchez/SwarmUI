using UnityEngine;
using System;
using System.Net.Sockets;
using System.Linq;

public class DroneCreator : MonoBehaviour
{
    public GameObject droneTemplate;
    public GameObject centroid;
    public int numDrones = 4;

    public bool openSocket = true;

    private TcpClient socket;
    private int hz_counter = 0;
    void Start(){
        for (int i = 0; i < numDrones; i++){
            Vector3 dronePos = (numDrones % 2 != 0)  ? new Vector3( Mathf.Cos(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (2 * numDrones))), 0.0f, Mathf.Sin(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (2 * numDrones))) ) : new Vector3( Mathf.Cos(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (numDrones))), 0.0f, Mathf.Sin(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (numDrones))) );
            GameObject newDrone = Instantiate(droneTemplate, new Vector3(0,0,0), Quaternion.identity);
            // Get TextMeshPro component from child object and set text
            newDrone.transform.parent = centroid.transform;
            newDrone.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = i.ToString();
            // Do rainbow color effect with index and NumDrones
            //newDrone.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color((float)i/numDrones, 1, 1 - (float)i/numDrones);
            newDrone.GetComponent<MeshRenderer>().material.color = new Color((float)i/numDrones, 1, 1 - (float)i/numDrones);
            newDrone.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().color = new Color((float)i/numDrones, 1, 1 - (float)i/numDrones);
            //newDrone.transform.GetChild(1).GetComponent<Transform>().localPosition = dronePos;
            // Set to dronePos times the local scale of the drone
            newDrone.transform.localPosition = dronePos * newDrone.transform.localScale.x / centroid.transform.localScale.x; 
            //newDrone.transform.GetChild(1).GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
        }

        Destroy(droneTemplate);

        // Declare TCP client socket 
        socket = new TcpClient();  
        socket.BeginConnect("127.0.0.1", 18000, ConnectCallback, null);
    }

    void ConnectCallback(IAsyncResult ar){
        socket.EndConnect(ar);
        Debug.Log("Connected to server");
    } 

    void Update(){
        if (openSocket){
            hz_counter++;
            if (hz_counter % 60 == 0){
                // Send 28 bytes, 7 floats, twice in the same message
                float float1 = 1.0f;
                float float2 = 2.0f;
                float float3 = 3.0f;
                float float4 = 4.0f;
                float float5 = 5.0f;
                float float6 = 6.0f;
                float float7 = 7.0f;
                // Make byte array with the 14 floats, 56 bytes
                byte[] data = BitConverter.GetBytes(float1).Concat(BitConverter.GetBytes(float2)).Concat(BitConverter.GetBytes(float3)).Concat(BitConverter.GetBytes(float4)).Concat(BitConverter.GetBytes(float5)).Concat(BitConverter.GetBytes(float6)).Concat(BitConverter.GetBytes(float7)).Concat(BitConverter.GetBytes(float1)).Concat(BitConverter.GetBytes(float2)).Concat(BitConverter.GetBytes(float3)).Concat(BitConverter.GetBytes(float4)).Concat(BitConverter.GetBytes(float5)).Concat(BitConverter.GetBytes(float6)).Concat(BitConverter.GetBytes(float7)).ToArray();
                // Send data
                socket.GetStream().Write(data, 0, data.Length);

            }
        }

    }
}