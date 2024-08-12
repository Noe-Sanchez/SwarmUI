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
    private float hz_counter = 0;
    
    private byte[] posData;
    void Start(){
        posData = new byte[(numDrones+1) * 28];

        for (int i = 0; i < numDrones; i++){
            Vector3 dronePos = (numDrones % 2 != 0)  ? new Vector3( Mathf.Cos(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (2 * numDrones))), 0.0f, Mathf.Sin(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (2 * numDrones))) ) : new Vector3( Mathf.Cos(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (numDrones))), 0.0f, Mathf.Sin(((2 * Mathf.PI * i) / numDrones) + (Mathf.PI / (numDrones))) );
            GameObject newDrone = Instantiate(droneTemplate, new Vector3(0,0,0), Quaternion.identity);
            // Get TextMeshPro component from child object and set text
            newDrone.transform.parent = centroid.transform;
            newDrone.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = i.ToString();
            // Do rainbow color effect with index and NumDrones
            //newDrone.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color((float)i/numDrones, 1, 1 - (float)i/numDrones);
            newDrone.GetComponent<MeshRenderer>().material.color = new Color((float)i/numDrones, 1, 1 - (float)i/numDrones);
            //Add transparency to mesh renderer of 0.5
            newDrone.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 3); 
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
            
            // Use delta time
            hz_counter += Time.deltaTime;
            //if (hz_counter >= 1.0f/60.0f){

            
            // Use delta time to send data at 60 Hz
            if (hz_counter >= 1.0f){
                hz_counter = 0;
                // Send 28 bytes, 7 floats, twice in the same message
                /*float float1 = 1.0f;
                float float2 = 2.0f;
                float float3 = 3.0f;
                float float4 = 4.0f;
                float float5 = 5.0f;
                float float6 = 6.0f;
                float float7 = 7.0f;
                // Make byte array with the 14 floats, 56 bytes
                byte[] data = BitConverter.GetBytes(float1).Concat(BitConverter.GetBytes(float2)).Concat(BitConverter.GetBytes(float3)).Concat(BitConverter.GetBytes(float4)).Concat(BitConverter.GetBytes(float5)).Concat(BitConverter.GetBytes(float6)).Concat(BitConverter.GetBytes(float7)).Concat(BitConverter.GetBytes(float1)).Concat(BitConverter.GetBytes(float2)).Concat(BitConverter.GetBytes(float3)).Concat(BitConverter.GetBytes(float4)).Concat(BitConverter.GetBytes(float5)).Concat(BitConverter.GetBytes(float6)).Concat(BitConverter.GetBytes(float7)).ToArray();
                // Send data
                socket.GetStream().Write(data, 0, data.Length);*/
                for (int i = 0; i < (numDrones+1); i++){
                    float posx, posy, posz, rotx, roty, rotz, rotw;
                    // If not centroid i.e not last index
                    if (i == (numDrones)){
                        posx = centroid.transform.localPosition.x;
                        posy = centroid.transform.localPosition.y;
                        posz = centroid.transform.localPosition.z;
                        rotx = centroid.transform.localRotation.x;
                        roty = centroid.transform.localRotation.y;
                        rotz = centroid.transform.localRotation.z;
                        rotw = centroid.transform.localRotation.w;
                    }else{
                        posx = centroid.transform.GetChild(i).transform.localPosition.x;
                        posy = centroid.transform.GetChild(i).transform.localPosition.y;
                        posz = centroid.transform.GetChild(i).transform.localPosition.z;
                        rotx = centroid.transform.GetChild(i).transform.localRotation.x;
                        roty = centroid.transform.GetChild(i).transform.localRotation.y;
                        rotz = centroid.transform.GetChild(i).transform.localRotation.z;
                        rotw = centroid.transform.GetChild(i).transform.localRotation.w;
                    }

                    posData[i*28 + 0] = BitConverter.GetBytes(posx)[0];
                    posData[i*28 + 1] = BitConverter.GetBytes(posx)[1];
                    posData[i*28 + 2] = BitConverter.GetBytes(posx)[2];
                    posData[i*28 + 3] = BitConverter.GetBytes(posx)[3];

                    posData[i*28 + 4] = BitConverter.GetBytes(posy)[0];
                    posData[i*28 + 5] = BitConverter.GetBytes(posy)[1];
                    posData[i*28 + 6] = BitConverter.GetBytes(posy)[2];
                    posData[i*28 + 7] = BitConverter.GetBytes(posy)[3];

                    posData[i*28 + 8] = BitConverter.GetBytes(posz)[0];
                    posData[i*28 + 9] = BitConverter.GetBytes(posz)[1];
                    posData[i*28 + 10] = BitConverter.GetBytes(posz)[2];
                    posData[i*28 + 11] = BitConverter.GetBytes(posz)[3];

                    posData[i*28 + 12] = BitConverter.GetBytes(rotx)[0];
                    posData[i*28 + 13] = BitConverter.GetBytes(rotx)[1];
                    posData[i*28 + 14] = BitConverter.GetBytes(rotx)[2];
                    posData[i*28 + 15] = BitConverter.GetBytes(rotx)[3];

                    posData[i*28 + 16] = BitConverter.GetBytes(roty)[0];
                    posData[i*28 + 17] = BitConverter.GetBytes(roty)[1];
                    posData[i*28 + 18] = BitConverter.GetBytes(roty)[2];
                    posData[i*28 + 19] = BitConverter.GetBytes(roty)[3];

                    posData[i*28 + 20] = BitConverter.GetBytes(rotz)[0];
                    posData[i*28 + 21] = BitConverter.GetBytes(rotz)[1];
                    posData[i*28 + 22] = BitConverter.GetBytes(rotz)[2];
                    posData[i*28 + 23] = BitConverter.GetBytes(rotz)[3];

                    posData[i*28 + 24] = BitConverter.GetBytes(rotw)[0];
                    posData[i*28 + 25] = BitConverter.GetBytes(rotw)[1];
                    posData[i*28 + 26] = BitConverter.GetBytes(rotw)[2];
                    posData[i*28 + 27] = BitConverter.GetBytes(rotw)[3];


                socket.GetStream().Write(posData, 0, posData.Length);

                }
            }
        }

    }
}