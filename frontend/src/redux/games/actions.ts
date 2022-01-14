import {Dispatch} from '@reduxjs/toolkit';
import {GAME} from '../../core/_consts/api-urls';
import axios from '../../utils/API';
import {SAVE_GAME, GET_GAMES, SHOW_ALERT} from '../action-types';
import { showAlert } from '../user/actions';
import {GameConfig, IGame} from './interfaces';

export const getGames = (gameConfig: GameConfig) => async (dispatch: Dispatch): Promise<void> => {
  try {
    const response = await axios.post(GAME.GET_GAMES, gameConfig);
    dispatch({type: GET_GAMES, payload: response.data.data});
  } catch (error) {
    console.warn(error);
  }
};
export const createGame = (gameToCreate: IGame) => async (dispatch: Dispatch): Promise<void> => {
  try {
    const response = await axios.post(GAME.CREATE, gameToCreate);
    dispatch({type: SAVE_GAME, payload: response.data});
    dispatch(showAlert({type: "success", message: "The game successfully saved."}));
  } catch (error) {
    dispatch(showAlert({type: "error", message: "Game saving failed."}));
  }
};
