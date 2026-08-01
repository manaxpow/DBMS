# Complete DBMS API Endpoint Documentation

## 1. Database

| Method   | Endpoint                       | Path parameters | Query / filters & Pagination | Request body               |
| -------- | ------------------------------ | --------------- | --------------------------------------------------------------- | -------------------------- |
| `POST`   | `/databases`                   | None            | None | CreateDatabaseRequest      |
| `GET`    | `/databases`                   | None            | name, state, storageEngine, owner, createdFrom, createdTo, sort, page, pageSize | None                       |
| `GET`    | `/databases/{dbName}`          | dbName          | includeSchemas, includeStatistics | None                       |
| `DELETE` | `/databases/{dbName}`          | dbName          | force, deleteFiles, backupBeforeDrop | None                       |
| `POST`   | `/databases/{dbName}/open`     | dbName          | wait, timeoutSeconds | Optional body              |
| `POST`   | `/databases/{dbName}/close`    | dbName          | force, wait, timeoutSeconds | Optional body              |
| `POST`   | `/databases/{dbName}/readonly` | dbName          | wait, timeoutSeconds | SetDatabaseReadOnlyRequest |
| `POST`   | `/databases/{dbName}/recovery` | dbName          | wait, timeoutSeconds | StartRecoveryRequest       |

## 2. Schema

| Method   | Endpoint                                   | Path parameters    | Query / filters & Pagination | Request body        |
| -------- | ------------------------------------------ | ------------------ | --------------------------------- | ------------------- |
| `POST`   | `/databases/{dbName}/schemas`              | dbName             | None | CreateSchemaRequest |
| `GET`    | `/databases/{dbName}/schemas`              | dbName             | name, owner, sort, page, pageSize | None                |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}` | dbName, schemaName | includeObjects, includeStatistics | None                |
| `PUT`    | `/databases/{dbName}/schemas/{schemaName}` | dbName, schemaName | None | UpdateSchemaRequest |
| `DELETE` | `/databases/{dbName}/schemas/{schemaName}` | dbName, schemaName | cascade, force | None                |

## 3. Table

| Method   | Endpoint                                                      | Path parameters               | Query / filters & Pagination | Request body       |
| -------- | ------------------------------------------------------------- | ----------------------------- | ---------------------------------------------------------------------------------------- | ------------------ |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/tables`             | dbName, schemaName            | None | CreateTableRequest |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/tables`             | dbName, schemaName            | name, hasRows, partitioned, sort, page, pageSize | None               |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/tables/{tableName}` | dbName, schemaName, tableName | includeColumns, includeConstraints, includeIndexes, includePartitions, includeStatistics | None               |
| `PUT`    | `/databases/{dbName}/schemas/{schemaName}/tables/{tableName}` | dbName, schemaName, tableName | None | UpdateTableRequest |
| `DELETE` | `/databases/{dbName}/schemas/{schemaName}/tables/{tableName}` | dbName, schemaName, tableName | cascade, force | None               |

## 4. Column

| Method   | Endpoint                                       | Path parameters                           | Query / filters & Pagination | Request body       |
| -------- | ---------------------------------------------- | ----------------------------------------- | ------------------------------ | ------------------ |
| `POST`   | `/.../tables/{tableName}/columns`              | dbName, schemaName, tableName             | position | AddColumnRequest   |
| `GET`    | `/.../tables/{tableName}/columns`              | dbName, schemaName, tableName             | name, dataType, nullable, sort, page, pageSize | None               |
| `GET`    | `/.../tables/{tableName}/columns/{columnName}` | dbName, schemaName, tableName, columnName | includeDependencies | None               |
| `PUT`    | `/.../tables/{tableName}/columns/{columnName}` | dbName, schemaName, tableName, columnName | validateExistingData | AlterColumnRequest |
| `DELETE` | `/.../tables/{tableName}/columns/{columnName}` | dbName, schemaName, tableName, columnName | cascade, force | None               |

## 5. Row

