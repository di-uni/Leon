using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public static Transform playerTransform;
    public static float spawnZ = -6.4f;
    public static float tileLength = 6.4f;
    private float safeZone =10.0f;
    public static int amnTilesOnScreen = 10;
    private int lastPrefabIndex =0;
    public static Color rancolor = new Color();
    

    public static List<GameObject> activeTiles= new List<GameObject>();
    // Start is called before the first frame update
    private void Start()
    {
        //activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        for(int i =0; i< amnTilesOnScreen; i++){
            if(i<6)
                SpawnTile(0);
            else
                SpawnTile();
        }
    
    }

    // Update is called once per frame
    private void Update()
    {
        if((playerTransform.position.z-safeZone) > (spawnZ - amnTilesOnScreen *tileLength)){

            SpawnTile();
            DeleteTile();
        }
    }

    private void SpawnTile(int prefabIndex = -1)
    {
        

        GameObject go;
        if(prefabIndex == -1)
            go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        else
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
    }

    private void DeleteTile() {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex() 
    {
        if(tilePrefabs.Length <= 1)
            return 0;

        int randomIndex = lastPrefabIndex;
        while(randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0,tilePrefabs.Length);
        }
        lastPrefabIndex = randomIndex;
        return randomIndex;
    }

    public void CheckColor(Color color ,Vector3 position){
        if(activeTiles.Count>=10){
            int behind= (int) (playerTransform.position.z/tileLength);
            int forward = (int) (spawnZ/tileLength);
            //Debug.Log(behind+"counting");
            int currentTile = amnTilesOnScreen-(forward-behind)+1;
            rancolor= activeTiles[currentTile].GetComponent<MeshRenderer>().material.color;
            Debug.Log(currentTile);
        }

        float red =color.r;
        float green=color.g;
        float blue = color.b;
        float red2 = rancolor.r;
        float green2 =rancolor.g;
        float blue2 =rancolor.b;
        float[] colorLab = rgb2lab(red,green,blue);
        float[] ranLab = rgb2lab(red2,green2,blue2);
        float distance =Mathf.Sqrt(Mathf.Pow((colorLab[0]-ranLab[0]),2f)+ Mathf.Pow((colorLab[1]-ranLab[1]),2f)+ Mathf.Pow((colorLab[2]-ranLab[2]),2f));
        
        Debug.Log(distance);
        if(distance<24){Debug.Log("Matched");}
        else Debug.Log("Not matched");
        Debug.Log(activeTiles.Count+"this is tile");
    }
    
    static float Gamma(float x){
        return x>0.04045f ? Mathf.Pow((x+0.055f)/1.055f,2.4f): x/ 12.92f;

    }

    public static float[] rgb2lab(float var_R, float var_G, float var_B){
        float[] arr = new float[3];
        float B = Gamma(var_B);
        float G = Gamma(var_G);
        float R = Gamma(var_R);
        float X = 0.412453f * R + 0.357580f *G + 0.180423f *B;
        float Y = 0.212671f * R + 0.715160f *G + 0.072169f *B;
        float Z = 0.019334f * R + 0.119193f *G + 0.950227f *B;

        X/= 0.95047f;
        Y/= 1.0f;
        Z/= 1.08883f;

        float FX =X>0.008856f ? Mathf.Pow(X, 1.0f / 3.0f) : (7.787f * X + 0.137931f);
        float FY =Y>0.008856f ? Mathf.Pow(Y, 1.0f / 3.0f) : (7.787f * Y + 0.137931f);
        float FZ =Z>0.008856f ? Mathf.Pow(Z, 1.0f / 3.0f) : (7.787f * Z + 0.137931f);

        arr[0] = Y > 0.008856f ? (116.0f * FY -16.0f) : (903.3f *Y);
        arr[1] = 500f * (FX-FY);
        arr[2] = 200f * (FY-FZ);
        
        return arr;
    }
    
}
