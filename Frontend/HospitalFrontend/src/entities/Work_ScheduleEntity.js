export const work_ScheduleEntity = {
    entity: "Work_Schedules",
    operations: {
search: {
    endpoint: "searchWork_Schedule",
    filters: [
        { name: "DayName", label: "DayName", type: "string", },
        { name: "DoctorID", label: "DoctorID", type: "int", },
        { name: "PeopleName", label: "PeopleName", type: "string", },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "dayName", header: "DayName" },
        { field: "doctorID", header: "DoctorID" },
        { field: "peopleName", header: "PeopleName" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "dayName", header: "DayName" },
        { field: "doctorID", header: "DoctorID" },
        { field: "peopleName", header: "PeopleName" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "doctorID",
            label: "DoctorName",
            type: "select",
            source: {
                entity: "Doctor",
                operation: "getAll",
                valueField: "id",
                displayField: "personName"
            }
        },

        {
            name: "dayID",
            label: "DayName",
            type: "select",
            source: {
                entity: "Work_Day",
                operation: "getAll",
                valueField: "id",
                displayField: "dayName"
            }
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "doctorID",
            label: "DoctorName",
            type: "select",
            source: {
                entity: "Doctor",
                operation: "getAll",
                valueField: "id",
                displayField: "personName"
            }
        },

        {
            name: "dayID",
            label: "DayName",
            type: "select",
            source: {
                entity: "Work_Day",
                operation: "getAll",
                valueField: "id",
                displayField: "dayName"
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