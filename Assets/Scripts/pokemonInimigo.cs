using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pokemonInimigo : MonoBehaviour {

    private pokemonPlayer pokemonPlayer;
    private battleController battleController;

    public string nome;
    public int level;
    public int xp;
    public float pvMax, pv, percentualVida;

    public string[] acoes;
    public int[] danoAcoes;
    public GameObject[] animacoes;

    private string fala;
    private Transform barraHP;
    private Vector3 vector3;

    private int rand, hit, idFase;

    private int idComando;

    // Use this for initialization
    void Start(){
        pokemonPlayer = FindObjectOfType(typeof(pokemonPlayer)) as pokemonPlayer;
        battleController = FindObjectOfType(typeof(battleController)) as battleController;
        barraHP = GameObject.Find("HP_Inimigo").transform;

        pv = pvMax;

        percentualVida = pv / pvMax;
        vector3 = barraHP.localScale;
        vector3.x = percentualVida;
        barraHP.localScale = vector3;
    }

    public void tomarDano(int hit){  // class que vai tirar dano do pokemon

        pv -= hit;

        if (pv <= 0){
            pv = 0;
            this.GetComponent<SpriteRenderer>().enabled = false;
        }

        percentualVida = pv / pvMax;
        vector3 = barraHP.localScale;
        vector3.x = percentualVida;
        barraHP.localScale = vector3;
    }

    public IEnumerator acaoInicial(){
        idComando = Random.Range(0, acoes.Length);
        yield return new WaitForSeconds(1f);
        StartCoroutine("comando", idComando);
    }

    public IEnumerator comando(int idAcao){

        switch(idAcao){

            case 0:
                StartCoroutine("aplicarDano");
            break;

            case 1:
                StartCoroutine("aplicarDano");
            break;

            case 2:
                StartCoroutine("aplicarDano");
            break;

            case 3:
                StartCoroutine("aplicarDano");
            break;

            case 4:
                StartCoroutine("aplicarDano");
            break;
        }

        yield return new WaitForSeconds(1f);
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
                pokemonPlayer.comandoInicial();
            break;
        }
    }

    public IEnumerator aplicarDano(){

        GameObject tempPrefab = Instantiate(animacoes[idComando]) as GameObject;
		tempPrefab.transform.position = pokemonPlayer.transform.position;
        
        hit = Random.Range(1, danoAcoes[idComando]);

        fala = nome + " usou " + acoes[idComando] + " e causou " + hit + " de dano.";
        StartCoroutine("dialogo", fala);

        yield return new WaitForSeconds(1f);

        Destroy(tempPrefab);
        pokemonPlayer.tomarDano(hit);
        idFase = 2;
    }

}