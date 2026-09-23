import { createEntityStore } from "./createEntityStore";
import { securityLogsEntity } from "../entities/SecurityLogsEntity";

const useSecurityLogsStore = createEntityStore(securityLogsEntity);

export default useSecurityLogsStore;