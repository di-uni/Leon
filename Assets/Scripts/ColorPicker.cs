﻿using UnityEngine;
using System.Collections;

public class ColorPicker : MonoBehaviour {

	public Texture2D colorSpace;
	public Texture2D alphaGradient;
	public string Title = "Color Picker";
	public Vector2 startPos = new Vector2(20, 20);
	public GameObject receiver;

	public GameObject receiver1;

	public GameObject receiver2;


	public string colorSetFunctionName = "OnSetNewColor";
	public string colorGetFunctionName = "OnGetColor";
	public bool useExternalDrawer = false;
	public int drawOrder = 0;

	private TileManager tileManager= new TileManager();


	private Color TempColor; 
	public static Color SelectedColor;

	static ColorPicker activeColorPicker = null;

	enum ESTATE
	{
		Hidden,
		Showed,
		Showing,
		Hidding
	}; 
	ESTATE mState = ESTATE.Hidden;
	
	int sizeFull = 600;
	int sizeHidden =250;
	float animTime = 0.25f;
	float dt = 0;

	float sizeCurr = 0;
	float alphaGradientHeight = 16;

	GUIStyle titleStyle = null;
	Color textColor = Color.black;
	Texture2D txColorDisplay;

	string txtR, txtG, txtB, txtA;
	float valR, valG, valB, valA;
	
	public void NotifyColor(Color color)
	{
		SetColor(color);
		SelectedColor = color;
		UpdateColorEditFields(false);
		UpdateColorSliders(false);
	}

	void Start()
	{
		sizeCurr = sizeHidden;

		txColorDisplay = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		if(receiver)
		{
			receiver.SendMessage(colorGetFunctionName, this, SendMessageOptions.DontRequireReceiver);
			receiver1.SendMessage(colorGetFunctionName, this, SendMessageOptions.DontRequireReceiver);
			receiver2.SendMessage(colorGetFunctionName, this, SendMessageOptions.DontRequireReceiver);
		}
	}


	void OnGUI()
	{
		if(!useExternalDrawer)
		{
			_DrawGUI();
		}
	}

	void UpdateColorSliders(bool isFocused)
	{
		if(!isFocused)
		{
			valR = TempColor.r;
			valG = TempColor.g;
			valB = TempColor.b;
			valA = TempColor.a;
		}
		else
		{
			SetColor(new Color(valR, valG, valB, valA));
		}
	}

	void UpdateColorEditFields(bool isFocused)
	{
		if(!isFocused)
		{
			txtR = (255 * TempColor.r).ToString();
			txtG = (255 * TempColor.g).ToString();
			txtB = (255 * TempColor.b).ToString();
			txtA = (255 * TempColor.a).ToString();
		}
		else
		{
			byte r = 0;
			byte g = 0;
			byte b = 0;
			byte a = 0;
			if(!string.IsNullOrEmpty(txtR)) {
				r = byte.Parse(txtR, System.Globalization.NumberStyles.Any);
			}
			if(!string.IsNullOrEmpty(txtG)) {
				g = byte.Parse(txtG, System.Globalization.NumberStyles.Any);
			}
			if(!string.IsNullOrEmpty(txtB)) {
				b = byte.Parse(txtB, System.Globalization.NumberStyles.Any);
			}
			if(!string.IsNullOrEmpty(txtA)) {
				a = byte.Parse(txtA, System.Globalization.NumberStyles.Any);
			}
			SetColor(new Color32(r, g, b, a));
		}
	}

