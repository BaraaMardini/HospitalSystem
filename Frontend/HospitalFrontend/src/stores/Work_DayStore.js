import { createEntityStore } from "./createEntityStore";
import { work_DayEntity } from "../entities/Work_DayEntity";

const useWork_DayStore = createEntityStore(work_DayEntity);

export default useWork_DayStore;