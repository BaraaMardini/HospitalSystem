export const specializationEntity = {
    entity: "Specializations",
    operations: {
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "id" },
        { field: "name", header: "name" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "name",
            label: "name",
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
            label: "name",
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
