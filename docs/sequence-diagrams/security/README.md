# Security Unit Test Sequence Diagrams

## 1. Authentication Tests

### 1.1 Authenticate_WhenCredentialsAreValid_ShouldSucceed

```mermaid
sequenceDiagram
    autonumber
    participant Test as AuthenticationTests
    participant Auth as Authentication
    participant DB as UserDatabase

    Test->>Auth: Authenticate(username, password)
    activate Auth
    Auth->>DB: GetUser(username)
    activate DB
    DB-->>Auth: userRecord
    deactivate DB
    Auth->>Auth: HashPassword(password, userRecord.Salt)
    Auth-->>Auth: hashedPassword
    Auth->>Auth: Check hashedPassword == userRecord.PasswordHash
    Auth-->>Auth: true
    Auth-->>Test: AuthToken (success)
    deactivate Auth
```

### 1.2 Authenticate_WhenPasswordIsInvalid_ShouldFail

```mermaid
sequenceDiagram
    autonumber
    participant Test as AuthenticationTests
    participant Auth as Authentication
    participant DB as UserDatabase

    Test->>Auth: Authenticate(username, invalidPassword)
    activate Auth
    Auth->>DB: GetUser(username)
    activate DB
    DB-->>Auth: userRecord
    deactivate DB
    Auth->>Auth: HashPassword(invalidPassword, userRecord.Salt)
    Auth-->>Auth: wrongHashedPassword
    Auth->>Auth: Check wrongHashedPassword == userRecord.PasswordHash
    Auth-->>Auth: false
    Auth-->>Test: throws InvalidCredentialsException
    deactivate Auth
```

### 1.3 Authenticate_WhenUserDoesNotExist_ShouldFail

```mermaid
sequenceDiagram
    autonumber
    participant Test as AuthenticationTests
    participant Auth as Authentication
    participant DB as UserDatabase

    Test->>Auth: Authenticate(unknownUsername, password)
    activate Auth
    Auth->>DB: GetUser(unknownUsername)
    activate DB
    DB-->>Auth: null
    deactivate DB
    Auth-->>Test: throws UserNotFoundException
    deactivate Auth
```

## 2. Authorization Tests

### 2.1 Authorize_WhenPermissionExists_ShouldAllowOperation

```mermaid
sequenceDiagram
    autonumber
    participant Test as AuthorizationTests
    participant AuthZ as Authorization
    participant DB as PermissionDatabase

    Test->>AuthZ: Authorize(userToken, operation, resource)
    activate AuthZ
    AuthZ->>DB: GetRolesForUser(userToken.UserId)
    activate DB
    DB-->>AuthZ: roles
    deactivate DB
    AuthZ->>DB: GetPermissionsForRoles(roles, resource)
    activate DB
    DB-->>AuthZ: permissions
    deactivate DB
    AuthZ->>AuthZ: Check if operation is in permissions
    AuthZ-->>AuthZ: true
    AuthZ-->>Test: true (Allowed)
    deactivate AuthZ
```

### 2.2 Authorize_WhenPermissionDoesNotExist_ShouldDenyOperation

```mermaid
sequenceDiagram
    autonumber
    participant Test as AuthorizationTests
    participant AuthZ as Authorization
    participant DB as PermissionDatabase

    Test->>AuthZ: Authorize(userToken, operation, resource)
    activate AuthZ
    AuthZ->>DB: GetRolesForUser(userToken.UserId)
    activate DB
    DB-->>AuthZ: roles
    deactivate DB
    AuthZ->>DB: GetPermissionsForRoles(roles, resource)
    activate DB
    DB-->>AuthZ: empty list
    deactivate DB
    AuthZ->>AuthZ: Check if operation is in permissions
    AuthZ-->>AuthZ: false
    AuthZ-->>Test: throws UnauthorizedAccessException
    deactivate AuthZ
```

### 2.3 Authorize_ShouldCheckPermissionBeforeExecutingOperation

```mermaid
sequenceDiagram
    autonumber
    participant Test as AuthorizationTests
    participant AuthZ as Authorization
    participant Executor as QueryExecutor

    Test->>Executor: Execute(query)
    activate Executor
    Executor->>AuthZ: Authorize(user, query.Operation, query.Table)
    activate AuthZ
    AuthZ-->>Executor: throws UnauthorizedAccessException
    deactivate AuthZ
    Executor-->>Test: throws UnauthorizedAccessException
    deactivate Executor
```

### 2.4 AssignRole_WhenRoleIsValid_ShouldAddRole

```mermaid
sequenceDiagram
    autonumber
    participant Test as AuthorizationTests
    participant AuthZ as Authorization
    participant DB as PermissionDatabase

    Test->>AuthZ: AssignRole(userId, roleId)
    activate AuthZ
    AuthZ->>DB: RoleExists(roleId)
    activate DB
    DB-->>AuthZ: true
    deactivate DB
    AuthZ->>DB: AddUserRole(userId, roleId)
    activate DB
    DB-->>AuthZ: success
    deactivate DB
    AuthZ-->>Test: success
    deactivate AuthZ
```

### 2.5 RevokeRole_WhenRoleExists_ShouldRemoveRole

```mermaid
sequenceDiagram
    autonumber
    participant Test as AuthorizationTests
    participant AuthZ as Authorization
    participant DB as PermissionDatabase

    Test->>AuthZ: RevokeRole(userId, roleId)
    activate AuthZ
    AuthZ->>DB: UserHasRole(userId, roleId)
    activate DB
    DB-->>AuthZ: true
    deactivate DB
    AuthZ->>DB: RemoveUserRole(userId, roleId)
    activate DB
    DB-->>AuthZ: success
    deactivate DB
    AuthZ-->>Test: success
    deactivate AuthZ
```
