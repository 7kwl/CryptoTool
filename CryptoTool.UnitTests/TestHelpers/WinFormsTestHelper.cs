using System.Reflection;

namespace CryptoTool.UnitTests.TestHelpers;

internal static class WinFormsTestHelper
{
    public static TControl GetPrivateField<TControl>(object instance, string fieldName)
        where TControl : class
    {
        var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        var value = field!.GetValue(instance);
        return Assert.IsType<TControl>(value);
    }

    public static void ClickButton(object instance, string fieldName)
    {
        var button = GetPrivateField<Button>(instance, fieldName);
        button.PerformClick();
    }

    public static void SetPrivateField(object instance, string fieldName, object? value)
    {
        var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        field!.SetValue(instance, value);
    }
}
