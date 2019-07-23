using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public Text scoreText;
    public Image backgroundImg;
    //MeshRenderer background;

    private bool isShowned = false;
    private float transition = 0.5f;

    private Sprite newSprite;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        //MeshRenderer background = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isShowned)
            return;
        
        transition += Time.deltaTime;
        // backgroundImg= GetComponent<Image>();

        // backgroundImg.color = Color.Lerp(backgroundImg.color,new Color(0,0,0,0), transition);
    }

    public void ToggleEndMenu(float score)
    {
        gameObject.SetActive(true);
        scoreText.text = ((int)score).ToString();
        isShowned = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
