export const statusEntity = {
    entity: "Statuss",
    operations: {
search: {
    endpoint: "searchStatus",
    filters: [
        { name: "StatusTypeCode", label: "StatusTypeCode", type: "string", },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "statusName", header: "StatusName" },
        { field: "description", header: "Description" },
        { field: "statusTypeID", header: "StatusTypeID" },
        { field: "statusTypeCode", header: "StatusTypeCode" },
        { field: "isActive", header: "IsActive" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "statusName", header: "StatusName" },
        { field: "description", header: "Description" },
        { field: "statusTypeID", header: "StatusTypeID" },
        { field: "statusTypeCode", header: "StatusTypeCode" },
        { field: "isActive", header: "IsActive" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "statusName",
            label: "StatusName",
            type: "text"
        },

        {
            name: "description",
            label: "Description",
            type: "text"
        },

        {
            name: "statusTypeID",
            label: "StatusTypeID",
            type: "select",
            source: {
                entity: "StatusType",
                operation: "getAll",
                valueField: "id",
                displayField: "statusTypeCode"
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
            name: "statusName",
            label: "StatusName",
            type: "text"
        },

        {
            name: "description",
            label: "Description",
            type: "text"
        },

        {
            name: "statusTypeID",
            label: "StatusTypeID",
            type: "select",
            source: {
                entity: "StatusType",
                operation: "getAll",
                valueField: "id",
                displayField: "statusTypeCode"
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

