using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{

    /* --- Components --- */

    // Animation
    public Animator animator;
    public AnimationClip idleAnim;
    public AnimationClip runningAnim;
    public AnimationClip hurtAnim;
    public AnimationClip deathAnim;

    // Audio
    public AudioSource audioSource;
    public AudioClip hurtAudio;
    public AudioClip deathAudio;
    public AudioClip aggroAudio;
    public AudioClip idleAudio;
    public AudioClip collectAudio;

    // Material
    public SpriteRenderer spriteRenderer;
    public Material hurtMaterial;
    public Material deathMaterial;

    // Model
    public Skeleton skeleton;
    public Particle[] particles;

    /* --- Internal Variables --- */

    // Controls
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public bool hurt = false;
    [HideInInspector] public bool death = false;
    [HideInInspector] public bool aggro = false;
    [HideInInspector] public bool collect = false;

    private bool overriding = false;
    private float hurtDuration = 0.2f;
    private float deathDuration = 0.4f;

    private float duration;
    private float elapsedDuration = 0f;

    /* --- Unity Methods --- */
    void Update()
    {
        SetAnimation();
        SetMaterial();
        SetAudio();
    }

    public void SetAnimation()
    {
        bool animated = false;

        if (characterMovement.isMobile && characterMovement.currSpeed != 0 && runningAnim)
        {
            animator.Play(runningAnim.name);
            animated = true;
            return;
        }
        animator.Play(idleAnim.name);
        return;
    }

    public void SetMaterial()
    {
        // death
        // hurt
    }

    public void SetAudio()
    {
        return;
    }

}
