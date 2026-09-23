import { createEntityStore } from "./createEntityStore";
import { statusEntity } from "../entities/StatusEntity";

const useStatusStore = createEntityStore(statusEntity);
export default useStatusStore;