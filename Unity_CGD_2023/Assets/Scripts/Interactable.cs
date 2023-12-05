using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Interactable : MonoBehaviour
{
    public string interactableName = "Interactable"; // Name for UI display
    public float interactionCooldown = 1.0f;
    private bool canInteract = true;

    public GameObject vendorScreen;

    [SerializeField]
    PlayerController playerController;

    private void Start()
    {

    }

    public void Interact()
    {
        if (canInteract)
        {
            Debug.Log($"Interacting with {interactableName}");

            switch (gameObject.tag)
            {
                case "Vendor":
                    OpenVendor();
                    break;
                // Add more cases for other interactable types if needed
                default:
                    Debug.LogWarning("Unhandled interactable type.");
                    break;
            }

            StartCoroutine(InteractionCooldown());
        }
    }

    private void Update()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (Input.GetButtonDown("Cancel"))
        {
            CloseVendor();
        }
    }

    private void OpenVendor()
    {
        vendorScreen.SetActive(true);
        playerController.SwitchActionMapToMenu();
        playerController.rb.velocity = playerController.noMove;
        playerController.MoveForce2 = playerController.noMove;
    }
    private void CloseVendor()
    {
        vendorScreen.SetActive(false);
        playerController.SwitchActionMapToPlayer();
    }

    private System.Collections.IEnumerator InteractionCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactionCooldown);
        canInteract = true;
    }
}
