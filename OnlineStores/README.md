# OnlineStores API Documentation

Click here to see swagger documentation
   - **https://manaxpow.github.io/DBMS/api/**

## 1. Authentication

| Method | Endpoint            | Path parameters | Query / filters & Pagination | Request body           | Authorize |
| ------ | ------------------- | --------------- | ---------------------------- | ---------------------- | --------- |
| POST   | /auth/login         | None            | None                         | LoginRequest           | None      |
| POST   | /auth/register      | None            | None                         | RegisterRequest        | None      |
| POST   | /auth/logout        | None            | allDevices                   | Optional LogoutRequest | User      |
| POST   | /auth/refresh-token | None            | None                         | RefreshTokenRequest    | None      |
| GET    | /users/me           | None            | includeStore, includeRole    | None                   | User      |

## 2. Dashboard (Overview)

| Method | Endpoint          | Path parameters | Query / filters & Pagination | Request body | Authorize |
| ------ | ----------------- | --------------- | ---------------------------- | ------------ | --------- |
| GET    | /overview/summary | None            | from, to, timezone, currency | None         | Admin     |

## 3. Store

| Method | Endpoint    | Path parameters | Query / filters & Pagination  | Request body        | Authorize |
| ------ | ----------- | --------------- | ----------------------------- | ------------------- | --------- |
| GET    | /store      | None            | includeOwner, includeSettings | None                | Admin     |
| PUT    | /store      | None            | None                          | UpdateStoreRequest  | Admin     |
| POST   | /store/logo | None            | replaceExisting               | multipart/form-data | Admin     |

## 4. Products

| Method | Endpoint                     | Path parameters | Query / filters & Pagination                                                                                    | Request body         | Authorize |
| ------ | ---------------------------- | --------------- | --------------------------------------------------------------------------------------------------------------- | -------------------- | --------- |
| GET    | /products                    | None            | search, status, type, categoryId, minPrice, maxPrice, stockStatus, createdFrom, createdTo, sort, page, pageSize | None                 | None      |
| GET    | /products/{productId}        | productId       | includeImages, includeVariants, includeStatistics                                                               | None                 | None      |
| POST   | /products                    | None            | publishImmediately                                                                                              | CreateProductRequest | Admin     |
| PUT    | /products/{productId}        | productId       | None                                                                                                            | UpdateProductRequest | Admin     |
| DELETE | /products/{productId}        | productId       | force, deleteAssets                                                                                             | None                 | Admin     |
| POST   | /products/{productId}/images | productId       | setAsPrimary, position                                                                                          | multipart/form-data  | Admin     |

## 5. Orders

| Method | Endpoint                 | Path parameters | Query / filters & Pagination                                                                                                   | Request body             | Authorize |
| ------ | ------------------------ | --------------- | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------ | --------- |
| GET    | /orders                  | None            | search, customerId, status, paymentStatus, fulfillmentStatus, createdFrom, createdTo, minTotal, maxTotal, sort, page, pageSize | None                     | Admin, User |
| GET    | /orders/{orderId}        | orderId         | includeItems, includeCustomer, includePayments, includeHistory                                                                 | None                     | Admin, User |
| POST   | /orders                  | None            | None                                                                                                                           | CreateOrderRequest       | Admin, User |
| PUT    | /orders/{orderId}        | orderId         | None                                                                                                                           | UpdateOrderRequest       | Admin     |
| PATCH  | /orders/{orderId}/status | orderId         | notifyCustomer                                                                                                                 | UpdateOrderStatusRequest | Admin     |
| DELETE | /orders/{orderId}        | orderId         | None                                                                                                                           | None                     | Admin     |

## 6. Customers

| Method | Endpoint                       | Path parameters | Query / filters & Pagination                                                                                     | Request body          | Authorize |
| ------ | ------------------------------ | --------------- | ---------------------------------------------------------------------------------------------------------------- | --------------------- | --------- |
| GET    | /customers                     | None            | search, status, category, memberType, createdFrom, createdTo, lastActiveFrom, lastActiveTo, sort, page, pageSize | None                  | Admin     |
| GET    | /customers/{customerId}        | customerId      | includeUsers, includeStatistics, includeMetadata                                                                 | None                  | Admin     |
| POST   | /customers                     | None            | sendInvitation                                                                                                   | CreateCustomerRequest | Admin     |
| PUT    | /customers/{customerId}        | customerId      | None                                                                                                             | UpdateCustomerRequest | Admin     |
| DELETE | /customers/{customerId}        | customerId      | force, anonymizeData                                                                                             | None                  | Admin     |
| GET    | /customers/{customerId}/orders | customerId      | status, paymentStatus, createdFrom, createdTo, sort, page, pageSize                                              | None                  | Admin     |

## 7. Discounts

| Method | Endpoint                | Path parameters | Query / filters & Pagination                                                                           | Request body          | Authorize |
| ------ | ----------------------- | --------------- | ------------------------------------------------------------------------------------------------------ | --------------------- | --------- |
| GET    | /discounts              | None            | search, status, type, applicableTo, startsFrom, startsTo, expiresFrom, expiresTo, sort, page, pageSize | None                  | Admin     |
| GET    | /discounts/{discountId} | discountId      | includeUsageStatistics, includeProducts                                                                | None                  | Admin     |
| POST   | /discounts              | None            | activateImmediately                                                                                    | CreateDiscountRequest | Admin     |
| PUT    | /discounts/{discountId} | discountId      | None                                                                                                   | UpdateDiscountRequest | Admin     |
| DELETE | /discounts/{discountId} | discountId      | force                                                                                                  | None                  | Admin     |

## 8. Reports

| Method | Endpoint         | Path parameters | Query / filters & Pagination                    | Request body | Authorize |
| ------ | ---------------- | --------------- | ----------------------------------------------- | ------------ | --------- |
| GET    | /reports/revenue | None            | from, to, interval, currency, timezone, groupBy | None         | Admin     |

## 9. Design

| Method | Endpoint      | Path parameters | Query / filters & Pagination    | Request body       | Authorize |
| ------ | ------------- | --------------- | ------------------------------- | ------------------ | --------- |
| GET    | /design/theme | None            | includeCustomCss, includeAssets | None               | None      |
| PUT    | /design/theme | None            | publishImmediately              | UpdateThemeRequest | Admin     |

## 10. Settings

| Method | Endpoint          | Path parameters | Query / filters & Pagination    | Request body                 | Authorize |
| ------ | ----------------- | --------------- | ------------------------------- | ---------------------------- | --------- |
| GET    | /settings/general | None            | includeLocalization, includeSeo | None                         | None      |
| PUT    | /settings/general | None            | None                            | UpdateGeneralSettingsRequest | Admin     |