| Method   | Endpoint                               | Path parameters                      | Query / filters & Pagination | Request body      |
| -------- | -------------------------------------- | ------------------------------------ | -------------------------------------------------- | ----------------- |
| `POST`   | `/.../tables/{tableName}/rows`         | dbName, schemaName, tableName        | returning | InsertRowRequest  |
| `POST`   | `/.../tables/{tableName}/rows/bulk`    | dbName, schemaName, tableName        | returning | BulkInsertRequest |
| `GET`    | `/.../tables/{tableName}/rows`         | dbName, schemaName, tableName        | filter, columns, sort, includeTotal, transactionId, page, pageSize | None              |
| `GET`    | `/.../tables/{tableName}/rows/{rowId}` | dbName, schemaName, tableName, rowId | columns, transactionId | None              |
| `PUT`    | `/.../tables/{tableName}/rows/{rowId}` | dbName, schemaName, tableName, rowId | returning | ReplaceRowRequest |
| `PATCH`  | `/.../tables/{tableName}/rows/{rowId}` | dbName, schemaName, tableName, rowId | returning | UpdateRowRequest  |
| `DELETE` | `/.../tables/{tableName}/rows/{rowId}` | dbName, schemaName, tableName, rowId | cascade, transactionId | None              |

## 6. Constraint

| Method   | Endpoint                                               | Path parameters                               | Query / filters & Pagination | Request body                  |
| -------- | ------------------------------------------------------ | --------------------------------------------- | --------------------------- | ----------------------------- |
| `POST`   | `/.../tables/{tableName}/constraints/check`            | dbName, schemaName, tableName                 | validateExistingRows | CreateCheckConstraintRequest  |
| `POST`   | `/.../tables/{tableName}/constraints/primary-key`      | dbName, schemaName, tableName                 | validateExistingRows | CreatePrimaryKeyRequest       |
| `POST`   | `/.../tables/{tableName}/constraints/unique`           | dbName, schemaName, tableName                 | validateExistingRows | CreateUniqueConstraintRequest |
| `POST`   | `/.../tables/{tableName}/constraints/foreign-key`      | dbName, schemaName, tableName                 | validateExistingRows | CreateForeignKeyRequest       |
| `GET`    | `/.../tables/{tableName}/constraints`                  | dbName, schemaName, tableName                 | type, enabled, column, sort, page, pageSize | None                          |
| `GET`    | `/.../tables/{tableName}/constraints/{constraintName}` | dbName, schemaName, tableName, constraintName | includeDependencies | None                          |
| `PUT`    | `/.../tables/{tableName}/constraints/{constraintName}` | dbName, schemaName, tableName, constraintName | validateExistingRows | UpdateConstraintRequest       |
| `DELETE` | `/.../tables/{tableName}/constraints/{constraintName}` | dbName, schemaName, tableName, constraintName | cascade | None                          |

## 7. Index

| Method   | Endpoint                                                   | Path parameters                          | Query / filters & Pagination | Request body            |
| -------- | ---------------------------------------------------------- | ---------------------------------------- | ------------------------------- | ----------------------- |
| `POST`   | `/.../tables/{tableName}/indexes`                          | dbName, schemaName, tableName            | online | CreateIndexRequest      |
| `GET`    | `/.../tables/{tableName}/indexes`                          | dbName, schemaName, tableName            | name, type, unique, state, sort, page, pageSize | None                    |
| `GET`    | `/.../tables/{tableName}/indexes/{indexName}`              | dbName, schemaName, tableName, indexName | includeStatistics | None                    |
| `DELETE` | `/.../tables/{tableName}/indexes/{indexName}`              | dbName, schemaName, tableName, indexName | force | None                    |
| `POST`   | `/.../tables/{tableName}/indexes/{indexName}/search`       | dbName, schemaName, tableName, indexName | transactionId | IndexSearchRequest      |
| `POST`   | `/.../tables/{tableName}/indexes/{indexName}/range-search` | dbName, schemaName, tableName, indexName | transactionId | IndexRangeSearchRequest |
| `POST`   | `/.../tables/{tableName}/indexes/{indexName}/rebuild`      | dbName, schemaName, tableName, indexName | online, wait | Optional body           |

## 8. Partition

