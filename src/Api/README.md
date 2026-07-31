# Complete DBMS API Endpoint Documentation

## 1. Database

| Method   | Endpoint                       | Application operation | Path parameters | Query / filters                                                 | Pagination     | Request body               | Success response         | Main errors             |
| -------- | ------------------------------ | --------------------- | --------------- | --------------------------------------------------------------- | -------------- | -------------------------- | ------------------------ | ----------------------- |
| `POST`   | `/databases`                   | `CreateDatabaseAsync` | None            | None                                                            | None           | CreateDatabaseRequest      | 201 + DatabaseResponse   | 400, 409, 422           |
| `GET`    | `/databases`                   | `GetDatabasesAsync`   | None            | name, state, storageEngine, owner, createdFrom, createdTo, sort | page, pageSize | None                       | 200 + paged list         | 400, 500                |
| `GET`    | `/databases/{dbName}`          | `GetDatabaseAsync`    | dbName          | includeSchemas, includeStatistics                               | None           | None                       | 200 + detail response    | 400, 404                |
| `DELETE` | `/databases/{dbName}`          | `DropDatabaseAsync`   | dbName          | force, deleteFiles, backupBeforeDrop                            | None           | None                       | 204                      | 400, 404, 409, 423      |
| `POST`   | `/databases/{dbName}/open`     | `OpenDatabaseAsync`   | dbName          | wait, timeoutSeconds                                            | None           | Optional body              | 200 + state response     | 400, 404, 409, 423, 503 |
| `POST`   | `/databases/{dbName}/close`    | `CloseDatabaseAsync`  | dbName          | force, wait, timeoutSeconds                                     | None           | Optional body              | 200 + state response     | 400, 404, 409, 423      |
| `POST`   | `/databases/{dbName}/readonly` | `SetReadOnlyAsync`    | dbName          | wait, timeoutSeconds                                            | None           | SetDatabaseReadOnlyRequest | 200 + state response     | 400, 404, 409, 423      |
| `POST`   | `/databases/{dbName}/recovery` | `StartRecoveryAsync`  | dbName          | wait, timeoutSeconds                                            | None           | StartRecoveryRequest       | 202 + operation response | 400, 404, 409, 423, 503 |

## 2. Schema

| Method   | Endpoint                                   | Application operation | Path parameters    | Query / filters                   | Pagination     | Request body        | Success response           | Main errors             |
| -------- | ------------------------------------------ | --------------------- | ------------------ | --------------------------------- | -------------- | ------------------- | -------------------------- | ----------------------- |
| `POST`   | `/databases/{dbName}/schemas`              | `CreateSchemaAsync`   | dbName             | None                              | None           | CreateSchemaRequest | 201 + SchemaResponse       | 400, 404, 409, 422      |
| `GET`    | `/databases/{dbName}/schemas`              | `GetSchemasAsync`     | dbName             | name, owner, sort                 | page, pageSize | None                | 200 + paged list           | 400, 404                |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}` | `GetSchemaAsync`      | dbName, schemaName | includeObjects, includeStatistics | None           | None                | 200 + SchemaDetailResponse | 400, 404                |
| `PUT`    | `/databases/{dbName}/schemas/{schemaName}` | `UpdateSchemaAsync`   | dbName, schemaName | None                              | None           | UpdateSchemaRequest | 200 + SchemaResponse       | 400, 404, 409, 412, 422 |
| `DELETE` | `/databases/{dbName}/schemas/{schemaName}` | `DropSchemaAsync`     | dbName, schemaName | cascade, force                    | None           | None                | 204                        | 400, 404, 409, 423      |

## 3. Table

| Method   | Endpoint                                                      | Application operation | Path parameters               | Query / filters                                                                          | Pagination     | Request body       | Success response          | Main errors             |
| -------- | ------------------------------------------------------------- | --------------------- | ----------------------------- | ---------------------------------------------------------------------------------------- | -------------- | ------------------ | ------------------------- | ----------------------- |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/tables`             | `CreateTableAsync`    | dbName, schemaName            | None                                                                                     | None           | CreateTableRequest | 201 + TableResponse       | 400, 404, 409, 422      |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/tables`             | `GetTablesAsync`      | dbName, schemaName            | name, hasRows, partitioned, sort                                                         | page, pageSize | None               | 200 + paged list          | 400, 404                |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/tables/{tableName}` | `GetTableAsync`       | dbName, schemaName, tableName | includeColumns, includeConstraints, includeIndexes, includePartitions, includeStatistics | None           | None               | 200 + TableDetailResponse | 400, 404                |
| `PUT`    | `/databases/{dbName}/schemas/{schemaName}/tables/{tableName}` | `UpdateTableAsync`    | dbName, schemaName, tableName | None                                                                                     | None           | UpdateTableRequest | 200 + TableResponse       | 400, 404, 409, 412, 422 |
| `DELETE` | `/databases/{dbName}/schemas/{schemaName}/tables/{tableName}` | `DropTableAsync`      | dbName, schemaName, tableName | cascade, force                                                                           | None           | None               | 204                       | 400, 404, 409, 423      |

