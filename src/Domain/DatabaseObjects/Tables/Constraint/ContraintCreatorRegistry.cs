public class ContraintCreatorRegistry
{
    private static Dictionary<Type, ContraintCreator> creators = new Dictionary<Type, ContraintCreator>();

    public static void Register(Type type, ContraintCreator creator) => creators.Add(type, creator);

    public static ContraintCreator GetCreator(Type type) => creators[type];
}
