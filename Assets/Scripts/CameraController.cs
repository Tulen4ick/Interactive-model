using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{

    public float rotationSpeed = 5.0f;
    public float zoomSpeed = 10.0f;
    public float verticalMoveSpeed = 5.0f;
    public float minVerticalAngle = -80.0f;
    public float maxVerticalAngle = 80.0f;
    public float minZoomDistance = 1.0f;
    public float maxZoomDistance = 20.0f;

    private Transform target;
    private Transform highlight;
    private Vector3 focusPoint;
    private float currentDistance = 10.0f;
    private float currentVerticalAngle = 30.0f;
    private float currentHorizontalAngle = 0.0f;

    void Update()
    {
        OutlineHighliting();
        HandleSelection();
        HandleRotation();
        HandleVerticalMovement();
        HandleZoom();
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void OutlineHighliting()
    {
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitinfo))
            {
                highlight = hitinfo.transform;
                if (hitinfo.transform.tag == "Selectable" && highlight != target)
                {
                    if (highlight.gameObject.GetComponent<Outline>() != null)
                    {
                        highlight.gameObject.GetComponent<Outline>().enabled = true;
                    }
                    else
                    {
                        Outline outline = highlight.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                        outline.OutlineColor = Color.blue;
                        highlight.gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
                        outline.OutlineWidth = 7f;
                    }
                }
                else
                {
                    highlight = null;
                }
            }
        }
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitinfo))
                {
                    if (hitinfo.transform.tag == "Selectable" && hitinfo.transform != target)
                    {
                        hitinfo.transform.gameObject.GetComponent<Outline>().enabled = true;
                        focusPoint = hitinfo.transform.position;
                        Vector3 toCamera = transform.position - focusPoint;
                        if (target != null)
                        {
                            target.gameObject.GetComponent<Outline>().enabled = false;
                        }
                        else
                        {
                            currentDistance = toCamera.magnitude / 2;
                        }
                        target = hitinfo.transform;
                        highlight = null;
                        currentVerticalAngle = Vector3.Angle(Vector3.up, toCamera) - 90f;
                        currentHorizontalAngle = transform.eulerAngles.y;
                    }
                    else if (hitinfo.transform.tag != "Selectable")
                    {
                        /*currentDistance = 1;
                        focusPoint = transform.position;*/
                        if (target)
                        {
                            target.gameObject.GetComponent<Outline>().enabled = false;
                            focusPoint = Vector3.zero;
                            currentDistance *= 2;
                            target = null;
                        }
                    }
                }
                else
                {
                    /*currentDistance = 1;
                    focusPoint = transform.position;*/
                    if (target)
                    {
                        target.gameObject.GetComponent<Outline>().enabled = false;
                        focusPoint = Vector3.zero;
                        currentDistance *= 2;
                        target = null;
                    }

                }
            }
        }
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            currentHorizontalAngle += Input.GetAxis("Mouse X") * rotationSpeed;
            currentVerticalAngle -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);
        }
    }

    private void HandleVerticalMovement()
    {
        if (Input.GetMouseButton(2))
        {
            float verticalMove = Input.GetAxis("Mouse Y") * verticalMoveSpeed;
            focusPoint += Vector3.up * verticalMove;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            currentDistance -= scroll * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minZoomDistance, maxZoomDistance);
        }
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
        Vector3 direction = rotation * Vector3.forward;
        Vector3 desiredPosition = focusPoint - direction * currentDistance;

        transform.position = desiredPosition;
        transform.LookAt(focusPoint);
    }
}

