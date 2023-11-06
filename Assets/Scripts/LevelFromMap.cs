using System.IO;
using UnityEngine;

public class LevelFromMap : MonoBehaviour
{
    [SerializeField]
    GameObject plane;
    [SerializeField]
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        byte[] bytes = File.ReadAllBytes(Application.dataPath +"/Textures/lab.png"); //800x800

        int len = (int) plane.transform.localScale.x * 10;
        Texture2D tex = new Texture2D(len, len);
        tex.LoadImage(bytes);
        tex.Apply();
        plane.GetComponent<Renderer>().material = new Material(mat);
        //plane.GetComponent<Renderer>().material.mainTexture = tex;
        //var Bytes = tex.EncodeToPNG();
        //File.WriteAllBytes(Application.dataPath + "/Test2.png", Bytes);

        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                Color col = tex.GetPixel(i, j);
                if (col != Color.white)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(i - len/2, 1f, j - len/2);
                    cube.transform.localScale = new Vector3(1, 4, 1);
                    cube.GetComponent<Renderer>().material = new Material(mat);
                    cube.transform.SetParent(plane.transform);
                }
            }
        }
    }
}
