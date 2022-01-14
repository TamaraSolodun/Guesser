import { Store } from "../root";
import { IUser } from "./interfaces";

export const selectorGetUser = (store : Store) : IUser => {
    return store.userRedux?.user;
}