## 4. Column

| Method   | Endpoint                                       | Application operation | Path parameters                           | Query / filters                | Pagination     | Request body       | Success response           | Main errors             |
| -------- | ---------------------------------------------- | --------------------- | ----------------------------------------- | ------------------------------ | -------------- | ------------------ | -------------------------- | ----------------------- |
| `POST`   | `/.../tables/{tableName}/columns`              | `AddColumnAsync`      | dbName, schemaName, tableName             | position                       | None           | AddColumnRequest   | 201 + ColumnResponse       | 400, 404, 409, 422      |
| `GET`    | `/.../tables/{tableName}/columns`              | `GetColumnsAsync`     | dbName, schemaName, tableName             | name, dataType, nullable, sort | page, pageSize | None               | 200 + paged list           | 400, 404                |
| `GET`    | `/.../tables/{tableName}/columns/{columnName}` | `GetColumnAsync`      | dbName, schemaName, tableName, columnName | includeDependencies            | None           | None               | 200 + ColumnDetailResponse | 400, 404                |
| `PUT`    | `/.../tables/{tableName}/columns/{columnName}` | `AlterColumnAsync`    | dbName, schemaName, tableName, columnName | validateExistingData           | None           | AlterColumnRequest | 200 + ColumnResponse       | 400, 404, 409, 412, 422 |
| `DELETE` | `/.../tables/{tableName}/columns/{columnName}` | `DropColumnAsync`     | dbName, schemaName, tableName, columnName | cascade, force                 | None           | None               | 204                        | 400, 404, 409, 423      |

## 5. Row

| Method   | Endpoint                               | Application operation | Path parameters                      | Query / filters                                    | Pagination     | Request body      | Success response         | Main errors             |
| -------- | -------------------------------------- | --------------------- | ------------------------------------ | -------------------------------------------------- | -------------- | ----------------- | ------------------------ | ----------------------- |
| `POST`   | `/.../tables/{tableName}/rows`         | `InsertRowAsync`      | dbName, schemaName, tableName        | returning                                          | None           | InsertRowRequest  | 201 + RowResponse        | 400, 404, 409, 422      |
| `POST`   | `/.../tables/{tableName}/rows/bulk`    | `BulkInsertRowsAsync` | dbName, schemaName, tableName        | returning                                          | None           | BulkInsertRequest | 201 + BulkInsertResponse | 400, 404, 409, 413, 422 |
| `GET`    | `/.../tables/{tableName}/rows`         | `QueryRowsAsync`      | dbName, schemaName, tableName        | filter, columns, sort, includeTotal, transactionId | page, pageSize | None              | 200 + paged rows         | 400, 404, 422           |
| `GET`    | `/.../tables/{tableName}/rows/{rowId}` | `GetRowAsync`         | dbName, schemaName, tableName, rowId | columns, transactionId                             | None           | None              | 200 + RowResponse        | 400, 404                |
| `PUT`    | `/.../tables/{tableName}/rows/{rowId}` | `ReplaceRowAsync`     | dbName, schemaName, tableName, rowId | returning                                          | None           | ReplaceRowRequest | 200 + RowResponse        | 400, 404, 409, 412, 422 |
| `PATCH`  | `/.../tables/{tableName}/rows/{rowId}` | `UpdateRowAsync`      | dbName, schemaName, tableName, rowId | returning                                          | None           | UpdateRowRequest  | 200 + RowResponse        | 400, 404, 409, 412, 422 |
| `DELETE` | `/.../tables/{tableName}/rows/{rowId}` | `DeleteRowAsync`      | dbName, schemaName, tableName, rowId | cascade, transactionId                             | None           | None              | 204                      | 400, 404, 409, 423      |

## 6. Constraint

