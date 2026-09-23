export const applicationLogsEntity = {
    entity: "ApplicationLogs",
    operations: {
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "userID", header: "UserID" },
        { field: "logLevel", header: "LogLevel" },
        { field: "logMessage", header: "LogMessage" },
        { field: "exception", header: "Exception" },
        { field: "iPAddress", header: "IPAddress" },
        { field: "createdAt", header: "CreatedAt" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "userID",
            label: "UserID",
            type: "number"
        },

        {
            name: "logLevel",
            label: "LogLevel",
            type: "text"
        },

        {
            name: "logMessage",
            label: "LogMessage",
            type: "text"
        },

        {
            name: "exception",
            label: "Exception",
            type: "text"
        },

        {
            name: "iPAddress",
            label: "IPAddress",
            type: "text"
        },
    ]
},
    },
};

