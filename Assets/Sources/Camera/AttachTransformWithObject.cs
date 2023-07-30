using System;
using UnityEngine;

#pragma warning disable

public sealed class AttachTransformWithObject : MonoBehaviour, IModuleHandler
{
    private PlayerMovement _playerMovement;
    private float _smoothPositionCamera;
    private Vector3 _distanceFromObject;

    /// <exception cref="ArgumentNullException"></exception>
    public void OnInitialization(IParamArgs args)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(IParamArgs));

        _playerMovement = (PlayerMovement)args.GetContainer()?[0] ?? null;
        _smoothPositionCamera = (float)args.GetContainer()?[1];
        _distanceFromObject = (Vector3)args.GetContainer()?[2];

        if (_playerMovement == null)
            throw new ArgumentNullException(nameof(PlayerMovement));

        Debug.Log($"Module: {nameof(AttachTransformWithObject)} has been initialise.");
    }

    private void LateUpdate()
    {
        Vector3 directionToFromObject = _playerMovement.gameObject.transform.position +  _distanceFromObject;
        transform.position = Vector3.Lerp(transform.position, directionToFromObject, _smoothPositionCamera);
        transform.LookAt(_playerMovement.gameObject.transform.position);
    }
}