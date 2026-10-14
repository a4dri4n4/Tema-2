using UnityEngine;
using TMPro;
using System.Collections;

public class TargetScore : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textScor;
    public TextMeshProUGUI textCombo;
    public TextMeshProUGUI textComboLost;

    [Header("Efecte")]
    public Material materialSucces;
    public AudioClip sunetSucces;
    public ParticleSystem efectParticule;

    [Header("Jucator")]
    public Transform player;

    private Material materialOriginal;
    private Renderer targetRenderer;
    private AudioSource audioSource;
    private int scorTotal = 0;
    private int combo = 0;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        materialOriginal = targetRenderer.material;
        audioSource = GetComponent<AudioSource>();
        textComboLost.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minge")){

        combo++;
        ActualizeazaCombo();
        CalculareScor();

        StartCoroutine(EfectVizual());
        audioSource.pitch = Mathf.Min(1f + combo * 0.1f, 2f);
        audioSource.PlayOneShot(sunetSucces);
        efectParticule.Play();

        other.GetComponent<MingeReset>().Respawn();
        }
    }

    void CalculareScor()
    {
        float distanta = Vector3.Distance(player.position, transform.position);
        int puncte = Mathf.RoundToInt(distanta * 10) * combo;
        scorTotal += puncte;

        textScor.text = "Scor: " + scorTotal + "\n(+ " + puncte + " pt)";
    }

    void ActualizeazaCombo()
    {
        textCombo.text = combo > 0 ? "COMBO x" + combo : "";
    }

    public void ResetCombo()
    {

        if (combo > 5)
        StartCoroutine(AfiseazaComboLost());
        combo = 0;
        ActualizeazaCombo();
        
    }

    IEnumerator EfectVizual()
    {
        targetRenderer.material = materialSucces;
        yield return new WaitForSeconds(0.5f);
        targetRenderer.material = materialOriginal;
    }

    IEnumerator AfiseazaComboLost()
    {
        textComboLost.text = "COMBO LOST!";
        yield return new WaitForSeconds(1.5f);
        textComboLost.text = "";
    }
}