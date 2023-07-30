using System;
using UnityEngine;

#pragma warning disable

public enum AnimationType : byte
{
    PlayAnimationNone = 0x00,
    PlayAnimationIdle = 0x01,
    PlayAnimationRun = 0x02,
    PlayAnimationAttack = 0x03,
    PlayAnimationDeath = 0x04
}

[RequireComponent(typeof(Animator))]
public sealed class PlayerAnimation : MonoBehaviour, IModuleHandler
{
    private Animator _animator;
    private float _animationPlayingSpeed;
    private AnimationType _lastAnimationIvoke;

    /// <exception cref="ArgumentNullException"></exception>
    public void OnInitialization(IParamArgs args)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(IParamArgs));

        _animator = (Animator)args.GetContainer()?[0] ?? null;

        if (_animator == null)
            throw new ArgumentNullException(nameof(Animator));

        _animationPlayingSpeed = (float)args.GetContainer()?[1];

        if (_animationPlayingSpeed <= 0.0f)
            Debug.LogWarning("Animation speed is less than or equal to zero!");

        _lastAnimationIvoke = AnimationType.PlayAnimationNone;

        Debug.Log($"Module: {nameof(PlayerAnimation)} has been initialise.");
    }

    public void PlayAnimation(AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.PlayAnimationIdle:
                _animator.SetFloat("idleSpeed", _animationPlayingSpeed);
                _animator.Play("idle");
                break;

            case AnimationType.PlayAnimationRun:
                _animator.SetFloat("runSpeed", _animationPlayingSpeed);
                _animator.Play("run");
                break;

            case AnimationType.PlayAnimationAttack:
                _animator.SetFloat("attackSpeed", _animationPlayingSpeed);
                _animator.Play("attack");
                break;
            case AnimationType.PlayAnimationDeath:
                _animator.Play("death");
                break;
        }

        _lastAnimationIvoke = animationType;
    }

    public AnimationType GetRunningAnimation()
    {
        return _lastAnimationIvoke;
    }
}