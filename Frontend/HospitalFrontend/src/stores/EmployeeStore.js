import { createEntityStore } from "./createEntityStore";
import { employeeEntity } from "../entities/EmployeeEntity";

const useEmployeeStore = createEntityStore(employeeEntity);
export default useEmployeeStore;