| Method   | Endpoint                                               | Application operation      | Path parameters                               | Query / filters             | Pagination     | Request body                  | Success response               | Main errors             |
| -------- | ------------------------------------------------------ | -------------------------- | --------------------------------------------- | --------------------------- | -------------- | ----------------------------- | ------------------------------ | ----------------------- |
| `POST`   | `/.../tables/{tableName}/constraints/check`            | `AddCheckConstraintAsync`  | dbName, schemaName, tableName                 | validateExistingRows        | None           | CreateCheckConstraintRequest  | 201 + ConstraintResponse       | 400, 404, 409, 422      |
| `POST`   | `/.../tables/{tableName}/constraints/primary-key`      | `AddPrimaryKeyAsync`       | dbName, schemaName, tableName                 | validateExistingRows        | None           | CreatePrimaryKeyRequest       | 201 + ConstraintResponse       | 400, 404, 409, 422      |
| `POST`   | `/.../tables/{tableName}/constraints/unique`           | `AddUniqueConstraintAsync` | dbName, schemaName, tableName                 | validateExistingRows        | None           | CreateUniqueConstraintRequest | 201 + ConstraintResponse       | 400, 404, 409, 422      |
| `POST`   | `/.../tables/{tableName}/constraints/foreign-key`      | `AddForeignKeyAsync`       | dbName, schemaName, tableName                 | validateExistingRows        | None           | CreateForeignKeyRequest       | 201 + ConstraintResponse       | 400, 404, 409, 422      |
| `GET`    | `/.../tables/{tableName}/constraints`                  | `GetConstraintsAsync`      | dbName, schemaName, tableName                 | type, enabled, column, sort | page, pageSize | None                          | 200 + paged list               | 400, 404                |
| `GET`    | `/.../tables/{tableName}/constraints/{constraintName}` | `GetConstraintAsync`       | dbName, schemaName, tableName, constraintName | includeDependencies         | None           | None                          | 200 + ConstraintDetailResponse | 400, 404                |
| `PUT`    | `/.../tables/{tableName}/constraints/{constraintName}` | `UpdateConstraintAsync`    | dbName, schemaName, tableName, constraintName | validateExistingRows        | None           | UpdateConstraintRequest       | 200 + ConstraintResponse       | 400, 404, 409, 412, 422 |
| `DELETE` | `/.../tables/{tableName}/constraints/{constraintName}` | `DropConstraintAsync`      | dbName, schemaName, tableName, constraintName | cascade                     | None           | None                          | 204                            | 400, 404, 409           |

## 7. Index

| Method   | Endpoint                                                   | Application operation   | Path parameters                          | Query / filters                 | Pagination     | Request body            | Success response          | Main errors        |
| -------- | ---------------------------------------------------------- | ----------------------- | ---------------------------------------- | ------------------------------- | -------------- | ----------------------- | ------------------------- | ------------------ |
| `POST`   | `/.../tables/{tableName}/indexes`                          | `CreateIndexAsync`      | dbName, schemaName, tableName            | online                          | None           | CreateIndexRequest      | 201 + IndexResponse       | 400, 404, 409, 422 |
| `GET`    | `/.../tables/{tableName}/indexes`                          | `GetIndexesAsync`       | dbName, schemaName, tableName            | name, type, unique, state, sort | page, pageSize | None                    | 200 + paged list          | 400, 404           |
| `GET`    | `/.../tables/{tableName}/indexes/{indexName}`              | `GetIndexAsync`         | dbName, schemaName, tableName, indexName | includeStatistics               | None           | None                    | 200 + IndexDetailResponse | 400, 404           |
| `DELETE` | `/.../tables/{tableName}/indexes/{indexName}`              | `DropIndexAsync`        | dbName, schemaName, tableName, indexName | force                           | None           | None                    | 204                       | 400, 404, 409, 423 |
| `POST`   | `/.../tables/{tableName}/indexes/{indexName}/search`       | `SearchIndexAsync`      | dbName, schemaName, tableName, indexName | transactionId                   | None           | IndexSearchRequest      | 200 + row references      | 400, 404, 422      |
| `POST`   | `/.../tables/{tableName}/indexes/{indexName}/range-search` | `RangeSearchIndexAsync` | dbName, schemaName, tableName, indexName | transactionId                   | None           | IndexRangeSearchRequest | 200 + row references      | 400, 404, 422      |
| `POST`   | `/.../tables/{tableName}/indexes/{indexName}/rebuild`      | `RebuildIndexAsync`     | dbName, schemaName, tableName, indexName | online, wait                    | None           | Optional body           | 202 + operation response  | 400, 404, 409, 423 |

## 8. Partition

