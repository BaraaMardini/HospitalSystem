import { createEntityStore } from "./createEntityStore";
import {PeopleEntity} from "../entities/PeopleEntity";

const usePeopleStore = createEntityStore(PeopleEntity);

export default usePeopleStore;