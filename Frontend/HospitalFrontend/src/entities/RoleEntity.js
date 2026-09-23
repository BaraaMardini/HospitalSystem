export const roleEntity = {
    entity: "Roles",
    operations: {
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "roleName", header: "RoleName" },
        { field: "description", header: "description" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "roleName",
            label: "RoleName",
            type: "text"
        },

        {
            name: "description",
            label: "description",
            type: "text"
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "roleName",
            label: "RoleName",
            type: "text"
        },

        {
            name: "description",
            label: "description",
            type: "text"
        },
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};

