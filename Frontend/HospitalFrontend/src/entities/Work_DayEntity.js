export const work_DayEntity = {
    entity: "Work_Days",
    operations: {
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "dayName", header: "DayName" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "dayName",
            label: "DayName",
            type: "text"
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "dayName",
            label: "DayName",
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

