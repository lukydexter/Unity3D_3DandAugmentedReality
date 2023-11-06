using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MultiRaycast : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 20;
    [SerializeField]
    int length = 100;
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    int limit=200;
    [SerializeField]
    LineRenderer laserLineRenderer;

    const int buildings = 7;
    const int street = 9;
    const int vehicles = 6;
    const int nature = 8;
    const int grass = 10;
    const int background = 3;
    const int accessories = 11;

    long filecounter = 0;
    List<float[]> pcs;

    private Color[] colors;

    float initTime;
    float timePassed;
    long counter;
    bool flag;
    long count_num=0;
    int k=-1;
    int j=0;

    private void Start()
    {
        initTime = Time.timeSinceLevelLoad;
        timePassed = Time.timeSinceLevelLoad;

        flag=true;
        counter=0;
        while(flag)
        {   
            string dest = Application.dataPath + "/PointClouds/" + counter + ".txt";
            if(File.Exists(dest))
            {
                File.Delete(Application.dataPath + "/PointClouds/" + counter + ".txt");
                counter++;
            }
            else
            {
                flag=false;
            }
        }

       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 1f, 0) * rotSpeed * Time.deltaTime, Space.Self);

        //Vector3[] directions = { new Vector3(0, 0, 1), new Vector3(0, -0.09f, 1), new Vector3(0, -0.08f, 1), new Vector3(0, -0.07f, 1), new Vector3(0, -0.06f, 1), new Vector3(0, -0.05f, 1), new Vector3(0, -0.03f, 1), new Vector3(0, 0.02f, 1), new Vector3(0, -0.02f, 1), new Vector3(0, 0.01f, 1), new Vector3(0, -.04f, 1), new Vector3(0, 0.04f, 1), new Vector3(0, -0.1f, 1), new Vector3(0.02f, 0, 1), new Vector3(-0.03f, -0.09f, 1), new Vector3(-0.08f, -0.08f, 1), new Vector3(-0.01f, -0.07f, 1), new Vector3(0.06f, -0.06f, 1), new Vector3(0.09f, -0.05f, 1), new Vector3(0.03f, -0.03f, 1), new Vector3(0.04f, 0.02f, 1), new Vector3(-0.04f, -0.02f, 1), new Vector3(0.05f, 0.01f, 1), new Vector3(0.07f, -.04f, 1), new Vector3(-0.07f, 0.04f, 1), new Vector3(0.03f, -0.1f, 1)};
        //Vector3[] directions = { Vector3.forward, Vector3.left, Vector3.back, Vector3.right };
        List<Vector3> directions = new List<Vector3>();

        for(k=-1; k<8; k++){
            for(j=0; j<8; j++){
                directions.Add(new Vector3(j*0.1f, k*0.1f, 1));
            }
        }

        RaycastHit[] hit = new RaycastHit[directions.Count];
        colors = new Color[directions.Count];
        int i = 0;

        foreach (Vector3 dir in directions)
        {
            //while(filecounter<=limit)
            //{
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, transform.TransformDirection(dir), out hit[i], Mathf.Infinity, layerMask))
                {
                    Color lineColor = Color.red;
                    int c = 0;

                    switch (hit[i].transform.gameObject.layer)
                    {
                        case vehicles:
                            lineColor = Color.blue;
                            c = vehicles;
                            break;
                        case buildings:
                            lineColor = Color.red;
                            c = buildings;
                            break;
                        case street:
                            lineColor = Color.blue;
                            c = street;
                            break;
                        case background:
                            lineColor = Color.yellow;
                            c = background;
                            break;
                        case grass:
                            lineColor = Color.green;
                            c = grass;
                            break;
                        case nature:
                            lineColor = Color.green;
                            c = nature;
                            break;
                        case accessories:
                            lineColor = Color.red;
                            c = accessories;
                            break;
                    }
                    Debug.DrawRay(transform.position, transform.TransformDirection(dir) * hit[i].distance, lineColor);
                    Debug.Log("Did Hit " + i);
                    laserLineRenderer.SetColors(lineColor, lineColor);

                    colors[i] = hit[i].collider.GetComponent<Renderer>().material.color;
                    SavePoint(hit[i].point, c, colors[i]);
                    

                    count_num++;
                    if(count_num==10000)
                    {
                        filecounter++;
                        count_num=0;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(dir) * length, Color.green);
                    //Debug.Log("Did not Hit");
                }

                timePassed = Math.Abs(initTime - Time.timeSinceLevelLoad);
                //Debug.Log("time passed: " + timePassed);
                //if (timePassed > 360 / rotSpeed)
                //{
                 //   initTime = Time.timeSinceLevelLoad;
                 //   filecounter++;
                //}
                i++;
            //}
        }
    }

    void SavePoint(Vector3 point, int c, Color colors)
    {
        string s = "";
        s += point[0].ToString() + ",";
        s += point[1].ToString() + ",";
        s += point[2].ToString() + ",";
        s += colors.r +",";
        s += colors.g +",";
        s += colors.b +"\n";
        //s += c.ToString() + "\n";


        //string destination = Application.dataPath + "/PointClouds/" + filecounter + ".txt";
        //FileStream file;
        
        //if(!File.Exists(destination)) //file = File.OpenWrite(destination);
         //{file = File.Create(destination);}
        //file = File.OpenWrite(destination);
        File.AppendAllText(Application.dataPath + "/PointClouds/" + filecounter + ".txt", s);
        
        //filecounter++;
    }
}
