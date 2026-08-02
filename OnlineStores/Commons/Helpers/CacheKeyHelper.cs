public static class CacheKeys
{
    private const string Prefix = "OnlineStore";

    public static class Product
    {
        public static string ById(Guid id)
            => $"{Prefix}:Products:{id}";

        public static string List(
            int page,
            int pageSize,
            string? search = null)
            => $"{Prefix}:Products:List:{page}:{pageSize}:{search ?? "all"}";
    }

    public static class Customer
    {
        public static string ById(Guid id)
            => $"{Prefix}:Customers:{id}";

        public static string Summary()
            => $"{Prefix}:Customers:Summary";

        public static string List(
            int page,
            int pageSize,
            string? search = null)
            => $"{Prefix}:Customers:List:{page}:{pageSize}:{search ?? "all"}";
    }

    public static class Order
    {
        public static string ById(Guid id)
            => $"{Prefix}:Orders:{id}";

        public static string List(
            Guid? customerId,
            int page,
            int pageSize)
            => $"{Prefix}:Orders:{customerId}:{page}:{pageSize}";
    }

    public static class Dashboard
    {
        public static string Summary()
            => $"{Prefix}:Dashboard:Summary";
    }
}
