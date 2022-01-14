import { GameResultType } from '../../../core/_consts/enums';
import {GET_GAMES, SAVE_GAME} from '../../action-types';

//#region Interfaces
export interface IGame {
  id: number;
  gameResultType: GameResultType,
  playerId: number;
  playedAt: string;
}

export interface GameConfig {
  resultType: GameResultType;
  playedDateFrom: string;
  playedDateTo: string;
  playerId: number;
}
//#endregion

//#region Actions
export interface ICreateGame {
  type: typeof SAVE_GAME;
  payload: IGame;
}
export interface IGetGames {
  type: typeof GET_GAMES;
  payload: Array<IGame>;
}
//#endregion

//#region reducer
export interface IGameReducer {
  games: Array<IGame>;
}
//#endregion

export type GameActions = ICreateGame | IGetGames;
