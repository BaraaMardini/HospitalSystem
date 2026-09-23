export const employeeEntity = {
    entity: "Employees",
    operations: {
search: {
    endpoint: "searchEmployee",
    filters: [
        { name: "Name", label: "Name", type: "select",         source: {
                entity: "People",
                operation: "getAll",
                valueField: "name",
                displayField: "name"
            } },
        { name: "RoleName", label: "RoleName", type: "select",         source: {
                entity: "Role",
                operation: "getAll",
                valueField: "roleName",
                displayField: "roleName"
            } },
        { name: "DepartmentName", label: "DepartmentName", type: "select",         source: {
                entity: "Department",
                operation: "getAll",
                valueField: "name",
                displayField: "name"
            } },
        { name: "StatusName", label: "StatusName", type: "select",         source: {
                entity: "Status",
                operation: "search",
                valueField: "statusName",
                displayField: "statusName"
            } },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "name", header: "Name" },
        { field: "personID", header: "PersonID" },
        { field: "roleName", header: "RoleName" },
        { field: "descriptionRole", header: "DescriptionRole" },
        { field: "departmentName", header: "DepartmentName" },
        { field: "salary", header: "Salary" },
        { field: "hireDate", header: "HireDate" },
        { field: "statusName", header: "StatusName" },
        { field: "descriptionSatus", header: "DescriptionSatus" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "name", header: "Name" },
        { field: "personID", header: "PersonID" },
        { field: "roleName", header: "RoleName" },
        { field: "descriptionRole", header: "DescriptionRole" },
        { field: "departmentName", header: "DepartmentName" },
        { field: "salary", header: "Salary" },
        { field: "hireDate", header: "HireDate" },
        { field: "statusName", header: "StatusName" },
        { field: "descriptionSatus", header: "DescriptionSatus" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "personID",
            label: "PersonName",
            type: "select",
            source: {
                entity: "People",
                operation: "getAll",
                valueField: "id",
                displayField: "name"
            }
        },

        {
            name: "roleId",
            label: "RoleName",
            type: "select",
            source: {
                entity: "Role",
                operation: "getAll",
                valueField: "id",
                displayField: "roleName"
            }
        },

        {
            name: "departmentId",
            label: "DepartmentName",
            type: "select",
            source: {
                entity: "Department",
                operation: "getAll",
                valueField: "id",
                displayField: "name"
            }
        },

        {
            name: "salary",
            label: "Salary",
            type: "number"
        },

      

        {
            name: "statusID",
            label: "StatusName",
            type: "select",
            source: {
                entity: "Status",
                operation: "search",
                valueField: "id",
                displayField: "statusName"
            }
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "roleId",
            label: "RoleName",
            type: "select",
             source: {
                entity: "Role",
                operation: "getAll",
                valueField: "id",
                displayField: "roleName"
            }
        },

        {
            name: "departmentId",
            label: "DepartmentName",
            type: "select",
          source: {
                entity: "Department",
                operation: "getAll",
                valueField: "id",
                displayField: "name"
            }
        },

        {
            name: "salary",
            label: "Salary",
            type: "number"
        },

        {
            name: "statusID",
            label: "StatusName",
            type: "select",
             source: {
                entity: "Status",
                operation: "search",
                valueField: "id",
                displayField: "statusName"
            }
        },
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};

