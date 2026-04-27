using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 8f;
    public float jumpPower = 5f;
    public bool isGrounded;

    public Transform feet;
    public float radius = 0.5f;
    public LayerMask ground;

    public float dashPower = 2000f;
    public float dashCD = 1.5f;
    private float dashTimer = 0;
    public bool isDashing = false;

    public AudioClip jumpClip;
    public AudioClip dashClip;
    public AudioSource SFXPlayer;

    public Slider dashSlider;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(feet.position, radius, ground);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (SettingsBridge.Instance != null)
        {
            SFXPlayer.volume = SettingsBridge.Instance.sfxVolume;
        }

        Vector3 moveVector = (transform.right * x + transform.forward * z).normalized;

        if (!isDashing)
        {
            rb.velocity = new Vector3(moveVector.x * speed, rb.velocity.y, moveVector.z * speed);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            SFXPlayer.clip = jumpClip;
            SFXPlayer.Play();
            Jump();
        }

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

        UpdateDashUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
        {
            SFXPlayer.clip = dashClip;
            SFXPlayer.Play();
            StartCoroutine(Dash());
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        dashTimer = dashCD;

        float a = Input.GetAxisRaw("Horizontal");
        float b = Input.GetAxisRaw("Vertical");

        Vector3 dashVector = (transform.right * a + transform.forward * b).normalized;

        if (dashVector == Vector3.zero)
        {
            dashVector = transform.forward * -1;
        }

        rb.AddForce(dashVector * dashPower, ForceMode.VelocityChange);

        yield return new WaitForSeconds(0.1f);
        isDashing = false;
    }
    
    void UpdateDashUI()
    {
        if (dashSlider != null)
        {
            float progress = 1 - (dashTimer / dashCD);
            dashSlider.value = Mathf.Clamp(progress, 0f, 1f);
        }
    }
}