| Method   | Endpoint                                             | Application operation  | Path parameters                              | Query / filters           | Pagination     | Request body           | Success response              | Main errors             |
| -------- | ---------------------------------------------------- | ---------------------- | -------------------------------------------- | ------------------------- | -------------- | ---------------------- | ----------------------------- | ----------------------- |
| `POST`   | `/.../tables/{tableName}/partitions`                 | `CreatePartitionAsync` | dbName, schemaName, tableName                | None                      | None           | CreatePartitionRequest | 201 + PartitionResponse       | 400, 404, 409, 422      |
| `GET`    | `/.../tables/{tableName}/partitions`                 | `GetPartitionsAsync`   | dbName, schemaName, tableName                | name, column, state, sort | page, pageSize | None                   | 200 + paged list              | 400, 404                |
| `GET`    | `/.../tables/{tableName}/partitions/{partitionName}` | `GetPartitionAsync`    | dbName, schemaName, tableName, partitionName | includeStatistics         | None           | None                   | 200 + PartitionDetailResponse | 400, 404                |
| `PUT`    | `/.../tables/{tableName}/partitions/{partitionName}` | `UpdatePartitionAsync` | dbName, schemaName, tableName, partitionName | validateRows              | None           | UpdatePartitionRequest | 200 + PartitionResponse       | 400, 404, 409, 412, 422 |
| `DELETE` | `/.../tables/{tableName}/partitions/{partitionName}` | `DropPartitionAsync`   | dbName, schemaName, tableName, partitionName | force, moveTo             | None           | None                   | 204                           | 400, 404, 409, 423      |

## 9. View

| Method   | Endpoint                                                          | Application operation | Path parameters              | Query / filters                        | Pagination     | Request body      | Success response         | Main errors             |
| -------- | ----------------------------------------------------------------- | --------------------- | ---------------------------- | -------------------------------------- | -------------- | ----------------- | ------------------------ | ----------------------- |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/views`                  | `CreateViewAsync`     | dbName, schemaName           | validateOnly                           | None           | CreateViewRequest | 201 + ViewResponse       | 400, 404, 409, 422      |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/views`                  | `GetViewsAsync`       | dbName, schemaName           | name, enabled, sort                    | page, pageSize | None              | 200 + paged list         | 400, 404                |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/views/{viewName}`       | `GetViewAsync`        | dbName, schemaName, viewName | includeDefinition, includeDependencies | None           | None              | 200 + ViewDetailResponse | 400, 404                |
| `PUT`    | `/databases/{dbName}/schemas/{schemaName}/views/{viewName}`       | `AlterViewAsync`      | dbName, schemaName, viewName | validateOnly                           | None           | AlterViewRequest  | 200 + ViewResponse       | 400, 404, 409, 412, 422 |
| `DELETE` | `/databases/{dbName}/schemas/{schemaName}/views/{viewName}`       | `DropViewAsync`       | dbName, schemaName, viewName | cascade                                | None           | None              | 204                      | 400, 404, 409           |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/views/{viewName}/query` | `QueryViewAsync`      | dbName, schemaName, viewName | filter, columns, sort, transactionId   | page, pageSize | QueryViewRequest  | 200 + paged rows         | 400, 404, 422           |

## 10. Stored Procedure

| Method   | Endpoint                                                                      | Application operation   | Path parameters                   | Query / filters                  | Pagination     | Request body            | Success response              | Main errors             |
| -------- | ----------------------------------------------------------------------------- | ----------------------- | --------------------------------- | -------------------------------- | -------------- | ----------------------- | ----------------------------- | ----------------------- |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/procedures`                         | `CreateProcedureAsync`  | dbName, schemaName                | validateOnly                     | None           | CreateProcedureRequest  | 201 + ProcedureResponse       | 400, 404, 409, 422      |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/procedures`                         | `GetProceduresAsync`    | dbName, schemaName                | name, enabled, sort              | page, pageSize | None                    | 200 + paged list              | 400, 404                |
| `GET`    | `/databases/{dbName}/schemas/{schemaName}/procedures/{procedureName}`         | `GetProcedureAsync`     | dbName, schemaName, procedureName | includeBody, includeDependencies | None           | None                    | 200 + ProcedureDetailResponse | 400, 404                |
| `POST`   | `/databases/{dbName}/schemas/{schemaName}/procedures/{procedureName}/execute` | `ExecuteProcedureAsync` | dbName, schemaName, procedureName | timeoutSeconds                   | None           | ExecuteProcedureRequest | 200 + ProcedureResultResponse | 400, 404, 409, 422, 504 |
| `PUT`    | `/databases/{dbName}/schemas/{schemaName}/procedures/{procedureName}`         | `AlterProcedureAsync`   | dbName, schemaName, procedureName | validateOnly                     | None           | AlterProcedureRequest   | 200 + ProcedureResponse       | 400, 404, 409, 412, 422 |
| `DELETE` | `/databases/{dbName}/schemas/{schemaName}/procedures/{procedureName}`         | `DropProcedureAsync`    | dbName, schemaName, procedureName | cascade                          | None           | None                    | 204                           | 400, 404, 409           |

