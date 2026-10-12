using UnityEngine;
using TMPro;
using System.Collections;

public class TargetScore : MonoBehaviour
{
    [Header("Setari UI")]
    public TextMeshProUGUI textScor;

    [Header("Efect vizual")]
    public Material materialSucces;
    private Material materialOriginal;
    private Renderer targetRenderer;

    [Header("Jucator pentru a calcula distanta")]
    public Transform player;

    private int scorTotal = 0;

    void Start()
    {
        //componenta care coloreaza cubul
        targetRenderer = GetComponent<Renderer>();
        materialOriginal = targetRenderer.material; //culoarea gri initiala
    }

    // functia apelata automat cand ceva intra in triggerul cubului
    void OnTriggerEnter(Collider other)
    {
        // verificam daca ce a intrat are eticheta "Minge"
        if (other.CompareTag("Minge"))
        {
            CalculareScor();
            StartCoroutine(EfectVizual());

            // respawn minge
            other.transform.position = new Vector3(0f, 1f, 1f);

            // opreste miscarea mingii
            other.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    void CalculareScor()
    {
        // calc distanta in m dintre jucator si tinta
        float distanta = Vector3.Distance(player.position, transform.position);

        // transf dist in puncte (4 m=40 p)
        // Mathf.RoundToInt pt a rotunji la un nr intreg
        int puncteCastigate = Mathf.RoundToInt(distanta * 10);

        scorTotal = scorTotal + puncteCastigate;

        // act textul pe ecran
        if (textScor != null)
        {
            textScor.text = "Scor: " + scorTotal + "\n(+ " + puncteCastigate + " pt distanta!)";
        }
    }

    // tinta se coloreaza pt 0.5 secunde
    IEnumerator EfectVizual()
    {
        if (materialSucces != null)
        {
            targetRenderer.material = materialSucces;
            yield return new WaitForSeconds(0.5f);
            targetRenderer.material = materialOriginal;
        }
    }
}