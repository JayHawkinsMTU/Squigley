using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NewStartButton : Button, IDataPersistence
{
    [SerializeField] VisualEffect fadeOut;
    TMP_Text ui;
    Wait waitOne = new Wait(0.0725f);
    Wait waitTwo = new Wait(1.5f);
    public string sceneName = "Overworld";
    [SerializeField] bool loadData = true;
    private bool active;

    public override void Activate()
    {
        active = true;
        fadeOut.StartEffect(1.5f, 1);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            waitOne.Iterate();
            waitTwo.Iterate();
            if(waitOne.Complete())
            {
                if(ui.color == Color.gray || ui.color == Color.yellow)
                {
                    ui.color = Color.white;
                }
                else if(ui.color == Color.white)
                {
                    ui.color = Color.gray;
                }
            }
            if(waitTwo.Complete())
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
        }
    }
    public void SaveData(ref SaveData data)
    {
        return;
    }
    public void LoadData(SaveData data)
    {
        if(loadData) this.sceneName = data.sceneName;
    }
}
