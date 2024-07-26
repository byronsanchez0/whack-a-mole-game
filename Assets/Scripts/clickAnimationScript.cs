using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab;
    public float laserLength = 0.3f;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the z-position is set to 0 for 2D

            // Instantiate the prefab at the mouse position
            GameObject spawnedObject = Instantiate(prefab, mousePosition, Quaternion.identity);

            // Adjust the sorting layer/order in layer if needed
            SpriteRenderer spriteRenderer = spawnedObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Foreground"; // Ensure this layer exists
                spriteRenderer.sortingOrder = 100; // Adjust this value as needed
            }

            // Start the animation (if not already automatically started)
            Animator animator = spawnedObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("ClickAnimation"); // Ensure this matches the name of your animation state
            }

            // Optionally destroy the object after the animation duration
            float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(spawnedObject, animationDuration);

        }
    }
}

