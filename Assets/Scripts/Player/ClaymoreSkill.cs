using UnityEngine;
using UnityEngine.UI;

public class ClaymoreSkill : MonoBehaviour
{
    public GameObject StartingPrefab;  // The first prefab to instantiate
    public GameObject ClaymorePrefab;  // The claymore prefab to instantiate
    public Transform SkillPoint;       // The transform where the prefabs will be instantiated

    // This method will be called when the button is clicked
    public void OnSkillButtonClick()
    {
        // Instantiate the StartingPrefab first at the SkillPoint's position and rotation
        Instantiate(StartingPrefab, SkillPoint.position, StartingPrefab.transform.rotation);

        // Instantiate the ClaymorePrefab at the SkillPoint's position and rotation
        Instantiate(ClaymorePrefab, SkillPoint.position, SkillPoint.rotation);
    }
}
