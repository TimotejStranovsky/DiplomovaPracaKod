using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] float maxTime;
    [SerializeField] float remainingTime;
    public string enemyName;
    public int reward;
    [SerializeField] EnemyIndex enemyIndex;
    [SerializeField] Image image;
    [SerializeField] Image cage;
    [SerializeField] TMP_Text textTimer;
    [SerializeField] TMP_Text text;
    public Sprite ghostImg;
    public bool ded = false;
    public bool win = false;
    //public SkinnedMeshRenderer ghostBody;
    //add pokedex addition

    private void Start()
    {
        enemyIndex = GameObject.FindGameObjectWithTag("Player").GetComponent<EnemyIndex>();
        image = GameObject.FindGameObjectWithTag("UI Image").GetComponent<Image>();
        textTimer = GameObject.FindGameObjectWithTag("UI Text").GetComponent<TextMeshProUGUI>();
        text = GameObject.FindGameObjectWithTag("Respawn").GetComponent<TextMeshProUGUI>();
        cage = GameObject.FindGameObjectWithTag("Cage Image").GetComponent<Image>();
        text.text = ""+health;
        maxHealth = health;
    }

    public void GetHit(float amount)
    {
        health -= amount;
        text.text = string.Format("{0:#.00}", health);
        if (cage == null)
            cage = GameObject.FindGameObjectWithTag("Cage Image").GetComponent<Image>();

        cage.fillAmount =  (maxHealth - health) / maxHealth;

        if (health <= 0 && !ded)
        {
            //something something pokedex
            enemyIndex.SubmitEntry(enemyName);
            win = true;
            ded = true;
            SceneManager.LoadSceneAsync("VictoryScreen",LoadSceneMode.Additive);
        }
    }

    public void ChangeMaxTime(float amount)
    {
        maxTime += amount;
        remainingTime += amount;
    }

    public void ChangeReward(int amount)
    {
        reward += amount;
    }

    private void FixedUpdate()
    {
        if(remainingTime < 0f && !ded)
        {
            win = false;
            ded = true;
            SceneManager.LoadScene("FailureScreen", LoadSceneMode.Additive);
        }
        remainingTime -= Time.deltaTime;

        //float colour = remainingTime / maxTime;
        //ghostBody.material.color = new Color(1, colour, colour, 1);

        if (image == null)
            image = GameObject.FindGameObjectWithTag("UI Image").GetComponent<Image>();
        if (textTimer == null)
            textTimer = GameObject.FindGameObjectWithTag("UI Text").GetComponent<TextMeshProUGUI>();
        if (remainingTime > 0f) 
        {
            image.fillAmount = remainingTime/maxTime;
            textTimer.text = remainingTime.ToString("F2");//System.TimeSpan.FromSeconds(remainingTime).ToString("ss");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(reward);
        }
    }
}
