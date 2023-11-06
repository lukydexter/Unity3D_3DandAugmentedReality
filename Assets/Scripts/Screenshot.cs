using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Screenshot : MonoBehaviour
{
    [SerializeField]
    int screenshotWidth = 1920;
    [SerializeField]
    int screenshotHeight = 1080;
    [SerializeField]
    int limit=200;

    long screenshotCounter = 0;
    float initTime;
    float timePassed;
    long counter;
    bool flag;


    [SerializeField]
    float rotSpeed = 20;
    [SerializeField]
    int length = 100;
    // Start is called before the first frame update
    void Start()
    {
        initTime = Time.timeSinceLevelLoad;
        timePassed = Time.timeSinceLevelLoad;

        flag=true;
        counter=0;
        while(flag)
        {   
            string dest = Application.dataPath + "/Images/" + counter + ".png";
            if(File.Exists(dest))
            {
                File.Delete(Application.dataPath + "/Images/" + counter + ".png");
                counter++;
            }
            else
            {
                flag=false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //while(screenshotCounter<=limit)
        //{
            timePassed = Math.Abs(initTime - Time.timeSinceLevelLoad);
            //Debug.Log("time passed: " + timePassed);
            if (timePassed > 360 / rotSpeed)
            {
                initTime = Time.timeSinceLevelLoad;
                TakeScreenshot();
                screenshotCounter++;
            }
        //}
    }

    private void TakeScreenshot()
    {
        Camera camera = GetComponent<Camera>();

        RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);

        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();

        string destination = Application.dataPath + "/Images/" + screenshotCounter + ".png";
        //FileStream file;
        
        //if(!File.Exists(destination)) //file = File.OpenWrite(destination);
         //{file = File.Create(destination);}


        //string filename = string.Format("Screenshot_{0}.png", screenshotCounter);

        File.WriteAllBytes(destination, bytes);

        Debug.Log(string.Format("Took screenshot to: {0}", screenshotCounter));

        //screenshotCounter++;
    }
}
