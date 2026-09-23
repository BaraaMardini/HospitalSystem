export const DepartmentEntity = {
    entity: "Departments",
    operations: {
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "name", header: "Name" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "name",
            label: "Name",
            type: "text"
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
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};
