public class ContraintCreatorRegistry
{
    private static Dictionary<Type, ContraintCreator> _creators = new Dictionary<Type, ContraintCreator>();
    public static void Register(Type type, ContraintCreator creator) => _creators.Add(type, creator);
    public static ContraintCreator GetCreator(Type type) => _creators[type];
}