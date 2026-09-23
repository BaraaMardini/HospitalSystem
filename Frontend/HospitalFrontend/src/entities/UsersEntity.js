export const usersEntity = {
  entity: "Userss", // يطابق [Route("api/userss")]
  title: "Users",
  description: "Manage system users, their roles and granular permissions.",
  addLabel: "Add User",
  idField: "id",
  operations: {
    search: {
      endpoint: "SearchUsers",
      filters: [
        { name: "Username", label: "Username", type: "string" },
        {
          name: "IsActive",
          label: "Status",
          type: "select",
          options: [
            { value: "true", label: "Active" },
            { value: "false", label: "Inactive" },
          ],
        },
      ],
      columns: [
        { field: "id", header: "ID" },
        { field: "username", header: "Username" },
        { field: "roleName", header: "Role" },
        { field: "isActive", header: "Status" },
      ],
    },
    getAll: {
      endpoint: "all",
      columns: [
        { field: "id", header: "ID" },
        { field: "username", header: "Username" },
        { field: "roleName", header: "Role" },
        { field: "isActive", header: "Status" },
      ],
    },
    add: {
      endpoint: "",
      fields: [
        { name: "username", label: "Username", type: "text", required: true },
        { name: "passwordHash", label: "Password", type: "password", required: true },
        {
          name: "roleID",
          label: "RoleName",
          type: "select",
          required: true,
          source: {
            entity: "Role",
            operation: "getAll",
            valueField: "id",
            displayField: "roleName",
          },
        },
        {
          name: "permissions",
          label: "Permissions",
          type: "select",
          multiple: true,
          source: {
            entity: "Permissions",
            operation: "getAll",
            valueField: "id",
            displayField: "name",
            groupBy: "moduleName",
          },
        },
        { name: "isActive", label: "IsActive", type: "checkbox" },
      ],
    },
    update: {
      // Maps to: PUT api/userss/{username}/{passwordhash}
      // createEntityApi.js only knows how to replace a single "{value}"
      // placeholder in the endpoint template, so we build the composite
      // "username/passwordHash" segment ourselves in UsersPage.js
      // (see buildKey) and pass it in as that single value.
      // DO NOT put "{username}/{passwordHash}" here — createEntityApi
      // only replaces "{value}", so that string would be sent literally.
      endpoint: "{value}",
      fields: [
        {
          name: "passwordHash",
          label: "Current Password",
          type: "password",
          required: true,
        },
        {
          name: "roleID",
          label: "RoleName",
          type: "select",
          source: {
            entity: "Role",
            operation: "getAll",
            valueField: "id",
            displayField: "roleName",
          },
        },
        { name: "isActive", label: "IsActive", type: "checkbox" },
      ],
    },
    delete: {
      // Same reasoning as `update` above — composite key built as a single
      // "{value}" string by UsersPage.js's buildKey().
      endpoint: "{value}",
    },
  },
};