## 11. Query Processor

| Method | Endpoint          | Application operation | Path parameters | Query / filters     | Pagination | Request body        | Success response               | Main errors                  |
| ------ | ----------------- | --------------------- | --------------- | ------------------- | ---------- | ------------------- | ------------------------------ | ---------------------------- |
| `POST` | `/query/tokenize` | `TokenizeAsync`       | None            | None                | None       | QueryTextRequest    | 200 + TokenListResponse        | 400, 413, 422                |
| `POST` | `/query/parse`    | `ParseQueryAsync`     | None            | None                | None       | QueryTextRequest    | 200 + SyntaxTreeResponse       | 400, 413, 422                |
| `POST` | `/query/analyze`  | `AnalyzeQueryAsync`   | None            | includeDependencies | None       | QueryRequest        | 200 + SemanticAnalysisResponse | 400, 404, 422                |
| `POST` | `/query/optimize` | `OptimizeQueryAsync`  | None            | includeAlternatives | None       | QueryRequest        | 200 + QueryPlanResponse        | 400, 404, 422                |
| `POST` | `/query/execute`  | `ExecuteQueryAsync`   | None            | includePlan, stream | None       | ExecuteQueryRequest | 200 + QueryResultResponse      | 400, 404, 409, 422, 423, 504 |

## 12. Transaction

| Method | Endpoint                                 | Application operation      | Path parameters | Query / filters                                                      | Pagination     | Request body            | Success response                | Main errors        |
| ------ | ---------------------------------------- | -------------------------- | --------------- | -------------------------------------------------------------------- | -------------- | ----------------------- | ------------------------------- | ------------------ |
| `POST` | `/transactions`                          | `BeginTransactionAsync`    | None            | None                                                                 | None           | BeginTransactionRequest | 201 + TransactionResponse       | 400, 404, 409      |
| `GET`  | `/transactions`                          | `GetTransactionsAsync`     | None            | database, state, isolationLevel, owner, startedFrom, startedTo, sort | page, pageSize | None                    | 200 + paged list                | 400, 403           |
| `GET`  | `/transactions/{transactionId}`          | `GetTransactionAsync`      | transactionId   | includeLocks, includeVersions                                        | None           | None                    | 200 + TransactionDetailResponse | 400, 404           |
| `POST` | `/transactions/{transactionId}/commit`   | `CommitTransactionAsync`   | transactionId   | wait, timeoutSeconds                                                 | None           | Optional body           | 200 + TransactionResponse       | 400, 404, 409, 423 |
| `POST` | `/transactions/{transactionId}/rollback` | `RollbackTransactionAsync` | transactionId   | wait, timeoutSeconds                                                 | None           | Optional body           | 200 + TransactionResponse       | 400, 404, 409, 423 |

## 13. Lock Manager

| Method | Endpoint             | Application operation  | Path parameters | Query / filters                                          | Pagination     | Request body           | Success response      | Main errors             |
| ------ | -------------------- | ---------------------- | --------------- | -------------------------------------------------------- | -------------- | ---------------------- | --------------------- | ----------------------- |
| `POST` | `/locks/acquire`     | `AcquireLockAsync`     | None            | wait                                                     | None           | AcquireLockRequest     | 200 + LockResponse    | 400, 404, 409, 423, 504 |
| `POST` | `/locks/release`     | `ReleaseLockAsync`     | None            | None                                                     | None           | ReleaseLockRequest     | 204                   | 400, 404, 409           |
| `POST` | `/locks/release-all` | `ReleaseAllLocksAsync` | None            | None                                                     | None           | ReleaseAllLocksRequest | 204                   | 400, 404                |
| `GET`  | `/locks`             | `GetLocksAsync`        | None            | transactionId, database, resourceType, mode, state, sort | page, pageSize | None                   | 200 + paged list      | 400, 403                |
| `GET`  | `/locks/deadlocks`   | `GetDeadlocksAsync`    | None            | database, detectedFrom, detectedTo, resolved             | page, pageSize | None                   | 200 + paged deadlocks | 400, 403                |

