using System.Reflection;
 
namespace UnityEngine.Rendering.Universal
{
    //Be careful! If during development the Unity team changes the name of the "m_ApplyToSortingLayers" variable, here it will also need to be changed.
    public static class Light2DLayersMaskAccessExtension
    {
        public static int[] GetLayers(this UnityEngine.Rendering.Universal.Light2D light)
        {
            FieldInfo targetSortingLayersField = typeof(UnityEngine.Rendering.Universal.Light2D).GetField("m_ApplyToSortingLayers",
                                                                       BindingFlags.NonPublic |
                                                                       BindingFlags.Instance);
            int[] mask = targetSortingLayersField.GetValue(light) as int[];
            return mask;
        }
        public static void SetLayers(this UnityEngine.Rendering.Universal.Light2D light, int[] mask)
        {
            FieldInfo targetSortingLayersField = typeof(UnityEngine.Rendering.Universal.Light2D).GetField("m_ApplyToSortingLayers",
                                                                       BindingFlags.NonPublic |
                                                                       BindingFlags.Instance);
            targetSortingLayersField.SetValue(light, mask);
        }
    }
}