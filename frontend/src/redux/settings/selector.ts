import { Store } from "../root";
import { ISettings } from "./interfaces";

export const selectorGetSettings = (store : Store) : any => {
    return store.settingsRedux;
}