| Method   | Endpoint                                             | Path parameters                              | Query / filters & Pagination | Request body           |
| -------- | ---------------------------------------------------- | -------------------------------------------- | ------------------------- | ---------------------- |
| `POST`   | `/.../tables/{tableName}/partitions`                 | dbName, schemaName, tableName                | None | CreatePartitionRequest |
| `GET`    | `/.../tables/{tableName}/partitions`                 | dbName, schemaName, tableName                | name, column, state, sort, page, pageSize | None                   |
| `GET`    | `/.../tables/{tableName}/partitions/{partitionName}` | dbName, schemaName, tableName, partitionName | includeStatistics | None                   |
| `PUT`    | `/.../tables/{tableName}/partitions/{partitionName}` | dbName, schemaName, tableName, partitionName | validateRows | UpdatePartitionRequest |
| `DELETE` | `/.../tables/{tableName}/partitions/{partitionName}` | dbName, schemaName, tableName, partitionName | force, moveTo | None                   |

## 9. View

| Method   | Endpoint                                                          | Path parameters              | Query / filters & Pagination | Request body      |
| -------- | ----------------------------------------------------------------- | ---------------------------- | -------------------------------------- | ----------------- |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/views`                  | dbName, schemaName           | validateOnly | CreateViewRequest |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/views`                  | dbName, schemaName           | name, enabled, sort, page, pageSize | None              |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/views/{viewName}`       | dbName, schemaName, viewName | includeDefinition, includeDependencies | None              |
| `PUT`    | `/databases/{dbName}/schemas/{schemaName}/views/{viewName}`       | dbName, schemaName, viewName | validateOnly | AlterViewRequest  |
| `DELETE` | `/databases/{dbName}/schemas/{schemaName}/views/{viewName}`       | dbName, schemaName, viewName | cascade | None              |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/views/{viewName}/query` | dbName, schemaName, viewName | filter, columns, sort, transactionId, page, pageSize | QueryViewRequest  |

## 10. Stored Procedure

| Method   | Endpoint                                                                      | Path parameters                   | Query / filters & Pagination | Request body            |
| -------- | ----------------------------------------------------------------------------- | --------------------------------- | -------------------------------- | ----------------------- |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/procedures`                         | dbName, schemaName                | validateOnly | CreateProcedureRequest  |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/procedures`                         | dbName, schemaName                | name, enabled, sort, page, pageSize | None                    |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/procedures/{procedureName}`         | dbName, schemaName, procedureName | includeBody, includeDependencies | None                    |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/procedures/{procedureName}/execute` | dbName, schemaName, procedureName | timeoutSeconds | ExecuteProcedureRequest |
| `PUT`    | `/databases/{dbName}/schemas/{schemaName}/procedures/{procedureName}`         | dbName, schemaName, procedureName | validateOnly | AlterProcedureRequest   |
| `DELETE` | `/databases/{dbName}/schemas/{schemaName}/procedures/{procedureName}`         | dbName, schemaName, procedureName | cascade | None                    |

## 11. Query Processor

| Method | Endpoint          | Path parameters | Query / filters & Pagination | Request body        |
| ------ | ----------------- | --------------- | ------------------- | ------------------- |
| `POST` | `/query/tokenize` | None            | None | QueryTextRequest    |
| `POST` | `/query/parse`    | None            | None | QueryTextRequest    |
| `POST` | `/query/analyze`  | None            | includeDependencies | QueryRequest        |
| `POST` | `/query/optimize` | None            | includeAlternatives | QueryRequest        |
| `POST` | `/query/execute`  | None            | includePlan, stream | ExecuteQueryRequest |

## 12. Transaction

| Method | Endpoint                                 | Path parameters | Query / filters & Pagination | Request body            |
| ------ | ---------------------------------------- | --------------- | -------------------------------------------------------------------- | ----------------------- |
| `POST` | `/transactions`                          | None            | None | BeginTransactionRequest |
| `GET`  | `/transactions`                          | None            | database, state, isolationLevel, owner, startedFrom, startedTo, sort, page, pageSize | None                    |
| `GET`  | `/transactions/{transactionId}`          | transactionId   | includeLocks, includeVersions | None                    |
| `POST` | `/transactions/{transactionId}/commit`   | transactionId   | wait, timeoutSeconds | Optional body           |
| `POST` | `/transactions/{transactionId}/rollback` | transactionId   | wait, timeoutSeconds | Optional body           |

