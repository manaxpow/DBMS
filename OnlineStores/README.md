## 1. Authentication

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| POST | /auth/login | None | None | LoginRequest |
| POST | /auth/logout | None | allDevices | Optional LogoutRequest |
| POST | /auth/refresh-token | None | None | RefreshTokenRequest |
| POST | /auth/forgot-password | None | None | ForgotPasswordRequest |
| POST | /auth/reset-password | None | None | ResetPasswordRequest |
| GET | /auth/session | None | includePermissions | None |

## 2. Navigation and Current User

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /navigation | None | platform, locale, includeHidden | None |
| GET | /users/me | None | includeStore, includeRole | None |
| GET | /users/me/permissions | None | resource, module | None |
| GET | /users/me/shortcuts | None | platform | None |
| PUT | /users/me/shortcuts | None | None | UpdateShortcutsRequest |

## 3. Overview

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /overview/summary | None | from, to, timezone, currency | None |
| GET | /overview/revenue-chart | None | from, to, interval, currency, timezone | None |
| GET | /overview/orders-chart | None | from, to, interval, status, timezone | None |
| GET | /overview/recent-activities | None | type, actorId, from, to, sort, page, pageSize | None |

## 4. Store

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /store | None | includeOwner, includeSettings | None |
| PUT | /store | None | None | UpdateStoreRequest |
| GET | /store/status | None | includeHealth | None |
| PATCH | /store/status | None | wait | UpdateStoreStatusRequest |
| GET | /store/statistics | None | from, to, timezone, currency | None |
| POST | /store/logo | None | replaceExisting | multipart/form-data |

## 5. Products

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /products | None | search, status, type, categoryId, minPrice, maxPrice, stockStatus, createdFrom, createdTo, sort, page, pageSize | None |
| GET | /products/{productId} | productId | includeImages, includeVariants, includeStatistics | None |
| POST | /products | None | publishImmediately | CreateProductRequest |
| PUT | /products/{productId} | productId | None | UpdateProductRequest |
| DELETE | /products/{productId} | productId | force, deleteAssets | None |
| PATCH | /products/{productId}/status | productId | None | UpdateProductStatusRequest |
| POST | /products/{productId}/images | productId | setAsPrimary, position | multipart/form-data |
| DELETE | /products/{productId}/images/{imageId} | productId, imageId | None | None |
| GET | /products/export | None | search, status, type, categoryId, stockStatus, format, fields | None |

## 6. Orders

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /orders | None | search, customerId, status, paymentStatus, fulfillmentStatus, createdFrom, createdTo, minTotal, maxTotal, sort, page, pageSize | None |
| GET | /orders/{orderId} | orderId | includeItems, includeCustomer, includePayments, includeHistory | None |
| POST | /orders | None | sendConfirmation | CreateOrderRequest |
| PUT | /orders/{orderId} | orderId | recalculateTotals | UpdateOrderRequest |
| PATCH | /orders/{orderId}/status | orderId | notifyCustomer | UpdateOrderStatusRequest |
| POST | /orders/{orderId}/cancel | orderId | notifyCustomer, restockItems | CancelOrderRequest |
| POST | /orders/{orderId}/refund | orderId | notifyCustomer | RefundOrderRequest |
| GET | /orders/{orderId}/invoice | orderId | format, locale | None |
| GET | /orders/export | None | search, status, paymentStatus, createdFrom, createdTo, format, fields | None |

## 7. Subscriptions

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /subscriptions | None | search, customerId, productId, status, billingInterval, startedFrom, startedTo, renewsFrom, renewsTo, sort, page, pageSize | None |
| GET | /subscriptions/{subscriptionId} | subscriptionId | includeCustomer, includePayments, includeHistory | None |
| POST | /subscriptions | None | chargeImmediately, sendConfirmation | CreateSubscriptionRequest |
| PUT | /subscriptions/{subscriptionId} | subscriptionId | prorate | UpdateSubscriptionRequest |
| POST | /subscriptions/{subscriptionId}/pause | subscriptionId | effectiveAt | PauseSubscriptionRequest |
| POST | /subscriptions/{subscriptionId}/resume | subscriptionId | chargeImmediately | Optional ResumeSubscriptionRequest |
| POST | /subscriptions/{subscriptionId}/cancel | subscriptionId | cancelAtPeriodEnd, refund | CancelSubscriptionRequest |
| GET | /subscriptions/export | None | search, status, billingInterval, startedFrom, startedTo, format, fields | None |

