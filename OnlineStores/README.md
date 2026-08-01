## 1. Authentication

| Method | Endpoint            | Path parameters | Query / filters & Pagination | Request body           |
| ------ | ------------------- | --------------- | ---------------------------- | ---------------------- |
| POST   | /auth/login         | None            | None                         | LoginRequest           |
| POST   | /auth/register      | None            | None                         | RegisterRequest        |
| POST   | /auth/logout        | None            | allDevices                   | Optional LogoutRequest |
| POST   | /auth/refresh-token | None            | None                         | RefreshTokenRequest    |
| GET    | /users/me           | None            | includeStore, includeRole    | None                   |

## 2. Dashboard (Overview)

| Method | Endpoint          | Path parameters | Query / filters & Pagination | Request body |
| ------ | ----------------- | --------------- | ---------------------------- | ------------ |
| GET    | /overview/summary | None            | from, to, timezone, currency | None         |

## 3. Store

| Method | Endpoint    | Path parameters | Query / filters & Pagination  | Request body        |
| ------ | ----------- | --------------- | ----------------------------- | ------------------- |
| GET    | /store      | None            | includeOwner, includeSettings | None                |
| PUT    | /store      | None            | None                          | UpdateStoreRequest  |
| POST   | /store/logo | None            | replaceExisting               | multipart/form-data |

## 4. Products

| Method | Endpoint                     | Path parameters | Query / filters & Pagination                                                                                    | Request body         |
| ------ | ---------------------------- | --------------- | --------------------------------------------------------------------------------------------------------------- | -------------------- |
| GET    | /products                    | None            | search, status, type, categoryId, minPrice, maxPrice, stockStatus, createdFrom, createdTo, sort, page, pageSize | None                 |
| GET    | /products/{productId}        | productId       | includeImages, includeVariants, includeStatistics                                                               | None                 |
| POST   | /products                    | None            | publishImmediately                                                                                              | CreateProductRequest |
| PUT    | /products/{productId}        | productId       | None                                                                                                            | UpdateProductRequest |
| DELETE | /products/{productId}        | productId       | force, deleteAssets                                                                                             | None                 |
| POST   | /products/{productId}/images | productId       | setAsPrimary, position                                                                                          | multipart/form-data  |

## 5. Orders

| Method | Endpoint                 | Path parameters | Query / filters & Pagination                                                                                                   | Request body             |
| ------ | ------------------------ | --------------- | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------ |
| GET    | /orders                  | None            | search, customerId, status, paymentStatus, fulfillmentStatus, createdFrom, createdTo, minTotal, maxTotal, sort, page, pageSize | None                     |
| GET    | /orders/{orderId}        | orderId         | includeItems, includeCustomer, includePayments, includeHistory                                                                 | None                     |
| PATCH  | /orders/{orderId}/status | orderId         | notifyCustomer                                                                                                                 | UpdateOrderStatusRequest |

## 6. Customers

| Method | Endpoint                       | Path parameters | Query / filters & Pagination                                                                                     | Request body          |
| ------ | ------------------------------ | --------------- | ---------------------------------------------------------------------------------------------------------------- | --------------------- |
| GET    | /customers                     | None            | search, status, category, memberType, createdFrom, createdTo, lastActiveFrom, lastActiveTo, sort, page, pageSize | None                  |
| GET    | /customers/{customerId}        | customerId      | includeUsers, includeStatistics, includeMetadata                                                                 | None                  |
| POST   | /customers                     | None            | sendInvitation                                                                                                   | CreateCustomerRequest |
| PUT    | /customers/{customerId}        | customerId      | None                                                                                                             | UpdateCustomerRequest |
| DELETE | /customers/{customerId}        | customerId      | force, anonymizeData                                                                                             | None                  |
| GET    | /customers/{customerId}/orders | customerId      | status, paymentStatus, createdFrom, createdTo, sort, page, pageSize                                              | None                  |

## 7. Discounts

| Method | Endpoint                | Path parameters | Query / filters & Pagination                                                                           | Request body          |
| ------ | ----------------------- | --------------- | ------------------------------------------------------------------------------------------------------ | --------------------- |
| GET    | /discounts              | None            | search, status, type, applicableTo, startsFrom, startsTo, expiresFrom, expiresTo, sort, page, pageSize | None                  |
| GET    | /discounts/{discountId} | discountId      | includeUsageStatistics, includeProducts                                                                | None                  |
| POST   | /discounts              | None            | activateImmediately                                                                                    | CreateDiscountRequest |
| PUT    | /discounts/{discountId} | discountId      | None                                                                                                   | UpdateDiscountRequest |
| DELETE | /discounts/{discountId} | discountId      | force                                                                                                  | None                  |

## 8. Reports

| Method | Endpoint         | Path parameters | Query / filters & Pagination                    | Request body |
| ------ | ---------------- | --------------- | ----------------------------------------------- | ------------ |
| GET    | /reports/revenue | None            | from, to, interval, currency, timezone, groupBy | None         |

## 9. Design

| Method | Endpoint      | Path parameters | Query / filters & Pagination    | Request body       |
| ------ | ------------- | --------------- | ------------------------------- | ------------------ |
| GET    | /design/theme | None            | includeCustomCss, includeAssets | None               |
| PUT    | /design/theme | None            | publishImmediately              | UpdateThemeRequest |

## 10. Settings

| Method | Endpoint          | Path parameters | Query / filters & Pagination    | Request body                 |
| ------ | ----------------- | --------------- | ------------------------------- | ---------------------------- |
| GET    | /settings/general | None            | includeLocalization, includeSeo | None                         |
| PUT    | /settings/general | None            | None                            | UpdateGeneralSettingsRequest |
