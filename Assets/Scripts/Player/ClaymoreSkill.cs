using UnityEngine;
using UnityEngine.UI;

public class ClaymoreSkill : MonoBehaviour
{
    private LevelManager theLevelManager;

    public AudioSource ClaymoreSound;
    public AudioClip ClaymoreSoundClip;

    public GameObject StartingPrefab;  // The first prefab to instantiate
    public GameObject ClaymorePrefab;  // The claymore prefab to instantiate
    public Transform SkillPoint;       // The transform where the prefabs will be instantiated

    private void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    // This method will be called when the button is clicked
    public void OnSkillButtonClick()
    {
        if (theLevelManager != null)
        {
            ClaymoreSound.PlayOneShot(ClaymoreSoundClip);

            // Instantiate the StartingPrefab first at the SkillPoint's position and rotation
            Instantiate(StartingPrefab, SkillPoint.position, StartingPrefab.transform.rotation);

            // Instantiate the ClaymorePrefab at the SkillPoint's position and rotation
            Instantiate(ClaymorePrefab, SkillPoint.position, SkillPoint.rotation);


        }
    }
}
