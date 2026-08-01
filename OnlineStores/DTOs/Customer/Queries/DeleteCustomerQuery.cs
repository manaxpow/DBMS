public record DeleteCustomerQuery(
    bool? Force = false,
    bool? AnonymizeData = false
);