## 14. MVCC

| Method | Endpoint          | Application operation | Path parameters | Query / filters                                                  | Pagination     | Request body  | Success response         | Main errors        |
| ------ | ----------------- | --------------------- | --------------- | ---------------------------------------------------------------- | -------------- | ------------- | ------------------------ | ------------------ |
| `GET`  | `/mvcc/snapshots` | `GetSnapshotsAsync`   | None            | database, transactionId, state, createdFrom, createdTo           | page, pageSize | None          | 200 + paged snapshots    | 400, 403           |
| `GET`  | `/mvcc/versions`  | `GetRowVersionsAsync` | None            | database, schema, table, rowId, transactionId, visibleOnly, sort | page, pageSize | None          | 200 + paged versions     | 400, 404, 403      |
| `POST` | `/mvcc/vacuum`    | `VacuumAsync`         | None            | database, schema, table, wait                                    | None           | VacuumRequest | 202 + operation response | 400, 404, 409, 423 |

## 15. Storage Engine

| Method | Endpoint                   | Application operation       | Path parameters | Query / filters                         | Pagination     | Request body               | Success response            | Main errors             |
| ------ | -------------------------- | --------------------------- | --------------- | --------------------------------------- | -------------- | -------------------------- | --------------------------- | ----------------------- |
| `GET`  | `/storage/pages`           | `GetPagesAsync`             | None            | database, fileName, dirty, pinned, sort | page, pageSize | None                       | 200 + paged pages           | 400, 403                |
| `GET`  | `/storage/pages/{pageId}`  | `GetPageAsync`              | pageId          | includeData                             | None           | None                       | 200 + PageResponse          | 400, 403, 404           |
| `PUT`  | `/storage/pages/{pageId}`  | `WritePageAsync`            | pageId          | flush                                   | None           | WritePageRequest           | 200 + PageResponse          | 400, 403, 404, 409, 422 |
| `POST` | `/storage/flush`           | `FlushPagesAsync`           | None            | database, wait                          | None           | FlushPagesRequest          | 202 + operation response    | 400, 403, 404, 409      |
| `POST` | `/storage/evict`           | `EvictPageAsync`            | None            | force                                   | None           | EvictPageRequest           | 204                         | 400, 403, 404, 409, 423 |
| `POST` | `/storage/engine/inmemory` | `SelectInMemoryEngineAsync` | None            | database                                | None           | SelectStorageEngineRequest | 200 + StorageStatusResponse | 400, 403, 404, 409      |
| `POST` | `/storage/engine/disk`     | `SelectDiskEngineAsync`     | None            | database                                | None           | SelectStorageEngineRequest | 200 + StorageStatusResponse | 400, 403, 404, 409      |
| `GET`  | `/storage/status`          | `GetStorageStatusAsync`     | None            | database, includeFiles, includePages    | None           | None                       | 200 + StorageStatusResponse | 400, 403, 404           |

## 16. Buffer Pool

| Method | Endpoint                   | Application operation   | Path parameters | Query / filters             | Pagination     | Request body            | Success response         | Main errors        |
| ------ | -------------------------- | ----------------------- | --------------- | --------------------------- | -------------- | ----------------------- | ------------------------ | ------------------ |
| `GET`  | `/buffer-pool/frames`      | `GetFramesAsync`        | None            | pageId, dirty, pinned, sort | page, pageSize | None                    | 200 + paged frames       | 400, 403           |
| `POST` | `/buffer-pool/fetch-page`  | `FetchPageAsync`        | None            | None                        | None           | FetchPageRequest        | 200 + FrameResponse      | 400, 403, 404, 409 |
| `POST` | `/buffer-pool/unpin`       | `UnpinPageAsync`        | None            | None                        | None           | UnpinPageRequest        | 204                      | 400, 403, 404, 409 |
| `POST` | `/buffer-pool/flush-dirty` | `FlushDirtyFramesAsync` | None            | wait                        | None           | FlushDirtyFramesRequest | 202 + operation response | 400, 403, 409      |

## 17. File Manager

