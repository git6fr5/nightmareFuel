using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {CharacterMovement}: ";
    private bool DEBUG_init = false;
    //private bool DEBUG_clips = false;
    //private bool DEBUG_sound = false;

    /* --- Components --- */

    // Animation
    public AnimationClip idleAnim;
    public AnimationClip runningAnim;
    public AnimationClip hurtAnim;
    public AnimationClip deathAnim;

    public Animator animator;
    private RuntimeAnimatorController animatorController;
    private List<AnimationClip> animationClips = new List<AnimationClip>();
    private AnimationClip[] controllerClips;

    // Audio
    public AudioClip hurtAudio;
    public AudioClip deathAudio;
    public AudioClip aggroAudio;
    public AudioClip idleAudio;
    public AudioClip collectAudio;

    public AudioSource audioSource;

    // Model
    public Skeleton skeleton;
    public Particle[] particles;

    /* --- Internal Variables --- */
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public bool hurt = false;
    [HideInInspector] public bool death = false;
    [HideInInspector] public bool aggro = false;
    [HideInInspector] public bool collect = false;

    private bool overriding = false;
    private string prevAnim;
    private float hurtDuration = 0.2f;
    private float deathDuration = 0.4f;

    private float duration;
    private float elapsedDuration = 0f;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
    }

    void Update()
    {
        if (!overriding)
        {
            SetAnimation();
        }
    }

    void FixedUpdate()
    {
        if (overriding)
        {
            elapsedDuration = elapsedDuration + Time.fixedDeltaTime;
            OverrideAnimationForDuration(elapsedDuration, duration);
        }
    }

    public void SetAnimation()
    {
        bool animated = false;

        /* --- High Priority --- */
        if (death && deathAnim)
        {
            animator.Play(deathAnim.name);
            overriding = true; duration = deathDuration;
            animated = true;
        }
        else if (hurt && hurtAnim)
        {
            animator.Play(hurtAnim.name);
            overriding = true; duration = hurtDuration;
            animated = true;
        }
        if (animated) { return; }

        /* --- Mid Priority --- */
        if (speed != 0 && runningAnim)
        {
            animator.Play(runningAnim.name);
            animated = true;
        }
        if (animated) { return; }

        /* --- Low Priority --- */
        animator.Play(idleAnim.name);
        return;
    }

    public void OverrideAnimation()
    {
        string currAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        //curr_anim = animator.GetCurrentAnimatorStateInfo();
        if (currAnim != prevAnim)
        {
            overriding = false;
        }
        print(prevAnim + ", " + currAnim);
        prevAnim = currAnim;
    }

    public void OverrideAnimationForDuration(float elapsedDuration, float duration)
    {
        if (elapsedDuration > duration)
        {
            overriding = false;
        }
    }

    public void PlaySound()
    {
        bool sounded = false;

        /*--- High Priority ---*/
        //print("attempting to play sound");
        if (death && deathAudio)
        {
            audioSource.clip = deathAudio;
            audioSource.Play();
            sounded = true;
        }
        else if (hurt && hurtAudio)
        {
            audioSource.clip = hurtAudio;
            audioSource.Play();
            sounded = true;
        }
        else if (aggro && aggroAudio)
        {
            audioSource.clip = aggroAudio;
            audioSource.Play();
            sounded = true;
        }
        else if (collect && collectAudio)
        {
            print("hello");
            audioSource.clip = collectAudio;
            audioSource.Play();
            sounded = true;
        }

        DisableHighPrio();
        if (sounded) { return; }

        /*--- Low Priority ---*/

        audioSource.clip = idleAudio;
        audioSource.Play();
        return;
    }

    private void DisableHighPrio()
    {
        hurt = false;
        death = false;
        collect = false;
    }
}
