export const roomTypeEntity = {
    entity: "RoomTypes",
    operations: {
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "typeName", header: "TypeName" },
        { field: "description", header: "Description" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "typeName",
            label: "TypeName",
            type: "text"
        },

        {
            name: "description",
            label: "Description",
            type: "text"
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "typeName",
            label: "TypeName",
            type: "text"
        },

        {
            name: "description",
            label: "Description",
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