| Method | Endpoint                           | Application operation | Path parameters  | Query / filters                    | Pagination     | Request body      | Success response        | Main errors             |
| ------ | ---------------------------------- | --------------------- | ---------------- | ---------------------------------- | -------------- | ----------------- | ----------------------- | ----------------------- |
| `POST` | `/files`                           | `CreateFileAsync`     | None             | None                               | None           | CreateFileRequest | 201 + FileResponse      | 400, 403, 409, 422      |
| `GET`  | `/files`                           | `GetFilesAsync`       | None             | name, open, minSize, maxSize, sort | page, pageSize | None              | 200 + paged files       | 400, 403                |
| `POST` | `/files/{fileName}/open`           | `OpenFileAsync`       | fileName         | readOnly                           | None           | Optional body     | 200 + FileStateResponse | 400, 403, 404, 409      |
| `POST` | `/files/{fileName}/close`          | `CloseFileAsync`      | fileName         | force, flush                       | None           | Optional body     | 200 + FileStateResponse | 400, 403, 404, 409, 423 |
| `GET`  | `/files/{fileName}/pages/{pageId}` | `ReadFilePageAsync`   | fileName, pageId | includeData                        | None           | None              | 200 + PageResponse      | 400, 403, 404           |
| `PUT`  | `/files/{fileName}/pages/{pageId}` | `WriteFilePageAsync`  | fileName, pageId | flush                              | None           | WritePageRequest  | 200 + PageResponse      | 400, 403, 404, 409, 422 |

## 18. Recovery

| Method | Endpoint            | Application operation  | Path parameters | Query / filters                                           | Pagination     | Request body           | Success response         | Main errors                  |
| ------ | ------------------- | ---------------------- | --------------- | --------------------------------------------------------- | -------------- | ---------------------- | ------------------------ | ---------------------------- |
| `POST` | `/recovery/backups` | `CreateBackupAsync`    | None            | wait                                                      | None           | CreateBackupRequest    | 202 + operation response | 400, 403, 404, 409, 507      |
| `GET`  | `/recovery/backups` | `GetBackupsAsync`      | None            | database, type, status, createdFrom, createdTo, sort      | page, pageSize | None                   | 200 + paged backups      | 400, 403                     |
| `POST` | `/recovery/restore` | `RestoreBackupAsync`   | None            | wait, force                                               | None           | RestoreBackupRequest   | 202 + operation response | 400, 403, 404, 409, 423, 507 |
| `POST` | `/recovery/recover` | `RecoverDatabaseAsync` | None            | wait                                                      | None           | RecoverDatabaseRequest | 202 + operation response | 400, 403, 404, 409, 423      |
| `GET`  | `/recovery/wal`     | `GetWalAsync`          | None            | database, transactionId, recordType, fromLsn, toLsn, sort | page, pageSize | None                   | 200 + paged WAL records  | 400, 403, 404                |

## 19. Security

| Method   | Endpoint                   | Application operation   | Path parameters | Query / filters                  | Pagination     | Request body            | Success response         | Main errors                  |
| -------- | -------------------------- | ----------------------- | --------------- | -------------------------------- | -------------- | ----------------------- | ------------------------ | ---------------------------- |
| `POST`   | `/auth/login`              | `LoginAsync`            | None            | None                             | None           | LoginRequest            | 200 + LoginResponse      | 400, 401, 423                |
| `POST`   | `/auth/refresh`            | `RefreshTokenAsync`     | None            | None                             | None           | RefreshTokenRequest     | 200 + LoginResponse      | 400, 401                     |
| `POST`   | `/auth/logout`             | `LogoutAsync`           | None            | allSessions                      | None           | LogoutRequest           | 204                      | 400, 401                     |
| `POST`   | `/security/users`          | `CreateUserAsync`       | None            | None                             | None           | CreateUserRequest       | 201 + UserResponse       | 400, 403, 409, 422           |
| `GET`    | `/security/users`          | `GetUsersAsync`         | None            | username, enabled, role, sort    | page, pageSize | None                    | 200 + paged users        | 400, 403                     |
| `GET`    | `/security/users/{userId}` | `GetUserAsync`          | userId          | includeRoles, includePermissions | None           | None                    | 200 + UserDetailResponse | 400, 403, 404                |
| `PUT`    | `/security/users/{userId}` | `UpdateUserAsync`       | userId          | None                             | None           | UpdateUserRequest       | 200 + UserResponse       | 400, 403, 404, 409, 412, 422 |
| `DELETE` | `/security/users/{userId}` | `DeleteUserAsync`       | userId          | revokeSessions                   | None           | None                    | 204                      | 400, 403, 404, 409           |
| `POST`   | `/security/roles`          | `CreateRoleAsync`       | None            | None                             | None           | CreateRoleRequest       | 201 + RoleResponse       | 400, 403, 409, 422           |
| `GET`    | `/security/roles`          | `GetRolesAsync`         | None            | name, system, sort               | page, pageSize | None                    | 200 + paged roles        | 400, 403                     |
| `POST`   | `/security/permissions`    | `AssignPermissionAsync` | None            | None                             | None           | AssignPermissionRequest | 204                      | 400, 403, 404, 409           |

