export const permissionsEntity = {
    entity: "Permissionss",
    operations: {
search: {
    endpoint: "searchPermissions",
    filters: [
        { name: "ModuleName", label: "ModuleName", type: "string", },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "code", header: "Code" },
        { field: "name", header: "Name" },
        { field: "moduleName", header: "ModuleName" },
        { field: "actionName", header: "ActionName" },
        { field: "isActive", header: "IsActive" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "code", header: "Code" },
        { field: "name", header: "Name" },
        { field: "moduleName", header: "ModuleName" },
        { field: "actionName", header: "ActionName" },
        { field: "isActive", header: "IsActive" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "code",
            label: "Code",
            type: "text"
        },

        {
            name: "name",
            label: "Name",
            type: "text"
        },

        {
            name: "moduleName",
            label: "ModuleName",
            type: "text"
        },

        {
            name: "actionName",
            label: "ActionName",
            type: "text"
        },

        {
            name: "bitIndex",
            label: "BitIndex",
            type: "number"
        },

        {
            name: "bitValue",
            label: "BitValue",
            type: "text"
        },

        {
            name: "maskNumber",
            label: "MaskNumber",
            type: "number"
        },

        {
            name: "isActive",
            label: "IsActive",
            type: "checkbox"
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "name",
            label: "Name",
            type: "text"
        },

        {
            name: "isActive",
            label: "IsActive",
            type: "checkbox"
        },
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};

