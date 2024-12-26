using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMatrix : MonoBehaviour
{
    [SerializeField] Row[] rows;
    [SerializeField] Button backButton;
    private int xIndex = 0;
    private int yIndex = 0;
    public int startX, startY;

    public AudioSource audioSource;
    [SerializeField] AudioClip navigateSound;

    void LoopIndex() //Makes the indexes loop around on each axis.
    {
        if(yIndex >= rows.Length)
        {
            yIndex = 0;
        }
        else if(yIndex < 0)
        {
            yIndex = rows.Length - 1;
        }

        if(xIndex >= rows[yIndex].buttons.Length)
        {
            xIndex = 0;
        }
        else if(xIndex < 0)
        {
            xIndex = rows[yIndex].buttons.Length - 1;
        }

        
    }

    public void ToButton( int newX, int newY )
    {
        rows[yIndex].buttons[xIndex].Unhover();
        rows[newY].buttons[newX].Hover();
    }
    public void Navigate( char dir ) //Navigates the matrix.
    {
        int initIndexX = xIndex;
        int initIndexY = yIndex;
        if(dir == 'N')
        {
            yIndex--;
        }
        else if(dir == 'S')
        {
            yIndex++;
        }
        else if(dir == 'W')
        {
            xIndex--;
        }
        else if(dir == 'E')
        {
            xIndex++;
        }
        else
        {
            Debug.LogError("Invalid direction in ButtonMatrix: \'" + dir + "\'.");
            return;
        }
        LoopIndex();
        if(rows[yIndex].buttons[xIndex] != null)
        {
            rows[initIndexY].buttons[initIndexX].Unhover();
            rows[yIndex].buttons[xIndex].Hover();
            audioSource.PlayOneShot(navigateSound, 0.5f);
        }
        else
        {
            xIndex = initIndexX;
            yIndex = initIndexY;
        }
    }

    void Start()
    {
        rows[yIndex].buttons[xIndex].Unhover();
        xIndex = startX;
        yIndex= startY;
        audioSource = GetComponent<AudioSource>();
        rows[yIndex].buttons[xIndex].Hover();
    }

    void OnEnable()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameInput.UIUp()) Navigate('N');
        if(GameInput.UILeft()) Navigate('W');
        if(GameInput.UIRight()) Navigate('E');
        if(GameInput.UIDown()) Navigate('S');
        if(GameInput.Pause() && backButton != null) backButton.Activate();

        //Activation
        if(GameInput.Interact(1) || GameInput.Interact(2))
        {
            rows[yIndex].buttons[xIndex].Activate();
            audioSource.PlayOneShot(rows[yIndex].buttons[xIndex].activationSound, 0.5f);
        }
    }
}