	// Update is called once per frame
	public void _DrawGUI () 
	{
		if (titleStyle == null) {
			titleStyle = new GUIStyle (GUI.skin.label);
			titleStyle.normal.textColor = textColor;
		}

		Rect rectColorEdit = new Rect(startPos.x + sizeCurr + 10, startPos.y + 30, 40, 140);
		Rect rectColorSlider = new Rect(startPos.x + sizeCurr + 50, startPos.y + 30, 60, 140);

		GUI.Label(new Rect(startPos.x + sizeCurr + 60, startPos.y, 200, 30), Title, titleStyle);

		GUI.DrawTexture(new Rect(startPos.x + sizeCurr + 10, startPos.y, 40, 20), txColorDisplay);

		if(mState == ESTATE.Showed)
		{
			txtR = GUI.TextField(new Rect(startPos.x + sizeCurr + 10, startPos.y + 30, 40, 20), txtR, 3);
			txtG = GUI.TextField(new Rect(startPos.x + sizeCurr + 10, startPos.y + 60, 40, 20), txtG, 3);
			txtB = GUI.TextField(new Rect(startPos.x + sizeCurr + 10, startPos.y + 90, 40, 20), txtB, 3);
			txtA = GUI.TextField(new Rect(startPos.x + sizeCurr + 10, startPos.y + 120, 40, 20), txtA, 3);
			valR = GUI.HorizontalSlider(new Rect(startPos.x + sizeCurr + 50, startPos.y + 35, 60, 20), valR, 0.0f, 1.0f);
			valG = GUI.HorizontalSlider(new Rect(startPos.x + sizeCurr + 50, startPos.y + 65, 60, 20), valG, 0.0f, 1.0f);
			valB = GUI.HorizontalSlider(new Rect(startPos.x + sizeCurr + 50, startPos.y + 95, 60, 20), valB, 0.0f, 1.0f);
			valA = GUI.HorizontalSlider(new Rect(startPos.x + sizeCurr + 50, startPos.y + 125, 60, 20), valA, 0.0f, 1.0f);
			if(GUI.Button(new Rect(startPos.x + sizeCurr + 10, startPos.y + 150, 60, 20), "Apply"))
			{
				ApplyColor();
				SelectedColor = TempColor;
				if(receiver)
				{
					receiver.SendMessage(colorSetFunctionName, SelectedColor, SendMessageOptions.DontRequireReceiver);
					receiver1.SendMessage(colorSetFunctionName, SelectedColor, SendMessageOptions.DontRequireReceiver);
					receiver2.SendMessage(colorSetFunctionName, SelectedColor, SendMessageOptions.DontRequireReceiver);
				}
			}

			GUIStyle labelStyleRGBA = new GUIStyle(GUI.skin.label);
			labelStyleRGBA.normal.textColor = Color.white;
			GUI.Label(new Rect(startPos.x + sizeCurr + 110, startPos.y + 30, 20, 20), "R", labelStyleRGBA);
			GUI.Label(new Rect(startPos.x + sizeCurr + 110, startPos.y + 60, 20, 20), "G", labelStyleRGBA);
			GUI.Label(new Rect(startPos.x + sizeCurr + 110, startPos.y + 90, 20, 20), "B", labelStyleRGBA);
			GUI.Label(new Rect(startPos.x + sizeCurr + 110, startPos.y + 120, 20, 20), "A", labelStyleRGBA);
		}

		//update scaling states
		if(mState == ESTATE.Showing)
		{
			sizeCurr = Mathf.Lerp(sizeHidden, sizeFull, dt/animTime);
			if(dt/animTime > 1.0f) {
				mState = ESTATE.Showed;
			}
			dt += Time.deltaTime;
		}
		if(mState == ESTATE.Hidding)
		{
			sizeCurr = Mathf.Lerp(sizeFull, sizeHidden, dt/animTime);
			if(dt/animTime > 1.0f) {
				mState = ESTATE.Hidden;
			}
			dt += Time.deltaTime;
		}
		//draw color picker
		Rect rect = new Rect(startPos.x, startPos.y, sizeCurr, sizeCurr);
		GUI.DrawTexture(rect, colorSpace);

		float alphaGradHeight = alphaGradientHeight * (sizeCurr/sizeFull);
		Vector2 startPosAlpha = startPos + new Vector2(0, sizeCurr);
		Rect rectAlpha = new Rect(startPosAlpha.x, startPosAlpha.y, sizeCurr, alphaGradHeight);
		GUI.DrawTexture(rectAlpha, alphaGradient);

		Rect rectFullSize = new Rect(startPos.x, startPos.y, sizeCurr, sizeCurr + alphaGradHeight);

		Vector2 mousePos = Event.current.mousePosition;
		Event e = Event.current;
		bool isLeftMBtnClicked = e.type == EventType.MouseUp;
		bool isLeftMBtnDragging = e.type == EventType.MouseDrag;
		bool openCondition = (rectFullSize.Contains(e.mousePosition) && (((e.type == EventType.MouseUp || e.type == EventType.MouseDrag || e.type == EventType.MouseMove) && e.isMouse)));
		bool closeCondition = isLeftMBtnClicked || (!rectFullSize.Contains(e.mousePosition)) && (e.isMouse && (e.type == EventType.MouseMove || e.type == EventType.MouseDown));
		if(openCondition && (activeColorPicker == null || activeColorPicker.mState == ESTATE.Hidden))
		{
			if(mState == ESTATE.Hidden)
			{
				mState = ESTATE.Showing;
				activeColorPicker = this;
				dt = 0;
			}
		}
		if(closeCondition)
		{
			if(mState == ESTATE.Showed)
			{
				if(isLeftMBtnClicked)
				{
					ApplyColor();
				}
				else
				{
					SetColor(SelectedColor);
				}

				mState = ESTATE.Hidding;
				dt = 0;
			}
		}
		if(mState == ESTATE.Showed)
		{
			if(rect.Contains(e.mousePosition))
			{
				float coeffX = colorSpace.width / sizeCurr;
				float coeffY = colorSpace.height / sizeCurr;
				Vector2 localImagePos = (mousePos - startPos);
				Color res = colorSpace.GetPixel((int)(coeffX * localImagePos.x), colorSpace.height - (int)(coeffY * localImagePos.y)-1);
				SetColor(res);
				if(isLeftMBtnDragging )
				{
					ApplyColor();
				}
				UpdateColorEditFields(false);
				UpdateColorSliders(false);
			}
			else if(rectAlpha.Contains(e.mousePosition))
			{
				float coeffX = alphaGradient.width / sizeCurr;
				float coeffY = alphaGradient.height / sizeCurr;
				Vector2 localImagePos = (mousePos - startPosAlpha);
				Color res = alphaGradient.GetPixel((int)(coeffX * localImagePos.x), colorSpace.height - (int)(coeffY * localImagePos.y)-1);
				Color curr = GetColor();
				curr.a = res.r;
				SetColor(curr);
				if(isLeftMBtnDragging)
				{
					ApplyColor();
				}
				UpdateColorEditFields(false);
				UpdateColorSliders(false);
			}
			else if(rectColorEdit.Contains(e.mousePosition))
			{
				UpdateColorEditFields(true);
				UpdateColorSliders(false);
			}
			else if(rectColorSlider.Contains(e.mousePosition))
			{
				UpdateColorEditFields(false);
				UpdateColorSliders(true);
			}
			else
			{
				SetColor(SelectedColor);

			}
		}
	}

