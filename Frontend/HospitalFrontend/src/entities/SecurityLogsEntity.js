
export const securityLogsEntity = {
    entity: "SecurityLogss",

    operations: {
        getAll: {
            endpoint: "all",

            columns: [
                { field: "id", header: "ID" },
                { field: "userID", header: "User ID" },
                { field: "action", header: "Action" },
                { field: "description", header: "Description" },
                { field: "iPAddress", header: "IP Address" },
                { field: "createdAt", header: "Created At" },
            ],
        },
    },
};