## 13. Lock Manager

| Method | Endpoint             | Path parameters | Query / filters & Pagination | Request body           |
| ------ | -------------------- | --------------- | -------------------------------------------------------- | ---------------------- |
| `POST` | `/locks/acquire`     | None            | wait | AcquireLockRequest     |
| `POST` | `/locks/release`     | None            | None | ReleaseLockRequest     |
| `POST` | `/locks/release-all` | None            | None | ReleaseAllLocksRequest |
| `GET`  | `/locks`             | None            | transactionId, database, resourceType, mode, state, sort, page, pageSize | None                   |
| `GET`  | `/locks/deadlocks`   | None            | database, detectedFrom, detectedTo, resolved, page, pageSize | None                   |

## 14. MVCC

| Method | Endpoint          | Path parameters | Query / filters & Pagination | Request body  |
| ------ | ----------------- | --------------- | ---------------------------------------------------------------- | ------------- |
| `GET`  | `/mvcc/snapshots` | None            | database, transactionId, state, createdFrom, createdTo, page, pageSize | None          |
| `GET`  | `/mvcc/versions`  | None            | database, schema, table, rowId, transactionId, visibleOnly, sort, page, pageSize | None          |
| `POST` | `/mvcc/vacuum`    | None            | database, schema, table, wait | VacuumRequest |

## 15. Storage Engine

| Method | Endpoint                   | Path parameters | Query / filters & Pagination | Request body               |
| ------ | -------------------------- | --------------- | --------------------------------------- | -------------------------- |
| `GET`  | `/storage/pages`           | None            | database, fileName, dirty, pinned, sort, page, pageSize | None                       |
| `GET`  | `/storage/pages/{pageId}`  | pageId          | includeData | None                       |
| `PUT`  | `/storage/pages/{pageId}`  | pageId          | flush | WritePageRequest           |
| `POST` | `/storage/flush`           | None            | database, wait | FlushPagesRequest          |
| `POST` | `/storage/evict`           | None            | force | EvictPageRequest           |
| `POST` | `/storage/engine/inmemory` | None            | database | SelectStorageEngineRequest |
| `POST` | `/storage/engine/disk`     | None            | database | SelectStorageEngineRequest |
| `GET`  | `/storage/status`          | None            | database, includeFiles, includePages | None                       |

## 16. Buffer Pool

| Method | Endpoint                   | Path parameters | Query / filters & Pagination | Request body            |
| ------ | -------------------------- | --------------- | --------------------------- | ----------------------- |
| `GET`  | `/buffer-pool/frames`      | None            | pageId, dirty, pinned, sort, page, pageSize | None                    |
| `POST` | `/buffer-pool/fetch-page`  | None            | None | FetchPageRequest        |
| `POST` | `/buffer-pool/unpin`       | None            | None | UnpinPageRequest        |
| `POST` | `/buffer-pool/flush-dirty` | None            | wait | FlushDirtyFramesRequest |

## 17. File Manager

| Method | Endpoint                           | Path parameters  | Query / filters & Pagination | Request body      |
| ------ | ---------------------------------- | ---------------- | ---------------------------------- | ----------------- |
| `POST` | `/files`                           | None             | None | CreateFileRequest |
| `GET`  | `/files`                           | None             | name, open, minSize, maxSize, sort, page, pageSize | None              |
| `POST` | `/files/{fileName}/open`           | fileName         | readOnly | Optional body     |
| `POST` | `/files/{fileName}/close`          | fileName         | force, flush | Optional body     |
| `GET`  | `/files/{fileName}/pages/{pageId}` | fileName, pageId | includeData | None              |
| `PUT`  | `/files/{fileName}/pages/{pageId}` | fileName, pageId | flush | WritePageRequest  |

