import { createEntityStore } from "./createEntityStore";
import { statusTypeEntity } from "../entities/StatusTypeEntity";

const useStatusTypeStore = createEntityStore(statusTypeEntity);
export default useStatusTypeStore;