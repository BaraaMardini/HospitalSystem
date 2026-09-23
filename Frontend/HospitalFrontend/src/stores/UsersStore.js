import { createEntityStore } from "./createEntityStore";
import { usersEntity } from "../entities/UsersEntity";

const useUsersStore = createEntityStore(usersEntity);

export default useUsersStore;