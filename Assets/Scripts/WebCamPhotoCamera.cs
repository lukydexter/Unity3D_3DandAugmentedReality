using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamPhotoCamera : MonoBehaviour
{
    WebCamTexture webCamTexture;
    // Start is called before the first frame update
    void Start()
    {
        webCamTexture = new WebCamTexture();
        webCamTexture.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //GetComponent<RawImage>().texture = webCamTexture;
        IEnumerator TakePhoto()  // Start this Coroutine on some button click
        {

        // NOTE - you almost certainly have to do this here:

        yield return new WaitForEndOfFrame(); 

         // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
         // be sure to scroll down to the SECOND long example on that doco page 

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //FileStream file;
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        //File.WriteAllBytes(Application.dataPath + "/pictures/" + "photo.png", bytes);
        }
    }
}
