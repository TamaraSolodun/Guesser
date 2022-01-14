import { Store } from "../root";
import { IGame } from "./interfaces";

export const selectorGetGames = (store : Store) : Array<IGame> => {
    return store.gameRedux?.games;
}