## 18. Recovery

| Method | Endpoint            | Path parameters | Query / filters & Pagination | Request body           |
| ------ | ------------------- | --------------- | --------------------------------------------------------- | ---------------------- |
| `POST` | `/recovery/backups` | None            | wait | CreateBackupRequest    |
| `GET`  | `/recovery/backups` | None            | database, type, status, createdFrom, createdTo, sort, page, pageSize | None                   |
| `POST` | `/recovery/restore` | None            | wait, force | RestoreBackupRequest   |
| `POST` | `/recovery/recover` | None            | wait | RecoverDatabaseRequest |
| `GET`  | `/recovery/wal`     | None            | database, transactionId, recordType, fromLsn, toLsn, sort, page, pageSize | None                   |

## 19. Security

| Method   | Endpoint                   | Path parameters | Query / filters & Pagination | Request body            |
| -------- | -------------------------- | --------------- | -------------------------------- | ----------------------- |
| `POST`   | `/auth/login`              | None            | None | LoginRequest            |
| `POST`   | `/auth/refresh`            | None            | None | RefreshTokenRequest     |
| `POST`   | `/auth/logout`             | None            | allSessions | LogoutRequest           |
| `POST`   | `/security/users`          | None            | None | CreateUserRequest       |
| `GET`    | `/security/users`          | None            | username, enabled, role, sort, page, pageSize | None                    |
| `GET`    | `/security/users/{userId}` | userId          | includeRoles, includePermissions | None                    |
| `PUT`    | `/security/users/{userId}` | userId          | None | UpdateUserRequest       |
| `DELETE` | `/security/users/{userId}` | userId          | revokeSessions | None                    |
| `POST`   | `/security/roles`          | None            | None | CreateRoleRequest       |
| `GET`    | `/security/roles`          | None            | name, system, sort, page, pageSize | None                    |
| `POST`   | `/security/permissions`    | None            | None | AssignPermissionRequest |

## 20. Catalog

| Method | Endpoint                     | Path parameters | Query / filters & Pagination | Request body            |
| ------ | ---------------------------- | --------------- | --------------------------------------------------------- | ----------------------- |
| `GET`  | `/catalog/objects`           | None            | database, schema, objectType, name, owner, sort, page, pageSize | None                    |
| `GET`  | `/catalog/statistics`        | None            | database, schema, objectType, objectName, staleOnly, sort, page, pageSize | None                    |
| `POST` | `/catalog/statistics/update` | None            | wait | UpdateStatisticsRequest |

## 21. Replication

| Method   | Endpoint                      | Path parameters | Query / filters & Pagination | Request body                |
| -------- | ----------------------------- | --------------- | ---------------------------- | --------------------------- |
| `GET`    | `/replication/nodes`          | None            | role, state, healthy, sort, page, pageSize | None                        |
| `POST`   | `/replication/nodes`          | None            | None | AddReplicationNodeRequest   |
| `DELETE` | `/replication/nodes/{nodeId}` | nodeId          | force | None                        |
| `POST`   | `/replication/sync`           | None            | wait | StartReplicationSyncRequest |
| `GET`    | `/replication/status`         | None            | nodeId, database, includeLag | None                        |

## 22. Health and Monitoring

| Method | Endpoint                   | Path parameters | Query / filters & Pagination | Request body |
| ------ | -------------------------- | --------------- | --------------------------------------- | ------------ |
| `GET`  | `/health`                  | None            | None | None         |
| `GET`  | `/health/ready`            | None            | None | None         |
| `GET`  | `/metrics`                 | None            | format | None         |
| `GET`  | `/monitoring/performance`  | None            | database, from, to, interval, page, pageSize | None         |
| `GET`  | `/monitoring/buffer-pool`  | None            | database, from, to | None         |
| `GET`  | `/monitoring/transactions` | None            | database, state, from, to, page, pageSize | None         |
| `GET`  | `/diagnostics/errors`      | None            | level, code, from, to, sort, page, pageSize | None         |
| `GET`  | `/diagnostics/logs`        | None            | level, category, search, from, to, sort, page, pageSize | None         |
