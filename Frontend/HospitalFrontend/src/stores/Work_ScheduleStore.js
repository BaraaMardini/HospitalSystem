import { createEntityStore } from "./createEntityStore";
import { work_ScheduleEntity } from "../entities/Work_ScheduleEntity";

const useWork_ScheduleStore = createEntityStore(work_ScheduleEntity);

export default useWork_ScheduleStore;