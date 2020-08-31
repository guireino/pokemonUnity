using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class battleController : MonoBehaviour{
	
	private pokemonPlayer pokemonPlayer;
	private pokemonInimigo pokemonInimigo;

    public int idFase;
    public string fala;
    public Text texto;

    private Transform Treinador, Pokemon, posA, posB;
    public GameObject menuA, menuB;

	// Use this for initialization
	void Start(){
		pokemonPlayer = FindObjectOfType(typeof(pokemonPlayer)) as pokemonPlayer;
        pokemonInimigo = FindObjectOfType(typeof(pokemonInimigo)) as pokemonInimigo;

        Treinador = GameObject.Find("Treinador").transform;

        posA = GameObject.Find("PosA").transform;
        posB = GameObject.Find("PosB").transform;

        Pokemon = pokemonPlayer.transform;

        menuA.SetActive(false);
        menuB.SetActive(false);

        idFase = 0;
        fala = "Um " + pokemonInimigo.nome + " apareceu!";
        StartCoroutine("dialogo", fala);
    }
	
	// Update is called once per frame
	void Update(){

        if(idFase == 1){   // fazendo animacao do treinador saindo e pokemon

            Treinador.GetComponent<Animator>().SetBool("lancar", true);

           float step = 2 * Time.deltaTime;
           Treinador.position = Vector3.MoveTowards(Treinador.position, posA.position, step);
           Pokemon.position = Vector3.MoveTowards(Pokemon.position, posB.position, step);
        }
    }

    public IEnumerator dialogo(string txt){

        int letra = 0;
        texto.text = "";

        while(letra <= txt.Length - 1){
            texto.text += txt[letra];
            letra += 1;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(1);
        idFase += 1;

        switch(idFase){
            case 1:
               fala = "Vá " + pokemonPlayer.nome + "!";
               StartCoroutine("dialogo", fala);
            break;

            case 2:
                pokemonPlayer.comandoInicial();
            break;
        }
    }

    public void LUTAR(){
        menuA.SetActive(false);
        menuB.SetActive(true);
        pokemonPlayer.renomearBotoes();
    }

    public void comando(int idComando){
        pokemonPlayer.StartCoroutine("comando", idComando);
        menuB.SetActive(false);
    }

}