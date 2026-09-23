import { createEntityStore } from "./createEntityStore";
import { DepartmentEntity } from "../entities/DepartmentEntity";

const useDepartmentStore = createEntityStore(DepartmentEntity);

export default useDepartmentStore;