## 20. Catalog

| Method | Endpoint                     | Application operation          | Path parameters | Query / filters                                           | Pagination     | Request body            | Success response         | Main errors        |
| ------ | ---------------------------- | ------------------------------ | --------------- | --------------------------------------------------------- | -------------- | ----------------------- | ------------------------ | ------------------ |
| `GET`  | `/catalog/objects`           | `SearchCatalogAsync`           | None            | database, schema, objectType, name, owner, sort           | page, pageSize | None                    | 200 + paged objects      | 400, 403           |
| `GET`  | `/catalog/statistics`        | `GetCatalogStatisticsAsync`    | None            | database, schema, objectType, objectName, staleOnly, sort | page, pageSize | None                    | 200 + paged statistics   | 400, 403, 404      |
| `POST` | `/catalog/statistics/update` | `UpdateCatalogStatisticsAsync` | None            | wait                                                      | None           | UpdateStatisticsRequest | 202 + operation response | 400, 403, 404, 409 |

## 21. Replication

| Method   | Endpoint                      | Application operation        | Path parameters | Query / filters              | Pagination     | Request body                | Success response                | Main errors             |
| -------- | ----------------------------- | ---------------------------- | --------------- | ---------------------------- | -------------- | --------------------------- | ------------------------------- | ----------------------- |
| `GET`    | `/replication/nodes`          | `GetReplicationNodesAsync`   | None            | role, state, healthy, sort   | page, pageSize | None                        | 200 + paged nodes               | 400, 403                |
| `POST`   | `/replication/nodes`          | `AddReplicationNodeAsync`    | None            | None                         | None           | AddReplicationNodeRequest   | 201 + ReplicationNodeResponse   | 400, 403, 409, 422      |
| `DELETE` | `/replication/nodes/{nodeId}` | `RemoveReplicationNodeAsync` | nodeId          | force                        | None           | None                        | 204                             | 400, 403, 404, 409      |
| `POST`   | `/replication/sync`           | `StartReplicationSyncAsync`  | None            | wait                         | None           | StartReplicationSyncRequest | 202 + operation response        | 400, 403, 404, 409, 423 |
| `GET`    | `/replication/status`         | `GetReplicationStatusAsync`  | None            | nodeId, database, includeLag | None           | None                        | 200 + ReplicationStatusResponse | 400, 403, 404           |

## 22. Health and Monitoring

| Method | Endpoint                   | Application operation        | Path parameters | Query / filters                         | Pagination     | Request body | Success response                | Main errors   |
| ------ | -------------------------- | ---------------------------- | --------------- | --------------------------------------- | -------------- | ------------ | ------------------------------- | ------------- |
| `GET`  | `/health`                  | `GetHealthAsync`             | None            | None                                    | None           | None         | 200 + HealthResponse            | 503           |
| `GET`  | `/health/ready`            | `GetReadinessAsync`          | None            | None                                    | None           | None         | 200 + ReadinessResponse         | 503           |
| `GET`  | `/metrics`                 | `GetMetricsAsync`            | None            | format                                  | None           | None         | 200 + metrics                   | 400, 500      |
| `GET`  | `/monitoring/performance`  | `GetPerformanceAsync`        | None            | database, from, to, interval            | page, pageSize | None         | 200 + paged metrics             | 400, 404, 500 |
| `GET`  | `/monitoring/buffer-pool`  | `GetBufferPoolMetricsAsync`  | None            | database, from, to                      | None           | None         | 200 + BufferPoolMetricsResponse | 400, 404, 500 |
| `GET`  | `/monitoring/transactions` | `GetTransactionMetricsAsync` | None            | database, state, from, to               | page, pageSize | None         | 200 + paged metrics             | 400, 404, 500 |
| `GET`  | `/diagnostics/errors`      | `GetErrorsAsync`             | None            | level, code, from, to, sort             | page, pageSize | None         | 200 + paged errors              | 400, 403, 500 |
| `GET`  | `/diagnostics/logs`        | `GetLogsAsync`               | None            | level, category, search, from, to, sort | page, pageSize | None         | 200 + paged logs                | 400, 403, 500 |
