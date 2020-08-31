using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices;

public class pokemonPlayer : MonoBehaviour{

	private pokemonInimigo pokemonInimigo;
	private battleController battleController;

	public string nome;
	public int level;
	public int xp;
	public float pvMax, pv, percentual;

	public string[] acoes;
	public int[] danoAcoes;
	public GameObject[] animacoes;

	private string fala;
	private Transform barraHP, barraXP;
    private Vector3 vector3;
    private GameObject botaoA, botaoB, botaoC, botaoD;

    private int idComando;
    private int hit;
    private int idFase;

	// Use this for initialization
	void Start(){

		pokemonInimigo = FindObjectOfType(typeof(pokemonInimigo)) as pokemonInimigo;
		battleController = FindObjectOfType(typeof(battleController)) as battleController;

        pv = pvMax;
        barraHP = GameObject.Find("HP_Player").transform;
        barraXP = GameObject.Find("XP_Player").transform;

        percentual = pv / pvMax;
        vector3 = barraHP.localScale;
        vector3.x = percentual;
        barraHP.localScale = vector3;

        percentual = xp / 100;
        vector3 = barraXP.localScale;
        vector3.x = percentual;
        barraXP.localScale = vector3;
	}

	public void Update(){
		print(idFase);
	}
	
	public void tomarDano(int hit){

        pv -= hit;

        if(pv <= 0){
            pv = 0;
            GetComponent<SpriteRenderer>().enabled = false;
        }

        percentual = pv / pvMax;
        vector3 = barraHP.localScale;
        vector3.x = percentual;

        barraHP.localScale = vector3;
    }

    public void renomearBotoes(){

    	botaoA = GameObject.Find("Text_A");
    	botaoB = GameObject.Find("Text_B");
    	botaoC = GameObject.Find("Text_C");
    	botaoD = GameObject.Find("Text_D");

    	botaoA.GetComponent<Text>().text = acoes[0];
    	botaoB.GetComponent<Text>().text = acoes[1];
    	botaoC.GetComponent<Text>().text = acoes[2];
    	botaoD.GetComponent<Text>().text = acoes[3];
    }

    public IEnumerator comando(int idAcao){

    	switch(idAcao){

	    	case 1:
	    		idComando = 0;
	    		fala = nome + " use " + acoes[idComando] + "!";
	    		StartCoroutine("dialogo", fala);
	    	break;

	    	case 2:
	    		idComando = 1;
	    		fala = nome + " use " + acoes[idComando] + "!";
	    		StartCoroutine("dialogo", fala);
	    	break;

	    	case 3:
	    		idComando = 2;
	    		fala = nome + " use " + acoes[idComando] + "!";
	    		StartCoroutine("dialogo", fala);
	    	break;
                 
	    	case 4:
	    		idComando = 3;
	    		fala = nome + " use " + acoes[idComando] + "!";
	    		StartCoroutine("dialogo", fala);
	    	break;
    	}

        idFase = 1;
    	return null;
    }

    public IEnumerator dialogo(string txt){

        int letra = 0;
        battleController.texto.text = "";

        while (letra <= txt.Length - 1){
            battleController.texto.text += txt[letra];
            letra += 1;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(1f);

        switch (idFase){

            case 1:
                StartCoroutine("aplicarDano");
            break;

            case 2:
                pokemonInimigo.StartCoroutine("acaoInicial");
            break;

            case 3:
                battleController.menuA.SetActive(true);
            break;

            case 4:
            	fala = pokemonInimigo.nome + " foi derrotado!";
				StartCoroutine("dialogo", fala);
				idFase = 5;
            break;

            case 5:
            	StartCoroutine("ganhaXP", pokemonInimigo.xp);
            	idFase = 6;
            break;
        }
    }

    public IEnumerator aplicarDano(){

		GameObject tempPrefab = Instantiate(animacoes[idComando]) as GameObject;
		tempPrefab.transform.position = pokemonInimigo.transform.position;

    	hit = Random.Range(1, danoAcoes[idComando]);
    	fala = nome + " usou " + acoes[idComando] + " e causou " + hit + " de dano.";
    	StartCoroutine("dialogo", fala);

    	yield return new WaitForSeconds(1f);
    	pokemonInimigo.tomarDano(hit);
		Destroy(tempPrefab);

    	if(pokemonInimigo.pv <= 0){
    		idFase = 4;
    	}else{
    		idFase = 2;
    	}
    }

    public void comandoInicial(){
        fala = "O que fazer?";
        StartCoroutine("dialogo", fala);
        idFase = 3;
    }

    public IEnumerator ganhaXP(int xpGanho){

    	fala = nome + "recebeu " + xpGanho + " => xp.";
    	StartCoroutine("dialogo", fala);

    	xp += xpGanho;

    	percentual = xp / 100f;
        vector3 = barraXP.localScale;
        vector3.x = percentual;
        barraXP.localScale = vector3;

    	idFase = 5;

    	return null;
    }

}