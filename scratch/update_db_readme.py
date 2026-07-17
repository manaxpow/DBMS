import sys

def update_file(filename):
    with open(filename, 'r', encoding='utf-8') as f:
        content = f.read()

    # Update Schema
    content = content.replace(
        "+RemoveTable(string tableName) void",
        "+RemoveTable(string tableName) void\n        +DropTable(string tableName) void\n        +AlterTable(string tableName, Table newTable) void"
    )

    # Update Table
    content = content.replace(
        "+AddColumn(Column column) void",
        "+AddColumn(Column column) void\n        +DropColumn(string columnName) void\n        +AlterColumn(string columnName, Column newColumn) void"
    )
    content = content.replace(
        "+InsertRow(Row row) void",
        "+InsertRow(Row row) void\n        +DeleteRow(Row row) void"
    )

    # Update Index
    content = content.replace(
        "+Search(object key) object",
        "+Search(object key) object\n        +Delete(object key) void"
    )

    # Update View
    content = content.replace(
        "+Create(string name, string query, Schema schema) View",
        "+Create(string name, string query, Schema schema) View\n        +AlterView(string newQuery) void\n        +DropView() void"
    )

    # Update StoredProcedure
    content = content.replace(
        "+ValidateParameters(object parameters) bool",
        "+ValidateParameters(object parameters) bool\n        +AlterProcedure(object newBody) void\n        +DropProcedure() void"
    )
    
    # Add Database class to the diagram if it doesn't exist
    if "class Database {" not in content:
        db_class = """
    class Database {
        +string Name
        +DropSchema(string schemaName) void
        +AlterSchema(string schemaName, object newSchema) void
    }

    Database *-- Schema
"""
        content = content.replace("class Schema {", db_class + "    class Schema {")

    with open(filename, 'w', encoding='utf-8') as f:
        f.write(content)

if __name__ == "__main__":
    update_file(r"c:\Users\ADMIN\Desktop\DBMS\docs\sequence-diagrams\database-object\README.md")
