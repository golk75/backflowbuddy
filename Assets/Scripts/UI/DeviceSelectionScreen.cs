using UnityEngine;
using UnityEngine.UIElements;

public class DeviceSelectionScreen : VisualElement
{

    public new class UxmlFactory : UxmlFactory<DeviceSelectionScreen, UxmlTraits> { }

    public DeviceSelectionScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}