	public void SetColor(Color color)
	{
		TempColor = color;
		if(txColorDisplay != null)
		{
			txColorDisplay.SetPixel(0, 0, color);
			txColorDisplay.Apply();
		}
	}

	public Color GetColor()
	{
		return TempColor;
	}

	public void SetTitle(string title, Color textColor)
	{
		this.Title = title;
		this.textColor = textColor;
	}

	public void ApplyColor()
	{
		SelectedColor = TempColor;
		if(receiver)
		{
			receiver.SendMessage(colorSetFunctionName, SelectedColor, SendMessageOptions.DontRequireReceiver);
			receiver1.SendMessage(colorSetFunctionName, SelectedColor, SendMessageOptions.DontRequireReceiver);
			receiver2.SendMessage(colorSetFunctionName, SelectedColor, SendMessageOptions.DontRequireReceiver);
			//Debug.Log(SelectedColor);
		}
	}

	void Update(){
		SetColor(SelectedColor);
    }

	public void CompareColor(Color blockcolor){
		float red =SelectedColor.r;
        float green=SelectedColor.g;
        float blue = SelectedColor.b;
        float red2 = blockcolor.r;
        float green2 =blockcolor.g;
        float blue2 =blockcolor.b;
        float[] colorLab = rgb2lab(red,green,blue);
		//Debug.Log("charactercolor"+red+" "+green+" "+blue);
        float[] ranLab = rgb2lab(red2,green2,blue2);
        float distance =Mathf.Sqrt(Mathf.Pow((colorLab[0]-ranLab[0]),2f)+ Mathf.Pow((colorLab[1]-ranLab[1]),2f)+ Mathf.Pow((colorLab[2]-ranLab[2]),2f));
		//Debug.Log("tilecolor"+red2+" "+green2+" "+blue2);
		//Debug.Log(distance);
        if(distance<52){
			Physics.IgnoreLayerCollision(8,9,true);

		}
		else Physics.IgnoreLayerCollision(8,9,false);
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

        //arr[0] = Y > 0.008856f ? (116.0f * FY -16.0f) : (903.3f *Y);
		arr[0] = 116.0f * FY -16.0f;
        arr[1] = 500f * (FX-FY);
        arr[2] = 200f * (FY-FZ);
        
        return arr;
    }
}