## 8. Customers

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /customers/statistics | None | from, to, timezone, segment | None |
| GET | /customers/growth | None | from, to, interval, comparePrevious, timezone | None |
| GET | /customers | None | search, status, category, memberType, createdFrom, createdTo, lastActiveFrom, lastActiveTo, sort, page, pageSize | None |
| GET | /customers/{customerId} | customerId | includeUsers, includeStatistics, includeMetadata | None |
| POST | /customers | None | sendInvitation | CreateCustomerRequest |
| PUT | /customers/{customerId} | customerId | None | UpdateCustomerRequest |
| PATCH | /customers/{customerId} | customerId | None | PatchCustomerRequest |
| DELETE | /customers/{customerId} | customerId | force, anonymizeData | None |
| PATCH | /customers/{customerId}/status | customerId | None | UpdateCustomerStatusRequest |
| GET | /customers/{customerId}/orders | customerId | status, paymentStatus, createdFrom, createdTo, sort, page, pageSize | None |
| GET | /customers/{customerId}/subscriptions | customerId | status, billingInterval, sort, page, pageSize | None |
| GET | /customers/{customerId}/activities | customerId | type, actorId, from, to, sort, page, pageSize | None |
| POST | /customers/bulk-status | None | None | BulkUpdateCustomerStatusRequest |
| DELETE | /customers/bulk | None | force, anonymizeData | BulkDeleteCustomersRequest |
| GET | /customers/export | None | search, status, category, memberType, createdFrom, createdTo, format, fields | None |

## 9. Discounts

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /discounts | None | search, status, type, applicableTo, startsFrom, startsTo, expiresFrom, expiresTo, sort, page, pageSize | None |
| GET | /discounts/{discountId} | discountId | includeUsageStatistics, includeProducts | None |
| POST | /discounts | None | activateImmediately | CreateDiscountRequest |
| PUT | /discounts/{discountId} | discountId | None | UpdateDiscountRequest |
| DELETE | /discounts/{discountId} | discountId | force | None |
| PATCH | /discounts/{discountId}/status | discountId | None | UpdateDiscountStatusRequest |
| POST | /discounts/validate | None | None | ValidateDiscountRequest |
| GET | /discounts/export | None | search, status, type, startsFrom, expiresTo, format, fields | None |

## 10. Licenses

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /licenses | None | search, status, customerId, productId, activatedFrom, activatedTo, expiresFrom, expiresTo, sort, page, pageSize | None |
| GET | /licenses/{licenseId} | licenseId | includeCustomer, includeProduct, includeActivations | None |
| POST | /licenses | None | activateImmediately, sendToCustomer | CreateLicenseRequest |
| PUT | /licenses/{licenseId} | licenseId | None | UpdateLicenseRequest |
| POST | /licenses/{licenseId}/activate | licenseId | None | ActivateLicenseRequest |
| POST | /licenses/{licenseId}/revoke | licenseId | deactivateDevices | RevokeLicenseRequest |
| GET | /licenses/export | None | search, status, customerId, productId, expiresFrom, expiresTo, format, fields | None |

## 11. Emails

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /emails/campaigns | None | search, status, type, createdFrom, createdTo, scheduledFrom, scheduledTo, sort, page, pageSize | None |
| GET | /emails/campaigns/{campaignId} | campaignId | includeRecipients, includeStatistics, includeContent | None |
| POST | /emails/campaigns | None | saveAsDraft | CreateEmailCampaignRequest |
| PUT | /emails/campaigns/{campaignId} | campaignId | None | UpdateEmailCampaignRequest |
| DELETE | /emails/campaigns/{campaignId} | campaignId | force | None |
| POST | /emails/campaigns/{campaignId}/send | campaignId | sendTestOnly | Optional SendEmailCampaignRequest |
| POST | /emails/campaigns/{campaignId}/schedule | campaignId | None | ScheduleEmailCampaignRequest |
| GET | /emails/campaigns/{campaignId}/statistics | campaignId | from, to, interval, timezone | None |

## 12. Reports

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /reports/revenue | None | from, to, interval, currency, timezone, groupBy | None |
| GET | /reports/orders | None | from, to, interval, status, paymentStatus, timezone, groupBy | None |
| GET | /reports/customers | None | from, to, interval, status, category, timezone, groupBy | None |
| GET | /reports/products | None | from, to, categoryId, productId, sortBy, limit, timezone | None |
| GET | /reports/subscriptions | None | from, to, interval, status, billingInterval, timezone, groupBy | None |
| POST | /reports/generate | None | asynchronous | GenerateReportRequest |
| GET | /reports/{reportId}/download | reportId | format | None |

## 13. Design

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /design/theme | None | includeCustomCss, includeAssets | None |
| PUT | /design/theme | None | publishImmediately | UpdateThemeRequest |
| POST | /design/assets | None | type, replaceExisting | multipart/form-data |
| DELETE | /design/assets/{assetId} | assetId | force | None |
| GET | /design/templates | None | search, category, status, sort, page, pageSize | None |
| POST | /design/templates/{templateId}/apply | templateId | preserveCustomAssets, publishImmediately | Optional ApplyTemplateRequest |

## 14. Settings

| Method | Endpoint | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------- | --------------- | ---------------------------- | ------------ |
| GET | /settings/general | None | includeLocalization, includeSeo | None |
| PUT | /settings/general | None | None | UpdateGeneralSettingsRequest |
| GET | /settings/payment | None | includeProviderStatus | None |
| PUT | /settings/payment | None | validateCredentials | UpdatePaymentSettingsRequest |
| GET | /settings/notifications | None | channel, eventType | None |
| PUT | /settings/notifications | None | None | UpdateNotificationSettingsRequest |
| GET | /settings/team-members | None | search, role, status, createdFrom, createdTo, sort, page, pageSize | None |
| POST | /settings/team-members | None | sendInvitation | InviteTeamMemberRequest |
