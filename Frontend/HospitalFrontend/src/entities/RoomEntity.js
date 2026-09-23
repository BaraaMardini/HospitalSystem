export const roomEntity = {
    entity: "Rooms",
    operations: {
search: {
    endpoint: "searchRoom",
    filters: [
        { name: "RoomNumber", label: "RoomNumber", type: "string", },
        { name: "TypeName", label: "TypeName", type: "string", },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "roomNumber", header: "RoomNumber" },
        { field: "typeName", header: "TypeName" },
        { field: "description", header: "Description" },
        { field: "notes", header: "Notes" },
        { field: "isActive", header: "IsActive" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "roomNumber", header: "RoomNumber" },
        { field: "typeName", header: "TypeName" },
        { field: "description", header: "Description" },
        { field: "notes", header: "Notes" },
        { field: "isActive", header: "IsActive" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "roomNumber",
            label: "RoomNumber",
            type: "text"
        },

        {
            name: "notes",
            label: "Notes",
            type: "text"
        },

        {
            name: "roomTypeID",
            label: "RoomType",
            type: "select",
            source: {
                entity: "RoomType",
                operation: "getAll",
                valueField: "id",
                displayField: "typeName"
            }
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
            name: "roomNumber",
            label: "RoomNumber",
            type: "text"
        },

        {
            name: "notes",
            label: "Notes",
            type: "text"
        },

        {
            name: "roomTypeID",
            label: "RoomType",
            type: "select",
            source: {
                entity: "RoomType",
                operation: "getAll",
                valueField: "id",
                displayField: "typeName"
            }
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

