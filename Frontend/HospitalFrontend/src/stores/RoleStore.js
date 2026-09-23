// src/stores/RoleStore.js
import { createEntityStore } from "./createEntityStore";
import { roleEntity } from "../entities/RoleEntity";

const useRoleStore = createEntityStore(roleEntity);
export default useRoleStore;