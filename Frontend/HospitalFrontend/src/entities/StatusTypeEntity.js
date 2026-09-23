export const statusTypeEntity = {
    entity: "StatusTypes",
    operations: {
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "statusTypeCode", header: "StatusTypeCode" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "statusTypeCode",
            label: "StatusTypeCode",
            type: "text"
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "statusTypeCode",
            label: "StatusTypeCode",
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


