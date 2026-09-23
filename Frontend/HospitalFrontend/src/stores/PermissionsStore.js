import { createEntityStore } from "./createEntityStore";
import { permissionsEntity } from "../entities/PermissionsEntity";

const usePermissionsStore = createEntityStore(permissionsEntity);

export default usePermissionsStore;