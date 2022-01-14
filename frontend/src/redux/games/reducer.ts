import {GET_GAMES, SAVE_GAME} from '../action-types';
import {IGame, GameActions, IGameReducer} from './interfaces';

const gameInitState:IGameReducer = {games:[]};

export const gameReducer = (state = gameInitState, actions: GameActions) : IGameReducer => {
  const {type, payload} = actions;
  switch (type) {
    case GET_GAMES:
      return {...state, games: payload as Array<IGame>};
    case SAVE_GAME:
      return {...state, games: [...state.games, payload as IGame]};
    default:
      return state;
  }
};

export default gameReducer;
